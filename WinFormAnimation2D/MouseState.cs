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

        /// Current mouse position, this is updated by you.
        public Point CurPos;

        /// Captured mouse position when it was clicked.
        public Point ClickPos;
        public int ClickX {
            get { return ClickPos.X; }
        }
        public int ClickY {
            get { return ClickPos.Y; }
        }

        /// Mimimum motion delta for mouse to be recognised
        public Size Hysteresis = new Size(4, 4);
        public int HorizHysteresis {
            get { return Hysteresis.Height; }
        }
        public int VertHysteresis {
            get { return Hysteresis.Width; }
        }

        /// Coordinate to store as current mouse position
        public Point CurrentPos;
        public int CurrentX {
            get { return CurrentPos.X; }
        }
        public int CurrentY {
            get { return CurrentPos.Y; }
        }

        // Is the mouse being pressed down currently.
        public bool IsPressed;

        // Delta between current mouse position and its click position
        public int DeltaClickX {
            get { return CurrentX - ClickX; }
        }
        public int DeltaClickY {
            get { return CurrentY - ClickY; }
        }

        /// <summary>
        /// Record that a mouse click has happened.
        /// </summary>
        /// <param name="e"></param>
        public void RecordMouseClick(MouseEventArgs e)
        {
            this.ClickPos = new Point(e.X, e.Y);
            this.IsPressed = true;
        }

        /// <summary>
        /// Updates current mouse position. Then we can caluclate delta better.
        /// </summary>
        /// <param name="e"></param>
        public void UpdateCurrentPos(MouseEventArgs e)
        {
            this.CurPos = new Point(e.X, e.Y);
        }

    };

}
