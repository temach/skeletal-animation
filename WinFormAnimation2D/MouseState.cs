using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormAnimation2D
{
    class MouseState
    {
        // mimimum motion delta for mouse to be recognised
        public int _horiz_hysteresis = 4;
        public int _vert_hysteresis = 4;
        // mouse position when is was pressed down
        public int _mouse_x_captured;
        public int _mouse_y_captured;
        // is mouse pressed currently
        public bool _is_mouse_down;
        // change in mouse position for current frame
        public int delta_x;
        public int delta_y;
    };

}
