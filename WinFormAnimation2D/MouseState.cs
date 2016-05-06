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

        public Point LastFramePos;
        public Point FrameDelta
        {
            get
            {
                return new Point(LastFramePos.X - CurrentPos.X, LastFramePos.Y - CurrentPos.Y);
            }
        }

        /// Position of where the user is pointing inside the game world
        public OpenTK.Vector3 InnerWorldPos;
        /// Position of click inside the game world.
        public OpenTK.Vector3 InnerWorldClickPos;

        /// Mimimum motion delta for mouse to be recognised
        public readonly int HorizHysteresis = 4;
        public readonly int VertHysteresis = 4;

        /// Updates mouse click position.
        public void RecordMouseClick(MouseEventArgs e)
        {
            this.ClickPos = new Point(e.X, e.Y);
            this.IsPressed = true;
        }

        /// Updates current mouse position. Then we can caluclate delta better.
        public void RecordMouseMove(MouseEventArgs e)
        {
            this.LastFramePos = this.CurrentPos;
            this.CurrentPos = new Point(e.X, e.Y);
        }

        public void RecordInnerWorldMouseClick(OpenTK.Vector3 vec)
        {
            this.InnerWorldClickPos = vec;
            this.InnerWorldPos = vec;
        }

        public void RecordInnerWorldMouseMove(OpenTK.Vector3 vec)
        {
            this.InnerWorldPos = vec;
        }

    };
}
