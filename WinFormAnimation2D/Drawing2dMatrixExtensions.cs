using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Diagnostics;

namespace WinFormAnimation2D
{
    static class Drawing2dMatrixExtensions
    {

        /// <summary>
        /// Transform a single PointF object and return the result.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static PointF eTransformSinglePointF(this d2d.Matrix mat, PointF p)
        {
            var tmp = new PointF[] { p };
            mat.TransformPoints(tmp);
            return tmp[0];
        }

        /// <summary>
        /// Transform a single Vector2 object and return the result.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static tk.Vector2 eTransformSingleVector2(this d2d.Matrix mat, tk.Vector2 p)
        {
            var tmp = new tk.Vector2[] { p };
            mat.eTransformVector2(tmp);
            return tmp[0];
        }

        /// <summary>
        /// Applies the geometric transform represented by this System.Drawing.Drawing2D.Matrix
        /// to a specified array of Opentk.Vector2
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="vecs"></param>
        public static void eTransformVector2(this d2d.Matrix mat, tk.Vector2[] vecs)
        {
            PointF[] tmp = vecs.Select(vec => new PointF(vec.X, vec.Y)).ToArray();
            mat.TransformPoints(tmp);
            // set them equal this way we dont mess up if other 
            // objects kept pointers to some vector and we just override it
            for (int i = 0; i < vecs.Length; i++)
            {
                vecs[i].X = tmp[i].X;
                vecs[i].Y = tmp[i].Y;
            }
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
        /// <param name="angle">__ANGLE IS IN DEGREES__</param>
        /// <returns></returns>
        public static d2d.Matrix eSnapRotate(this d2d.Matrix mat, double angle)
        {
            // Graphics tries to work opposite of OpenGL, in Drawing2D:
            // PRE - multiply for local
            // post -multiply for global
            var curmat = mat.Elements;
            // get the vector components.
            var x_axis = new ai.Vector2D(curmat[0], curmat[1]);
            var y_axis = new ai.Vector2D(curmat[2], curmat[3]);
            // Get the scale of current matrix
            double x_len = x_axis.Length();
            double y_len = y_axis.Length();
            var newmat = new d2d.Matrix();
            // Preserve scale and translation
            // This means: v*M = v*(S * R * T)
            newmat.Scale((float)x_len, (float)y_len);
            newmat.Rotate((float)angle);
            newmat.Translate(curmat[4], curmat[5]);
            return newmat.Clone();
        }

        /// <summary>
        /// Returns the rotation angle in degrees.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        static public double eGetRotationAngle(this d2d.Matrix mat)
        {
            // see http://stackoverflow.com/questions/5072271/get-angle-from-matrix
            var curmat = mat.Elements;
            // check that error is tiny
            //Debug.Assert( 1e-10 
            //    > Math.Abs(curmat[1] - (-1 * curmat[2])) + Math.Abs(curmat[0] - curmat[1]) 
            //    , "Matrix has been modified such that getting the rotation angle is meaningless.");
            var scale_factor = Math.Sqrt((curmat[0]*curmat[3] - curmat[1]*curmat[2]));
            return Math.Acos(curmat[0]/scale_factor) * 180 / Math.PI; // For degrees
        }


        /// <summary>
        /// Returns the translation component of matrix as a Point
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        static public PointF eGetTranslationPoint(this d2d.Matrix mat)
        {
            return new PointF(mat.Elements[4], mat.Elements[5]);
        }
    }
}
