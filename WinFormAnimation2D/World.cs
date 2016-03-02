﻿/// @file
/// 
/// @mainpage
/// This program is a demo of skeletal animation in C#.
/// Feed it collada files with animation and bone data.
/// 
/// In blender spread the mesh on X-Y plane (so Z is normal vector)
/// Also make it in the X>0, Y>0 quadrant.
/// When importing a collada file remember to modify it by hand. 
/// You should change Z_UP to Y_UP. (I think this is only necessary 
/// while we work with Grpahics object, when we go to opengl this should fix itself)
/// 

/// row/column major applies to a few different things.
/// First it applies to _notation_
/// let's say matrix A and vector v and after applying matrix A on vector v we get v' as result.

/// This is column major: (This is the prefered notation for coding and writing OpenGL)
/// (projection x view x model x vector) 
/// So: v' = A * v
///      [ x.x x.y x.z 0 ]     [ v.x ]
/// v' = | y.x y.y y.z 0 |  *  [ v.y ]
///      | z.x z.y z.z 0 |     [ v.z ]
///      [ p.x p.y p.z 1 ]     [  1  ]

/// Now some row major notation: 
/// (and you multiply in shader: gl_Position = vec4(v_xy, 0.0, 1.0) * v_ViewMatrix * v_ProjectionMatrix )
/// (vector x model x view x projection)
/// v' = v * A
///                              [ x.x y.x z.x p.x ]  
/// v' = [ v.x, v.y, v.z, 1  ] * | x.y y.y z.y p.y |  
///                              | x.z y.z z.z p.z |  
///           					 [  0   0   0   1  ]  
/// You can see that the v' vector will be the same in both cases.
/// That's why this stuff does not matter. Just document which way you use.
/// see http://stackoverflow.com/questions/17717600/confusion-between-c-and-opengl-matrix-order-row-major-vs-column-major


/// About matrices in memory:
/// Assimp is row major (the first 4 elemtns of array are elemtns of the first row)
/// OpenGL user column-major (the first four elemnts of array are elements of the first column)
/// I do not know about OpenTK.

/// For programming purposes, OpenGL matrices are 16-value arrays 
/// with base vectors laid out contiguously in memory. The translation components 
/// occupy the 13th, 14th, and 15th elements of the 16-element matrix, where 
/// indices are numbered from 1 to 16
/// or a OpenGL matrix (the transpose of the above one)
/// [  0  4  8 12 ]
/// [  1  5  9 13 ]
/// [  2  6 10 14 ]
/// [  3  7 11 15 ]

/// Now consider:
/// Vector X = (x1, x2, x3)
/// Vector Y = (y1, y2, y3)
/// Vector Z = (y1, y2, y3)
/// translation T = (T1, T2, T3)

/// OpenTK uses the column major format (bottom row stores translation):
///  X1  X2  X3   0
///  Y1  Y2  Y3   0
///  Z1  Z2  Z3   0
///  T1  T2  T3   1
/// So OpenTK matrix in memory (column-major): [X1 X2 X3 0][Y1 Y2 Y3 0][Z1 Z2 Z3 0][T1 T2 T3 1]

/// Assimp uses the righ most column to store translation. Assimp:
///  X1  Y1  Z1  T1
///  X2  Y2  Z2  T2
///  X3  Y3  Z3  T3
///  0   0   0   1
/// So in memory Assimp matrix looks like: [X1 Y1 Z1 T1] [X2 Y2 Z2 T2] [X3 Y3 Z3 T3] [0  0  0  1]
/// Which is called row-major.

/// Lol, I found out why open3mod always had to use a Transpose on OpenTK matrix in the rendering loop. 
/// Its because the dude does not convert openTK matrix to assimp matrix properly in AssimpToOpenTK. His
/// translational part is not correct.

/// For singletons instead of constructor throwing an exception we will use readonly fields. 
/// Maybe this will be cleaner.

/// TODO: Get rid of assimp matrices and vectors.
/// TODO: Mouse scrolling add a recapture of initial captured mouse position when motion changes from left to right.

/// Understand matrix:
/// See good explanation: http://web.cse.ohio-state.edu/~whmin/courses/cse5542-2013-spring/6-Transformation_II.pdf
/// also: https://www.sjbaker.org/steve/omniv/matrices_can_be_your_friends.html


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

        // readonly because this is also singleton
        public readonly Renderer _drawer = null;

        public World(PictureBox targetcanvas) {
            _drawer = new Renderer(targetcanvas);
            // turn on some config settings
            _drawer.GlobalDrawConf = new DrawConfig {
                EnablePolygonModeFill = true,
                EnableLight = true,
            };

            //Filepath to our model
            byte[] filedata = Properties.Resources.square_center_2;
            MemoryStream sphere = new MemoryStream(filedata);
            
            // use format "nff" for sphere_3d
            // LoadModel(sphere, "nff");

            // use format "dae" for bird_plane
            var cur_scene = LoadScene(sphere, "dae");

            // use format "obj" for bird_plane_5
            // LoadModel(sphere, "obj");

            ent = new Entity(cur_scene);
        }

        public Scene LoadScene(MemoryStream model_data, string format_hint)
        {
            //Create a new importer
            Scene cur_scene;
            using (var importer = new AssimpContext())
            {
                //This is how we add a configuration (each config is its own class)
                importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

                //This is how we add a logging callback 
                LogStream logstream = new LogStream( (msg, userData) => { Console.WriteLine(msg); });
                logstream.Attach();

                //Import the model. All configs are set. The model
                //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
                cur_scene = importer.ImportFileFromStream(model_data
                    , PostProcessPreset.TargetRealTimeMaximumQuality
                    , format_hint);
            }

            if (cur_scene == null || cur_scene.SceneFlags.HasFlag(SceneFlags.Incomplete))
            {
                throw new Exception("Failed to load scene");
            }

            return cur_scene;
        }

        /// <summary>
        /// Render the model stored in EntityScene useing the Graphics object.
        /// </summary>
        public void RenderWorld(Matrix camera_matrix)
        {
            // do some default OpenGL calls
            _drawer.SetupRender(Util.GR);

            if (ent == null)
            {
                _drawer.DrawEmptyEntitySplash();
            }
            else
            {
                // Applying camera transform is good here.
                Util.GR.MultiplyTransform(camera_matrix);
                // this is a debug thing, just make everything bigger.
                Util.GR.ScaleTransform(3.0f, 3.0f);
                // Now finally put vertices to GPU, because everything else is ready.
                ent.RenderModel(_drawer.GlobalDrawConf);
            }
        }



    }   // end of class World
}

