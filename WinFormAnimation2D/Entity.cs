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

    class BoundingBox
    {
        public Vector3D _zero_near = new Vector3D(1e10f, 1e10f, 1e10f);
        public Vector3D _zero_far = new Vector3D(-1e10f, -1e10f, -1e10f);
        public Vector3D _center = new Vector3D(0, 0, 0);

        public BoundingBox(Scene sc)
        {
            Matrix4x4 id = Matrix4x4.Identity;

            CalcBoundingBox(sc, sc.RootNode, ref _zero_near, ref _zero_far, ref id);
        }

        /// <summary>
        /// For each node calculate the bounding box.
        /// This is used to align the viewport nicely when the scene is imported.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="node"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="cur_mat"></param>
        private void CalcBoundingBox(Scene sc, Node node, ref Vector3D min, ref Vector3D max, ref Matrix4x4 cur_mat)
        {
            Matrix4x4 prev = cur_mat;
            cur_mat = prev * node.Transform;

            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = sc.Meshes[index];
                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        Vector3D cur_vertex = mesh.Vertices[i];
                        cur_vertex = cur_mat.eTransformVector(cur_vertex);

                        min.X = Math.Min(min.X, cur_vertex.X);
                        min.Y = Math.Min(min.Y, cur_vertex.Y);
                        min.Z = Math.Min(min.Z, cur_vertex.Z);

                        max.X = Math.Max(max.X, cur_vertex.X);
                        max.Y = Math.Max(max.Y, cur_vertex.Y);
                        max.Z = Math.Max(max.Z, cur_vertex.Z);
                    }
                }
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                CalcBoundingBox(sc, node.Children[i], ref min, ref max, ref cur_mat);
            }

            // unwind the matrix stack
            // when we calculate the box for the next leaf
            cur_mat = prev;
        }

        // public BoundingBox(Vector3D zero_near, Vector3D zero_far)
        // {
        //     _zero_near = zero_near;
        // 	_zero_far = zero_far;
        //     _center = (zero_near + zero_far) / 2.0f;
        // }
    }

    /// <summary>
    /// Represents the currently loaded scene. There can only be one Entity loaded at a time.
    /// </summary>
    class Entity
    {
        // MAtrix to track the state of the entity
        public Matrix _ent_mat = new Matrix();

        // where the entity keeps its information
        public Scene _ent_scene = null;

        public BoundingBox _ent_box = null;

        // the only public constructor
        public Entity(Scene sc)
        {
            _ent_scene = sc;
            _ent_box = new BoundingBox(sc);
        }

        /// <summary>
        /// Change the matrices for each node to 
        /// match the orientaitons in the keyframe data.
        /// Then during rendering the mesh will look different.
        /// </summary>
        public void NextKeyframe()
        {
            return;
        }


        // setup specific to this scene what other objects do not know about. (wireframe, texture,material,scale...)
        // this gets the currently active globale settings for the program.
        // it looks at them and chooses the best settings for itself taking into 
        // consideration the globals. So it tries to get the scene looking ideal while 
        // still respecting global user settings (like: draw in wireframe, or without texture)
        // that are currently turned on in the application. This settings are 
        // activated back in the DrawToOpenGL class. After their activation we call the 
        // render method on this particular object that pushes vertices (not settings) to OpenGL.
        // This object should have some render code.

        public DrawSettings GetRenderSettings(DrawSettings state)
        {
            return new DrawSettings
            {
                _enableTexture2D = true,
                _enableDepthTest = true,
                _enableFaceCounterClockwise = true,
                _enablePerspectiveCorrectionHint = true,
                _enablePolygonModeFill = true,
            };
        }

        /// <summary>
        /// Render the model stored in EntityScene useing the Graphics object.
        /// </summary>
        public void RenderModel(Graphics g)
        {
            g.MultiplyTransform(_ent_mat);
            RecursiveRender(g, _ent_scene.RootNode);
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
            var saved = g.Save();
            g.MultiplyTransform(tmp);

            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _ent_scene.Meshes[mesh_id];
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

            // After drawing the left node. unwind the matrix stack.
            // so we don't apply this leaf's transform to the next leaf.
            g.Restore(saved);
        }

    } // end of class

}
