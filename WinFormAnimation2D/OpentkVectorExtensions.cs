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
    static class OpentkVectorExtensions
    {

        /// <summary>
        /// Convert _OpenTK_ 3D vector to 2D System.Drawing.Point
        /// for drawing with Graphics object.
        /// </summary>
        public static Point eToPoint(this tk.Vector3 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Convert _OpenTK_ 3D vector to 2D System.Drawing.PointF (floating point)
        /// for drawing with Graphics object.
        /// </summary>
        public static PointF eToPointFloat(this tk.Vector3 v)
        {
            return new PointF(v.X, v.Y);
        }

    }
}
