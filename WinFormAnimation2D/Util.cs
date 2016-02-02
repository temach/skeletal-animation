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

namespace WinFormAnimation2D
{
    static class Util
    {
        // When we get OpenGL we will not have to pass around a Graphics instance.
        public static Graphics GR = null;

        // Useful for random value generation. 
        // We use only one Random instance in the whole program.
        private static Random rand = new Random();

        // Note that this is Unsigned int (so overflow is ok)
        public static Func<Brush> GetNextBrush = SetupBrushGen();

        // Static config fields
        public static float stepsize = 10.0f;
        public static double epsilon = 1E-8;

        /// <summary>
        /// Big + Green pen to render points on screen
        /// </summary>
        public static Pen pp1 = new Pen(Color.LawnGreen, 20.0f);

        /// <summary>
        /// Medium + Black pen to render points on screen
        /// </summary>
        public static Pen pp2 = new Pen(Color.Black, 10.0f);

        /// <summary>
        /// Small + Red pen to render points on screen
        /// </summary>
        public static Pen pp3 = new Pen(Color.Red, 0.01f);

        /// <summary>
        /// Convert assimp 4 by 4 matrix into 3 by 2 matrix from System.Drawing.Drawing2D and use it
        /// for drawing with Graphics object.
        /// </summary>
        public static d2d.Matrix eTo3x2(this ai.Matrix4x4 m)
        {
            return new d2d.Matrix(m.A1, m.B1, m.A2, m.B2, m.A4, m.B4);
            // return new draw2D.Matrix(m[0, 0], m[1, 0], m[0, 1], m[1, 1], m[0, 3], m[1, 3]);
        }

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
        /// Quick debug function to draw _floating_ PointF with Graphics
        /// </summary>
        public static void eDrawPoint(this Graphics g, PointF p)
        {
            float rad = 0.03f;        // radius
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp3, rect);
        }

        /// <summary>
        /// Convert assimp 3D vector to 2D System.Drawing.Point
        /// for drawing with Graphics object.
        /// </summary>
        public static Point eToPoint(this ai.Vector3D v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Convert assimp 3D vector to 2D System.Drawing.PointF (floating point)
        /// for drawing with Graphics object.
        /// </summary>
        public static PointF eToPointFloat(this ai.Vector3D v)
        {
            return new PointF(v.X, v.Y);
        }
        /// <summary>
        /// Rescale the matrix. Preserve rotation and translation.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static d2d.Matrix eSnapScale(this d2d.Matrix mat, double scale = 1.0)
        {
            var curmat = mat.Elements;

            // normalise the x and y axis to set scale to 1.0f
            var x_axis = new ai.Vector2D(curmat[0], curmat[1]);
            var y_axis = new ai.Vector2D(curmat[2], curmat[3]);
            x_axis.Normalize();
            y_axis.Normalize();

            // scale the axis
            x_axis.X *= (float)scale;
            x_axis.Y *= (float)scale;
            y_axis.X *= (float)scale;
            y_axis.Y *= (float)scale;

            // make new matrix with scale of 1.0f 
            // Do not change the translation
            var newmat = new d2d.Matrix(x_axis[0], x_axis[1]
                , y_axis[0], y_axis[1]
                , curmat[4], curmat[5]
            );

            return newmat.Clone();
        }

        /// <summary>
        /// Snap translation part of the matrix to a given vector.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static d2d.Matrix eSnapTranslate(this d2d.Matrix mat, double x, double y)
        {
            var curmat = mat.Elements;
            var newmat = new d2d.Matrix(curmat[0], curmat[1]
                , curmat[2], curmat[3]
                , (float)x, (float)y);
            return newmat.Clone();
        }

        /// <summary>
        /// Snap rotate to some angle. Preserve scale and translation.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static d2d.Matrix eSnapRotate(this d2d.Matrix mat, double angle)
        {
            var curmat = mat.Elements;

            // get the vector components.
            var x_axis = new ai.Vector2D(curmat[0], curmat[1]);
            var y_axis = new ai.Vector2D(curmat[2], curmat[3]);

            // Get the scale of current matrix
            double x_len = x_axis.Length();
            double y_len = y_axis.Length();

            var newmat = new d2d.Matrix();
            newmat.Rotate((float)angle);

            // Preserve scale and translation
            newmat.Scale((float)x_len, (float)y_len);
            newmat.Translate(curmat[5], curmat[6]);

            return newmat.Clone();
        }

        /// <summary>
        /// Transform a direction vector by the given Matrix. Note: this is for assimp 
        /// matrix which is row major.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static ai.Vector3D eTransformVector(this ai.Matrix4x4 mat, ai.Vector3D vec)
        {
            return new ai.Vector3D
            {
                X = vec.X * mat.A1
                    + vec.Y * mat.B1
                    + vec.Z * mat.C1
                    + mat.A4,
                Y = vec.X * mat.A2
                    + vec.Y * mat.B2
                    + vec.Z * mat.C2
                    + mat.B4,
                Z = vec.X * mat.A3
                    + vec.Y * mat.B3
                    + vec.Z * mat.C3
                    + mat.C4
            };
        }


        /// <summary>
        /// Get a brush of random color.
        /// </summary>
        /// <returns></returns>
        private static Func<Brush> SetupBrushGen()
        {
            // Note that this is Unsigned int (so overflow is ok)
            uint _iter_nextbrush = 0;

            return () =>
            {
                // cache this variable
                _iter_nextbrush++;
                switch (_iter_nextbrush % 3)
                {
                    case 0:
                        return Brushes.GreenYellow;
                    case 1:
                        return Brushes.SeaGreen;
                    case 2:
                        return Brushes.Green;
                    case 3:
                        return Brushes.LightSeaGreen;
                    case 4:
                        return Brushes.LawnGreen;
                    default:
                        return Brushes.Red;
                }
            };
        }
    } // end of class
}