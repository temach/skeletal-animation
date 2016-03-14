using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormAnimation2D
{
    /// <summary>
    /// Simple class to store mouse status data.
    /// Monitor mouse status (delta, position, click_position, etc.) 
    /// </summary>
    class MouseState
    {

        // Is the mouse being pressed down currently.
        public bool IsPressed;

        /// Current mouse position, this is updated by you.
        public Point CurrentPos;
        /// Captured mouse position when it was clicked.
        public Point ClickPos;

        /// Position of where the user is pointing inside the game world
        public PointF InnerWorldPos;
        /// Position of click inside the game world.
        public PointF InnerWorldClickPos;

        /// Mimimum motion delta for mouse to be recognised
        public readonly int HorizHysteresis = 4;
        public readonly int VertHysteresis = 4;

        /// Updates mouse click position.
        public void RecordMouseClick(MouseEventArgs e, PointF inner_world)
        {
            this.InnerWorldPos = inner_world;
            this.InnerWorldClickPos = inner_world;
            this.ClickPos = new Point(e.X, e.Y);
            this.IsPressed = true;
        }

        /// Updates current mouse position. Then we can caluclate delta better.
        public void RecordMouseMove(MouseEventArgs e, PointF inner_world)
        {
            this.InnerWorldPos = inner_world;
            this.CurrentPos = new Point(e.X, e.Y);
        }

    };
}
