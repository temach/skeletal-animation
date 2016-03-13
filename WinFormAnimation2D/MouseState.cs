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
        public Point CurPos;

        /// Position of where the user is pointing inside the game world
        public PointF InnerWorldPos;
        public double InnerWorldX {
            get { return InnerWorldPos.X; }
        }
        public double InnerWorldY {
            get { return InnerWorldPos.Y; }
        }

        /// Captured mouse position when it was clicked.
        public Point ClickPos;
        public int ClickX {
            get { return ClickPos.X; }
        }
        public int ClickY {
            get { return ClickPos.Y; }
        }

        /// Mimimum motion delta for mouse to be recognised
        public readonly int HorizHysteresis = 4;
        public readonly int VertHysteresis = 4;

        /// Coordinate to store as current mouse position
        public Point CurrentPos;
        public int CurrentX {
            get { return CurrentPos.X; }
        }
        public int CurrentY {
            get { return CurrentPos.Y; }
        }

        // Delta between current mouse position and its click position
        public int DeltaClickX {
            get { return CurrentX - ClickX; }
        }
        public int DeltaClickY {
            get { return CurrentY - ClickY; }
        }

        public void RecordMouseClick(MouseEventArgs e, PointF inner_world)
        {
            this.InnerWorldPos = inner_world;
            this.RecordMouseClick(e);
        }

        /// Record that a mouse click has happened.
        public void RecordMouseClick(MouseEventArgs e)
        {
            this.ClickPos = new Point(e.X, e.Y);
            this.IsPressed = true;
        }

        /// Updates current mouse position. Then we can caluclate delta better.
        public void RecordCurrentPos(MouseEventArgs e)
        {
            this.CurPos = new Point(e.X, e.Y);
        }

    };

}
