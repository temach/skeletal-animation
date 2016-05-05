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
        public BoneNode _armature;
        public Scene _scene;

        public ArmatureEntity(Scene sc, BoneNode arma)
        {
            _armature = arma;
            _scene = sc;
        }


        public void RenderBone()
        {
        }

        //-------------------------------------------------------------------------------------------------
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        // one mesh, one bone policy
        private void RecursiveRenderSystemDrawing(Node nd)
        {
        }

    } // end of class

}
