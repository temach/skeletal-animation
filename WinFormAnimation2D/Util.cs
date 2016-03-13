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
using System.Reflection;

namespace WinFormAnimation2D
{
    public static class Util
    {
        // When we get OpenGL we will not have to pass around a Graphics instance.
        public static Graphics GR = null;

        // Useful for random value generation. 
        // We use only one Random instance in the whole program.
        private static Random rand = new Random();

        // Note that this is Unsigned int (so overflow is ok)
        public static Func<Brush> GetNextBrush = SetupBrushGen();

        // Static config fields
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
        /// Small + Red pen to render points on screen
        /// </summary>
        public static Pen pp4 = new Pen(Color.SkyBlue, 2.5f);


        /// <summary>
        /// Get a brush of next color. (to distingush rendered polygons)
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