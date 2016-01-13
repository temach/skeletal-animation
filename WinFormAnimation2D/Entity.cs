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


namespace WinFormAnimation2D
{
    class Bone
    {
        public Point[] points = null;
        public Matrix matrix = null;
    }
    

    class Entity
    {
        // where the entity keeps its information
        public Scene EntityScene = null;

        // the only public constructor
        public Entity(Scene sc)
        {
            EntityScene = sc;
        }

        /// <summary>
        /// Render the model stored in EntityScene useing the Graphics object.
        /// </summary>
        public void RenderModel(Graphics g)
        {
            RecursiveRender(g, EntityScene.RootNode);
        }

        //-------------------------------------------------------------------------------------------------
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        private void RecursiveRender(Graphics g, Node nd)
        {
            Matrix4x4 mat44 = nd.Transform;

            var tmp = mat44.eTo3x2();
            g.MultiplyTransform(tmp);

            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = EntityScene.Meshes[mesh_id];
                foreach (Face cur_face in cur_mesh.Faces)
                {
                    // list of 3 vertices
                    var tri_vertices = cur_face.Indices.Select(i => cur_mesh.Vertices[i].eToPointFloat()).ToArray();

                    // choose random brush color for this triangle
                    var br = Util.GetNextBrush();
                    g.FillPolygon(br, tri_vertices);

                    // Bad code to draw a single point. Better use DrawEllipse. But too lazy.
                    foreach(var p in tri_vertices)
                    {
                        g.eDrawPoint(p);
                    }
                }
            }

            // draw all children
            foreach (Node child in nd.Children)
            {
                RecursiveRender(g, child);
            }
        }


    } // end of class
}
