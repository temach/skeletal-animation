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

        // Obtain actual rendering resolution in pixels
        public Size RenderResolution {
            get { return _canvas.Size; }
        }

        public DrawConfig GlobalDrawConf;

        public Renderer(PictureBox targetcanvas)
        {
            _canvas = targetcanvas;
        }

        public void SetupRender(Graphics g)
        {
            // update viewport 
            var w = (double)RenderResolution.Width;
            var h = (double)RenderResolution.Height;
            // 2d
            Util.GR.Clear(Color.DarkGray);
            // 3d
            GL.DepthMask(true);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void InitOpenGL()
        {
            // enable stuff
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.CullFace);
            // other settings
            GL.ShadeModel(ShadingModel.Flat);
            GL.ClearColor(Color.DarkGray);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            // lights
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 5, 10, 2, 0 });
        }

        public void ResizeOpenGL(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            // set a proper perspective matrix for rendering
            float aspectRatio = ((float)width)/height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.001f, 100.0f);
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
	}

        public void DrawEmptyEntitySplash()
        {
            string msg = "No file loaded";
            var w = (float)RenderResolution.Width;
            var h = (float)RenderResolution.Height;
            Util.GR.DrawString(msg, GlobalDrawConf.DefaultFont16 
                , Brushes.Aquamarine , new PointF(w / 2.0f, h / 2.0f) );

        }

    }
}
