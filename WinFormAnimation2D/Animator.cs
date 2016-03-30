using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;        // for MemoryStream
using System.Reflection;
using System.Diagnostics;
using Assimp;
using Assimp.Configs;
using d2d = System.Drawing.Drawing2D;
using tk = OpenTK;
using Matrix4 = OpenTK.Matrix4;

namespace WinFormAnimation2D
{

    // Node with extended properties
    class NodeWrapper
    {
        public Node _inner;
        public Matrix4 GlobalTransform;
        public Matrix4x4 GlobTrans
        {
            get { return GlobalTransform.eToAssimp(); }
            set { GlobalTransform = value.eToOpenTK(); }
        }
        public Matrix4 LocalTransform;
        public Matrix4x4 LocTrans
        {
            get { return LocalTransform.eToAssimp(); }
            set {  LocalTransform = value.eToOpenTK(); }
        }

        public NodeWrapper Parent;
        public List<NodeWrapper> Children;

        public NodeWrapper(Node assimp_node)
        {
            _inner = assimp_node;
            Children = new List<NodeWrapper>(assimp_node.ChildCount);
        }

    }


    // our job is to update the skeleton. 
    // Entities should look up its status (current node transforms) during their rendering
    // , to make sure they are syncronised in position/rotation.
    //
    // Node animator knows about an action. It can perform the action on a given armature.
    // So it does: Snap this particular armature to this particular pose
    class NodeInterpolator
    {
        // Animation is what blender calls "action"
        // It is a set of keyframes that describe some action
        public Animation _action;
        public SceneWrapper _scene;

        public  NodeInterpolator(SceneWrapper sc, Animation action)
        {
            _scene = sc;
            _action = action;
        }

        // Update this particular armature to this particular frame in action (to this particular keyframe)
        public void ApplyAnimation(NodeWrapper armature, ActionState st)
        {
            ChangeLocalFixedDataBlend(st);
            ReCalculateGlobalTransform(armature);
        }

        /// <summary>
        /// Function to blend from one keyframe to another.
        /// </summary>
        public void ChangeLocalFixedDataBlend(ActionState st)
        {
            Debug.Assert(0 <= st.Blend && st.Blend <= 1);
            foreach (NodeAnimationChannel channel in _action.NodeAnimationChannels)
            {
                NodeWrapper bone_nd = _scene.FindNodeWrapper(channel.NodeName);
                // now rotation
                tk.Quaternion target_roto = tk.Quaternion.Identity;
                if (channel.RotationKeyCount > st.TargetKeyframe)
                {
                    target_roto = channel.RotationKeys[st.TargetKeyframe].Value.eToOpenTK();
                }
                tk.Quaternion start_frame_roto = channel.RotationKeys[st.OriginKeyframe].Value.eToOpenTK();
                tk.Quaternion result_roto = tk.Quaternion.Slerp(start_frame_roto, target_roto, (float)st.Blend);
                // now translation
                tk.Vector3 target_trans = tk.Vector3.Zero;
                if (channel.PositionKeyCount > st.TargetKeyframe)
                {
                    target_trans = channel.PositionKeys[st.TargetKeyframe].Value.eToOpenTK();
                }
                tk.Vector3 cur_trans = channel.PositionKeys[st.OriginKeyframe].Value.eToOpenTK();
                tk.Vector3 result_trans = cur_trans + tk.Vector3.Multiply(target_trans - cur_trans, (float)st.Blend);
                // combine rotation and translation
                tk.Matrix4 result = tk.Matrix4.CreateFromQuaternion(result_roto);
                result.Row3.Xyz = result_trans;
                bone_nd.LocTrans = result.eToAssimp();
            }
        }

        // Updates global transforms by walking the hierarchy 
        private void ReCalculateGlobalTransform(NodeWrapper nd)
        {
            if (nd.Parent != null)
            {
                nd.GlobalTransform = nd.LocalTransform * nd.Parent.GlobalTransform;
            }
            else
            {
                // only armature root bone has parent == null
                nd.GlobalTransform = nd.LocalTransform;
            }
            foreach (var child in nd.Children)
            {
                ReCalculateGlobalTransform(child);
            }
        }

    }

