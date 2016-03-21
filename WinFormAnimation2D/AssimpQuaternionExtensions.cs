using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;        // for MemoryStream
using System.Reflection;
using System.Diagnostics;
using Assimp;
using Assimp.Configs;
using d2d = System.Drawing.Drawing2D;
using tk = OpenTK;

namespace WinFormAnimation2D
{
    static class AssimpQuaternionExtensions
    {
        public static Matrix4x4 eToMatrix(this Quaternion q)
        {
            float w = q.W, x = q.X, y = q.Y, z = q.Z;
            float xx = 2.0f * x * x;
            float yy = 2.0f * y * y;
            float zz = 2.0f * z * z;
            float xy = 2.0f * x * y;
            float zw = 2.0f * z * w;
            float xz = 2.0f * x * z;
            float yw = 2.0f * y * w;
            float yz = 2.0f * y * z;
            float xw = 2.0f * x * w;
            return new Matrix4x4(1.0f-yy-zz, xy + zw, xz - yw, 0.0f,
                                 xy - zw, 1.0f-xx-zz, yz + xw, 0.0f,
                                 xz + yw, yz - xw, 1.0f-xx-yy, 0.0f,
                                 0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
