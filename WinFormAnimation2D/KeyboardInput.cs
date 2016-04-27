using Assimp;
using Assimp.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenTK;

namespace WinFormAnimation2D
{
    enum KeyboardAction
    {
            None
            , DoRotation
            , DoMotion
    }

    class KeyboardInput
    {

        public Keys RecentKey;

        public TextBox _cmd_line_control;
        public bool CmdHasFocus
        {
            get { return _cmd_line_control.Focused; }
        }

        public KeyboardInput(TextBox control)
        {
            _cmd_line_control = control;
        }

        public KeyboardAction ProcessKeydown(Keys key)
        {
            RecentKey = key;
            if (CmdHasFocus)
            {
                return KeyboardAction.None;
            }
            // else the user is talking to the 3D viewport
            switch (key)
            {
                case Keys.I:
                case Keys.O:
                case Keys.K:
                case Keys.L:
                case Keys.Oemcomma:
                case Keys.OemPeriod:
                    return KeyboardAction.DoRotation;
                case Keys.A:
                case Keys.D:
                case Keys.S:
                case Keys.W:
                case Keys.E:
                case Keys.Q:
                    return KeyboardAction.DoMotion;
                default:
                    return KeyboardAction.None;
            }
        }

        public double GetRotationDirection(Keys e)
        {
            switch (e)
            {
                case Keys.I:
                case Keys.K:
                case Keys.Oemcomma:
                    return 1;
                case Keys.O:
                case Keys.L:
                case Keys.OemPeriod:
                    return -1;
                default:
                    Debug.Assert(false);
                    break;
            }
            return double.NaN;
        }

        public Vector3 GetRotationAxis(Keys key)
        {
            switch (key)
            {
                case Keys.I:
                case Keys.O:
                    return Vector3.UnitX;
                case Keys.K:
                case Keys.L:
                    return Vector3.UnitY;
                case Keys.Oemcomma:
                case Keys.OemPeriod:
                    return Vector3.UnitZ;
                default:
                    Debug.Assert(false);
                    break;
            }
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }

        public Vector3 GetDirectionNormalized(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                    return new Vector3(-1, 0, 0);
                case Keys.D:
                    return new Vector3(1, 0, 0);
                case Keys.W:
                    return new Vector3(0, -1, 0);
                case Keys.S:
                    return new Vector3(0, 1, 0);
                case Keys.E:
                    return new Vector3(0, 0, -1);
                case Keys.Q:
                    return new Vector3(0, 0, 1);
                default:
                    Debug.Assert(false);
                    break;
            }
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }

    }
}
