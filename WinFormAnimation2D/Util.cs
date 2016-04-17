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

    public static class Breakpoints
    {
        public static bool Allow = false;
    }

    public static class Util
    {

        // When we get OpenGL we will not have to pass around a Graphics instance.
        public static Graphics GR = null;

        // Useful for random value generation. 
        // We use only one Random instance in the whole program.
        private static Random rand = new Random();

        // Note that this is Unsigned int (so overflow is ok)
        public static Func<Brush> GetNextBrush = SetupBrushGen();
        public static Func<Color> GetNextColor = SetupColorGen();

        // Static config fields
        public static double epsilon = 1E-8;

        /// <summary>
        /// Big + Green pen to render points on screen
        /// </summary>
        public static Pen pp1 = new Pen(Color.LawnGreen, 20.0f);
        public static Color cc1 = Color.LawnGreen;

        /// <summary>
        /// Medium + Black pen to render points on screen
        /// </summary>
        public static Pen pp2 = new Pen(Color.Black, 10.0f);
        public static Color cc2 = Color.Black;

        /// <summary>
        /// Small + Red pen to render points on screen
        /// </summary>
        public static Pen pp3 = new Pen(Color.Red, 0.01f);
        public static Color cc3 = Color.Red;

        /// <summary>
        /// Small + Red pen to render points on screen
        /// </summary>
        public static Pen pp4 = new Pen(Color.SkyBlue, 2.5f);
        public static Color cc4 = Color.SkyBlue;

        /// <summary>
        /// Internal matrix stack. Like OpenGL's one.
        /// </summary>
        public static Stack<d2d.GraphicsState> matrix_stack = new Stack<d2d.GraphicsState>();
        public static void PushMatrix()
        {
            matrix_stack.Push(Util.GR.Save());
        }
        public static void PopMatrix()
        {
            Util.GR.Restore(matrix_stack.Pop());
        }


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
                if (Properties.Settings.Default.TriangulateMesh == false)
                {
                    return Brushes.Green;
                }
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

        /// <summary>
        /// Get a brush of next color. (to distingush rendered polygons)
        /// </summary>
        /// <returns></returns>
        private static Func<Color> SetupColorGen()
        {
            // Note that this is Unsigned int (so overflow is ok)
            uint _iter_next_color = 0;
            return () =>
            {
                if (Properties.Settings.Default.TriangulateMesh == false)
                {
                    return Color.Green;
                }
                // cache this variable
                _iter_next_color++;
                switch (_iter_next_color % 3)
                {
                    case 0:
                        return Color.GreenYellow;
                    case 1:
                        return Color.SeaGreen;
                    case 2:
                        return Color.Green;
                    case 3:
                        return Color.LightSeaGreen;
                    case 4:
                        return Color.LawnGreen;
                    default:
                        return Color.Red;
                }
            };
        }

        // subtract one point from another
        public static Point Minus(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        // add one point to another
        public static Point Add(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

    } // end of class
}