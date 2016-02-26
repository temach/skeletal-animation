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

namespace WinFormAnimation2D
{
    static class Drawing2dMatrixExtensions
    {
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

    }
}
