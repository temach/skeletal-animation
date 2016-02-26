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
    static class OpentkMatrixExtensions
    {

        /// <summary>
        /// Convert 4x4 OpenTK matrix into Assimp matrix. This should not be used often.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static ai.Matrix4x4 eToAssimp(this tk.Matrix4 m)
        {
            return new ai.Matrix4x4
            {
               A1 = m.M11,
               B1 = m.M12,
               C1 = m.M13,
               D1 = m.M14,
               A2 = m.M21,
               B2 = m.M22,
               C2 = m.M23,
               D2 = m.M24,
               A3 = m.M31,
               B3 = m.M32,
               C3 = m.M33,
               D3 = m.M34,
               A4 = m.M41,
               B4 = m.M42,
               C4 = m.M43,
               D4 = m.M44
            };
        }
        
        /// <summary>
        /// Convert _OpenTK_ 4 by 4 matrix into 3 by 2 matrix from System.Drawing.Drawing2D and use it
        /// for drawing with Graphics object.
        /// </summary>
        public static d2d.Matrix eTo3x2(this tk.Matrix4 m)
        {
            return m.eToAssimp().eTo3x2();
        }
    }
}
