using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;        // for MemoryStream
using System.Reflection;
using OpenTK;
using System.Diagnostics;


namespace WinFormAnimation2D
{
    class ArmatureEntity
    {
        public Node _armature;
        public Scene _scene;

        public ArmatureEntity(Scene sc, Node arma)
        {
            _armature = arma;
            _scene = sc;
        }


        public void RenderBone()
        {
            // just draw a random line for now
            Util.GR.eDrawPoint(new Point(0, 0));
            Util.GR.DrawLine(Util.pp2, new Point(0, 0), new Point(40, 40));
            Util.GR.eDrawPoint(new Point(40, 40));
        }

        //-------------------------------------------------------------------------------------------------
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        // one mesh, one bone policy
        private void RecursiveRenderSystemDrawing(Node nd)
        {
            Util.PushMatrix();
            Matrix4x4 mat44 = nd.Transform;
            Util.GR.MultiplyTransform(mat44.eTo3x2());
            RenderBone();
            foreach (Node child in nd.Children)
            {
                RecursiveRenderSystemDrawing(child);
            }
            // we don't want to apply this branch transform to the next branch
            Util.PopMatrix();
        }

    } // end of class

}
