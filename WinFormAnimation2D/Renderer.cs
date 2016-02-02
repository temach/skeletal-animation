using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;


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
            Util.GR.Clear(Color.DarkGray);
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
