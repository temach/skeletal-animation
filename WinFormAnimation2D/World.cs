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
        private Entity ent = null;

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

            ent = new Entity(Current_Scene);
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
        /// Render the model stored in EntityScene useing the Graphics object.
        /// </summary>
        public void RenderModel(Graphics g)
        {
            ent.RenderModel(g);
        }


    }   // end of class World
}

