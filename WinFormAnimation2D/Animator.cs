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


namespace WinFormAnimation2D
{

    class NodeAnimator
    {
        // Animation is what blender calls "action"
        // It is a set of keyframes that describe some action
        public Animation _action;
        public Scene _scene;
        public int _frame;

        public  NodeAnimator(Scene sc, Animation action)
        {
            _action = action;
            _scene = sc;
        }

        private Node InnerRecurFindNodeInScene(Node cur_node, string node_name)
        {
            if (cur_node.Name == node_name)
            {
                return cur_node;
            }
            foreach (var child in cur_node.Children)
            {
                var tmp =  InnerRecurFindNodeInScene(child, node_name);
                if (tmp != null)
                {
                    return tmp;
                }
            }
            return null;
        }

        // our job is to update the skeleton. 
        // Entities should look up its status (current node transforms) during their rendering
        // , to make sure they are syncronised in position/rotation.
        public void SnapToKeyframe(int frame)
        {
            if (frame < 0 || frame >= _action.NodeAnimationChannels[0].PositionKeyCount)
            {
                MessageBox.Show("invalid frame " + frame);
                return;
            }
            foreach (NodeAnimationChannel channel in _action.NodeAnimationChannels)
            {
                Node nd = InnerRecurFindNodeInScene(_scene.RootNode, channel.NodeName);
                tk.Quaternion roto = channel.RotationKeys[frame].Value.eToOpenTK();
                tk.Vector3 trans = channel.PositionKeys[frame].Value.eToOpenTK();
                // snap rotation and translation. See the long comment to NodeAnimationChannel class.
                tk.Matrix4 mat_new = tk.Matrix4.CreateFromQuaternion(roto);
                mat_new.Row3.Xyz = trans;
                nd.Transform = mat_new.eToAssimp();
            }
            _frame = frame;
        }

    }
}


