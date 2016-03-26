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
    class NodeAnimator
    {
        // Animation is what blender calls "action"
        // It is a set of keyframes that describe some action
        public Animation _action;
        public SceneWrapper _scene;
        public int _keyframe;

        public  NodeAnimator(SceneWrapper sc, Animation action)
        {
            _action = action;
            _scene = sc;
        }

        // Update this particular armature to this particular frame in action (to this particular keyframe)
        public void SnapToKeyframe(NodeWrapper armature, int keyframe)
        {
            if (keyframe < 0 || keyframe >= _action.NodeAnimationChannels[0].PositionKeyCount)
            {
                MessageBox.Show("invalid frame " + keyframe);
                return;
            }
            ChangeLocalTransform(keyframe);
            ChangeGlobalTransform(armature);
            _keyframe = keyframe;
        }

        // For each channel update bone local transforms
        private void ChangeLocalTransform(int frame)
        {
            foreach (NodeAnimationChannel channel in _action.NodeAnimationChannels)
            {
                NodeWrapper bone_nd = _scene.FindNodeWrapper(channel.NodeName);
                tk.Quaternion roto = tk.Quaternion.Identity;
                if (channel.RotationKeyCount > frame)
                {
                    roto = channel.RotationKeys[frame].Value.eToOpenTK();
                }
                tk.Vector3 trans = tk.Vector3.Zero;
                if (channel.PositionKeyCount > frame)
                {
                    trans = channel.PositionKeys[frame].Value.eToOpenTK();
                }
                // snap rotation and translation. See the long comment to NodeAnimationChannel class.
                tk.Matrix4 mat_new = tk.Matrix4.CreateFromQuaternion(roto);
                mat_new.Row3.Xyz = trans;
                bone_nd.LocTrans = mat_new.eToAssimp();
            }
        }

        // Updates global transforms by walking the hierarchy 
        private void ChangeGlobalTransform(NodeWrapper nd)
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
                ChangeGlobalTransform(child);
            }
        }

    }
}


