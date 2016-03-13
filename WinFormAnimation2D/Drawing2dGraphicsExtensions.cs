using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai = Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing;
using d2d = System.Drawing.Drawing2D;
using tk = OpenTK;

namespace WinFormAnimation2D
{
    static class Drawing2dGraphicsExtensions
    {

        /// <summary>
        /// Draw circle with Graphics from point and radius.
        /// </summary>
        public static void eDrawCircle(this Graphics g, Pen pen, Point p, int rad)
        {
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(pen, rect);
        }

        /// <summary>
        /// Debug function to quickly draw points with Graphics
        /// </summary>
        public static void eDrawPoint(this Graphics g, Point p)
        {
            float rad = 0.3f;        // radius
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp3, rect);
        }

        /// <summary>
        /// Quick debug function to draw _FLOATING_ PointF with Graphics
        /// </summary>
        public static void eDrawPoint(this Graphics g, PointF p)
        {
            float rad = 0.03f;        // radius
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp3, rect);
        }

        /// <summary>
        /// Debug function to quickly draw _FLOATING_ points with Graphics
        /// </summary>
        public static void eDrawBigPoint(this Graphics g, PointF p)
        {
            float rad = 10.0f;        // radius
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp1, rect);
        }

    }
}
