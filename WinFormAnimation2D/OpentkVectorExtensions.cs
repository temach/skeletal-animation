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

        /// <summary>
        /// Convert _OpenTK_ 2D vector to 2D System.Drawing.Point
        /// for drawing with Graphics object.
        /// </summary>
        public static Point eToPoint(this tk.Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Convert _OpenTK_ 2D vector to 2D System.Drawing.PointF (floating point)
        /// for drawing with Graphics object.
        /// </summary>
        public static PointF eToPointFloat(this tk.Vector2 v)
        {
            return new PointF(v.X, v.Y);
        }

        /// <summary>
        /// Convert open tk 3D vector to opentk 2D vector.
        /// </summary>
        public static tk.Vector2 eTo2D(this tk.Vector3 v)
        {
            return new tk.Vector2(v.X, v.Y);
        }

        /// <summary>
        /// Checks if the vector has all values close to zero.
        /// </summary>
        public static bool eIsZero(this tk.Vector3 v)
        {
            if (Math.Abs(v.X) < Util.epsilon
                && Math.Abs(v.Y) < Util.epsilon
                && Math.Abs(v.Z) < Util.epsilon)
            {
                return true;
            }
            return false;
        }
    }
}
