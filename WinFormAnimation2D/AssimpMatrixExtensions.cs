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
    static class AssimpMatrixExtensions
    {

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
        /// Convert 4x4 Assimp matrix to OpenTK matrix.
        /// Will be a very useful function becasue Assimp 
        /// matrices are very limited.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static tk.Matrix4 eToOpenTK(this ai.Matrix4x4 m)
        {
            return new tk.Matrix4
            {
                M11 = m.A1,
                M12 = m.B1,
                M13 = m.C1,
                M14 = m.D1,
                M21 = m.A2,
                M22 = m.B2,
                M23 = m.C2,
                M24 = m.D2,
                M31 = m.A3,
                M32 = m.B3,
                M33 = m.C3,
                M34 = m.D3,
                M41 = m.A4,
                M42 = m.B4,
                M43 = m.C4,
                M44 = m.D4
            };
        }

        /// <summary>
        /// Convert assimp 4 by 4 matrix into 3 by 2 matrix from System.Drawing.Drawing2D and use it
        /// for drawing with Graphics object.
        /// </summary>
        public static d2d.Matrix eTo3x2(this ai.Matrix4x4 m)
        {
            return new d2d.Matrix(m.A1, m.B1, m.A2, m.B2, m.A4, m.B4);
            // return new draw2D.Matrix(m[0, 0], m[1, 0], m[0, 1], m[1, 1], m[0, 3], m[1, 3]);
        }

    }
}
