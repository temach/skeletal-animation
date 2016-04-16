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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Assimp.Configs;
using d2d = System.Drawing.Drawing2D;
using tk = OpenTK;
using Matrix4 = OpenTK.Matrix4;

namespace WinFormAnimation2D
{

    // Node with extended properties
    class BoneNode
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

        public BoneNode Parent;
        public List<BoneNode> Children;

        public BoneNode(Node assimp_node)
        {
            _inner = assimp_node;
            Children = new List<BoneNode>(assimp_node.ChildCount);
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
        public void ApplyAnimation(BoneNode armature, ActionState st)
        {
            ChangeLocalFixedDataBlend(st);
            ReCalculateGlobalTransform(armature);
        }

        /// <summary>
        /// Function to blend from one keyframe to another.
        /// </summary>
        public void ChangeLocalFixedDataBlend(ActionState st)
        {
            Debug.Assert(0 <= st.KfBlend && st.KfBlend <= 1);
            foreach (NodeAnimationChannel channel in _action.NodeAnimationChannels)
            {
                BoneNode bone_nd = _scene.GetBoneNode(channel.NodeName);
                // now rotation
                tk.Quaternion target_roto = tk.Quaternion.Identity;
                if (channel.RotationKeyCount > st.TargetKeyframe)
                {
                    target_roto = channel.RotationKeys[st.TargetKeyframe].Value.eToOpenTK();
                }
                tk.Quaternion start_frame_roto = channel.RotationKeys[st.OriginKeyframe].Value.eToOpenTK();
                tk.Quaternion result_roto = tk.Quaternion.Slerp(start_frame_roto, target_roto, (float)st.KfBlend);
                // now translation
                tk.Vector3 target_trans = tk.Vector3.Zero;
                if (channel.PositionKeyCount > st.TargetKeyframe)
                {
                    target_trans = channel.PositionKeys[st.TargetKeyframe].Value.eToOpenTK();
                }
                tk.Vector3 cur_trans = channel.PositionKeys[st.OriginKeyframe].Value.eToOpenTK();
                tk.Vector3 result_trans = cur_trans + tk.Vector3.Multiply(target_trans - cur_trans, (float)st.KfBlend);
                // combine rotation and translation
                tk.Matrix4 result = tk.Matrix4.CreateFromQuaternion(result_roto);
                result.Row3.Xyz = result_trans;
                bone_nd.LocTrans = result.eToAssimp();
            }
        }

        // Updates global transforms by walking the hierarchy 
        private void ReCalculateGlobalTransform(BoneNode nd)
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

}