    /// <summary>
    /// This class knows what argumets to pass to NodeInterpolator 
    /// </summary>
    class ActionState
    {
        public Animation _action;

        public List<double> KeyframeTimes;
        public int KeyframeCount
        {
            get { return KeyframeTimes.Count; }
        }

        public string Name
        {
            get { return _action.Name; }
        }
        public double TotalDurationSeconds
        {
            get { return _action.DurationInTicks * _action.TicksPerSecond; }
        }
        public double TotalDurationTicks
        {
            get { return _action.DurationInTicks; }
        }

        public double CurrentTimeSeconds
        {
            get
            {
                double interval = (KeyframeTimes[TargetKeyframe] - KeyframeTimes[OriginKeyframe]);
                return KeyframeTimes[OriginKeyframe] + interval * Blend;
            }
        }

        // TickPerSec can be used to change speed.
        private double _tps;
        public double TickPerSec
        {
            get { return _tps; }
            set { _tps = value; }
        }

        // start or origin keyframe
        private int _origin_keyframe;
        public int OriginKeyframe
        {
            get { return _origin_keyframe; }
            set
            {
                // Note: frame is strictly less than KeyframeCount
                if (0 <= value && value < KeyframeCount)
                {
                     _origin_keyframe = value;
                }
            }
        }

        // end or target keyframe
        private int _target_keyframe;
        public int TargetKeyframe
        {
            get { return _target_keyframe; }
            set
            {
                // Note: frame is strictly less than KeyframeCount
                if (0 <= value && value < KeyframeCount)
                {
                     _target_keyframe = value;
                }
            }
        }

        // 0.0 <= Blend <= 1.0, how much in between two keyframes are we
        private double _blend;
        public double Blend
        {
            get { return _blend; }
            set { _blend = Math.Min(Math.Max(0, value), 1.0); }
        }

        public ActionState(Animation action)
        {
            SetCurrentAction(action);
        }

        public void ForwardKeyframe()
        {
            OriginKeyframe = TargetKeyframe;
            TargetKeyframe += 1;
        }
        public void BackwardKeyframe()
        {
            OriginKeyframe = TargetKeyframe;
            TargetKeyframe -= 1;
        }

        public void SetCurrentAction(Animation action)
        {
            _action = action;
            _tps = action.TicksPerSecond;
            Blend = 0; 
            // Keyframe times must be initialised before Origin/Target Keyframes
            KeyframeTimes = _action.NodeAnimationChannels[0].PositionKeys.Select(vk => vk.Time).ToList();
            OriginKeyframe = 0;
            TargetKeyframe = 0;
        }

        public int FindStartFrameAtTime(double time_ticks)
        {
            Debug.Assert(time_ticks >= 0);
            for (int i = 0; i < KeyframeCount; i++)
            {
                if (time_ticks < KeyframeTimes[i])
                {
                    return i - 1;
                }
            }
            // return last frame if not found (because of numerical inaccuracies?)
            return KeyframeCount - 1;
        }

        //Note: all the calculations here are done in ticks.
        public void SetTime(double time_seconds)
        {            
            double time_ticks = time_seconds * TickPerSec;
            // when time overflows we loop by default
            double time = time_ticks % TotalDurationTicks;
            int start_frame = FindStartFrameAtTime(time_seconds);
            int end_frame = (start_frame + 1) % KeyframeCount;
            double delta_time = KeyframeTimes[end_frame] - KeyframeTimes[start_frame];
            // when we looped the animation
            if (delta_time < 0.0)
            {
                delta_time += TotalDurationTicks;
            }
            double blend = (time - KeyframeTimes[start_frame]) / delta_time;
            // assign results
            OriginKeyframe = start_frame;
            TargetKeyframe = end_frame;
            Blend = blend;
        }

    }
}


