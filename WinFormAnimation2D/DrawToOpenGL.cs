using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace WinFormAnimation2D
{
    /// <summary>
    /// Class to control openGL settings and do the actual drawing. 
    /// All openGL calls will be here.
    /// </summary>
    class DrawToOpenGL
    {
        /// <summary>
        /// The control to which we are rendering to.
        /// Change this to OpenGL control later.
        /// </summary>
        private System.Windows.Forms.PictureBox _draw_area;

        /// <summary>
        /// Obtain actual rendering resolution in pixels
        /// </summary>
        public Size RenderResolution { get { return _draw_area.Size; } }

        public System.Windows.Forms.PictureBox DrawArea
        {
            get
            {
                return _draw_area ;
            }
            // actually we should quit calling DrawArea a property and just make it a readonly field
            // Nobody should be setting it instead of the constructor anyways.
            private set
            {
                _draw_area = value;
            }
        }

        public void SetupRender()
        {
            // GL.ClearColor(Color.LightGray);
            // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // update viewport 
            var w = (double)RenderResolution.Width;
            var h = (double)RenderResolution.Height;

            // GL.Viewport(0, 0, w, h);

            ControlClearColor(_draw_area, Color.DarkGray);

            // set a proper perspective matrix for rendering
            // var aspectRatio = (float) (w / h);
            // Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.5f, 10000f);
            // GL.MatrixMode(MatrixMode.Projection);
            // GL.LoadMatrix(ref perspective);
        }


        private void ControlClearColor(System.Windows.Forms.Control ctrl, Color col)
        {
            // GL.MatrixMode(MatrixMode.Modelview);
            // GL.LoadIdentity();
            // GL.MatrixMode(MatrixMode.Projection);
            // GL.LoadIdentity();

            // paint the active viewport in a slightly different shade of gray,
            // overwriting the initial background color.
            // GL.Color4(Color.DarkGray);
            // GL.Rect(-1, -1, 1, 1);

            var g = ctrl.CreateGraphics();
            g.Clear(col);
        }

        public void ApplyDrawSettings(DrawSettings settings)
        {


        }


        public void DrawEmptyEntitySplash()
        {

        }

    }
}
