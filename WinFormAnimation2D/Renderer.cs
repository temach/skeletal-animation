using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace WinFormAnimation2D
{
    /// <summary>
    /// Class to control openGL settings and do the actual drawing. 
    /// All openGL calls will be here.
    /// </summary>
    class Renderer
    {
        // The control to which we are rendering to.
        // Change this to OpenGL control later.
        private PictureBox _canvas;

        public DrawConfig GlobalDrawConf;

        public Renderer()
        {
        }

        public void ClearFrameBuffer()
        {
        }

        public void InitOpenGL()
        {
            // enable stuff
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            // other settings
            GL.ShadeModel(ShadingModel.Flat);
            GL.ClearColor(Color.DarkGray);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            // lights
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0, 0, 10, 0 });
        }

        public void ResizeOpenGL(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            // set a proper perspective matrix for rendering
            float aspectRatio = ((float)width)/height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000.0f);
            GL.LoadMatrix(ref perspective);
            // now Model view matrix
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        /// <summary>
        /// Important points to remember:
        /// Set normals.
        /// Must be clock wise vertex draw order
        /// The x-axis is accross the screen, so the Z-axis triangle must have component along X: +-1
        /// since look at looks towards the center, we need to offset it a bit to see the Z axis.
        /// </summary>
        public void DrawAxis3D()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color.Aqua);
            GL.Normal3(0, 0, 1);
            int shift = 1;
            GL.Begin(BeginMode.Triangles);
            // x axis
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(20, -shift, 0);
            GL.Vertex3(20, shift, 0);
            // y axis
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(shift, 20, 0);
            GL.Vertex3(-shift, 20, 0);
            // z axis
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(-shift, 0, 20);
            GL.Vertex3(shift, 0, 20);
            GL.End();
            GL.Enable(EnableCap.DepthTest);
        }

        public void ClearOpenglFrameForRender(Matrix4 camera_matrix)
        {
            // TEST CODE to visualize mid point (pivot) and origin
            // var view = Matrix4.LookAt(0, 50, 500, 0, 0, 0, 0, 1, 0);
            //GL.LoadMatrix(ref view);
            if (Properties.Settings.Default.OpenGLCullFace)
            {
                GL.Enable(EnableCap.CullFace);
            }
            else
            {
                GL.Disable(EnableCap.CullFace);
            }
            GL.LoadIdentity();
            GL.LoadMatrix(ref camera_matrix);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            // light color
            var col = new Vector3(1, 1, 1);
            col *= (0.25f + 1.5f * 10 / 100.0f) * 1.5f;
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { col.X, col.Y, col.Z, 1 });
            // 3d
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void DrawEmptyEntitySplash()
        {
            string msg = "No file loaded";
            //var w = (float)RenderResolution.Width;
            //var h = (float)RenderResolution.Height;
            // Util.GR.DrawString(msg, GlobalDrawConf.DefaultFont16 , Brushes.Aquamarine, new PointF(w / 2.0f, h / 2.0f));
        }

    }
}
