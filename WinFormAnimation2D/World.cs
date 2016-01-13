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
    class World
    {
        /// debug stuff
        public List<Point> debug_vertices = new List<Point>();
        public Matrix debug_mat = null;
        public Point[] debug_points = null;
        public Random rand = new Random();
        public int rand_brush_count = 0;
        
        /// This is a singleton
        private static bool hasInit = false;
        
        /// Currently loaded assimp scene
        private static Scene Current_Scene = null;

        public World() {
            if (hasInit == true)
            {
                throw new Exception("Class World is a singleton. Can not create more than one instance.");
            }

            //Filepath to our model
            // byte[] filedata = Encoding.UTF8.GetBytes(Properties.Resources.sphere_3d);
            byte[] filedata = Properties.Resources.bird_plane_8;

            MemoryStream sphere = new MemoryStream(filedata);

            // use format "nff" for sphere_3d
            // LoadModel(sphere, "nff");

            // use format "dae" for bird_plane
            LoadModel(sphere, "dae");

            // use format "obj" for bird_plane_5
            // LoadModel(sphere, "obj");
        }

        /// <summary>
        /// Get a brush of random color.
        /// </summary>
        /// <returns></returns>
        public Brush GetRandBrush()
        {
            rand_brush_count++;
            switch (rand_brush_count % 4)
            {
                case 0:
                    return Brushes.LawnGreen;
                case 1:
                    return Brushes.Green;
                case 2:
                    return Brushes.GreenYellow;
                case 3:
                    return Brushes.LightSeaGreen;
                default:
                    return Brushes.Red;
            }
        }

        public void LoadModel(MemoryStream model_data, string format_hint)
        {
            //Create a new importer
            AssimpContext importer = new AssimpContext();

            //This is how we add a configuration (each config is its own class)
            // NormalSmoothingAngleConfig config = new NormalSmoothingAngleConfig(66.0f);
            // importer.SetConfig(config);

            //This is how we add a logging callback 
            LogStream logstream = new LogStream(delegate (String msg, String userData) {
                Console.WriteLine(msg);
            });
            logstream.Attach();

            //Import the model. All configs are set. The model
            //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
            Current_Scene = importer.ImportFileFromStream(model_data
                , PostProcessPreset.TargetRealTimeMaximumQuality
                , format_hint);

            // TODO: load data into your own data structures.

            //Dont need the raw data. We have the Current_Scene reference
            importer.Dispose();
        }

        /// <summary>
        /// Render the model stored in Current_Scene useing the Graphics object.
        /// </summary>
        public void RenderModel(Graphics g)
        {
            RecursiveRender(g, Current_Scene.RootNode);
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
                Mesh cur_mesh = Current_Scene.Meshes[mesh_id];
                foreach (Face cur_face in cur_mesh.Faces)
                {
                    // list of 3 vertices
                    var tri_vertices = cur_face.Indices.Select(i => cur_mesh.Vertices[i].eToPointFloat()).ToArray();

                    // choose random brush color for this triangle
                    var br = GetRandBrush();
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

    }   // end of class World
}

