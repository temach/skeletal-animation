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

namespace WinFormAnimation2D
{
    class CommandLine
    {

        public World _world;
        public PictureBox _window;
        public Timer _timer;
        public MainForm _form;
        public Entity _current;
        public ListBox _box_debug;

        public EventHandler PlayAnimationInterval;
        public EventHandler ClearScreen;

        public Dictionary<string, string> _debug = new Dictionary<string, string>();

        public CommandLine(PictureBox window, World world, Timer tm, ListBox debug, MainForm form)
        {
            _window = window;
            _world = world;
            _timer = tm;
            _box_debug = debug;
            _form = form;
            ClearScreen = delegate { _window.Invalidate(); };
            PlayAnimationInterval = delegate { this.stepf(); };
        }

        public void ShowDebug()
        {
            this._box_debug.Items.Clear();
            foreach (var v in _debug)
            {
                _box_debug.Items.Add(v.Key + " = " + v.Value);
            }
            _box_debug.Invalidate();
        }

        public void jumptime(double seconds)
        {
            if (_current == null)
            {
                return;
            }
            _current._action.SetTime(seconds);
            _form.SetAnimTime(seconds);
            _world._silly_waving_action.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
        }

        public void bkf()
        {
            if (_current == null)
            {
                return;
            }
            _current._action.BackwardKeyframe();
            _debug["from frame"] = _current._action.OriginKeyframe.ToString();
        }

        public void fkf()
        {
            if (_current == null)
            {
                return;
            }
            _current._action.ForwardKeyframe();
            _debug["from frame"] = _current._action.OriginKeyframe.ToString();
        }

        // sets the blend value for current keyframe
        public void blend(double percent)
        {
            if (_current == null)
            {
                return;
            }
            _current._action.Blend = percent / 100.0;
            _debug["blend"] = _current._action.Blend.ToString();
        }

        // applies the animation to the armature
        public void applyanim()
        {
            if (_current == null)
            {
                return;
            }
            _world._silly_waving_action.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
        }

        public void playall()
        {
            if (_current == null)
            {
                return;
            }
            _timer.Tick += PlayAnimationInterval;
            if (_timer.Enabled == false)
            {
                _timer.Start();
            }
        }

        public void stepall()
        {
            if (_current == null)
            {
                return;
            }
            if (_current._action.Blend < 0.99)
            {
                _current._action.Blend += 0.1;
            }
            else
            {
                _current._action.Blend = 0.0;
                _current._action.ForwardKeyframe();
            }
            _world._silly_waving_action.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
            _form.SetAnimTime(_current._action.CurrentTimeSeconds);
        }

        public void stepf()
        {
            if (_current == null)
            {
                return;
            }
            if (_current._action.Blend < 0.99)
            {
                _current._action.Blend += 0.1;
            }
            _world._silly_waving_action.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
            _form.SetAnimTime(_current._action.CurrentTimeSeconds);
        }

        public void RunCmd(string input)
        {
            string[] tokens = input.Trim(' ').Split(' ');
            int qty_args = tokens.Length - 1;
            object[] fargs = new object[qty_args];
            string fname = tokens[0];
            for (int k = 0; k < qty_args; k++)
            {
                string s = tokens[k];
                // as int?
                int i;
                if (int.TryParse(s, out i)) { fargs[k] = i; continue; }
                // as double?
                double d;
                if (double.TryParse(s, out d)) { fargs[k] = d;  continue; }
                // then string
                fargs[k] = s;
            }
            // find the function
            MethodInfo cmdinfo = this.GetType().GetMethod((string)fname, BindingFlags.Public | BindingFlags.Instance);
            if (cmdinfo == null)
            {
                _debug["help"] = string.Join(", ", this.GetType().GetMethods().Select(f => f.Name));
            }
            else
            {
                cmdinfo.Invoke(this, fargs);
            }
            ShowDebug();
        }
    }
}
