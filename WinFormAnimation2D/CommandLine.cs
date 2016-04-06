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

        public EventHandler StepInterval;
        public EventHandler ClearScreen;
        public EventHandler StepAll;

        public Dictionary<string, string> _debug = new Dictionary<string, string>();

        public CommandLine(PictureBox window, World world, Timer tm, ListBox debug, MainForm form)
        {
            _window = window;
            _world = world;
            _timer = tm;
            _box_debug = debug;
            _form = form;
            ClearScreen = delegate { _window.Invalidate(); };
            StepInterval = delegate { this.stepf(); };
            StepAll = delegate { this.stepall(); };
        }

        public void ShowDebug()
        {
            this._box_debug.Items.Clear();
            foreach (var v in _debug)
            {
                _box_debug.Items.Add(v.Key + " = " + v.Value);
            }
        }

        public void jumpt(double seconds)
        {
            if (_current == null)
            {
                return;
            }
            _current._action.SetTime(seconds);
            _form.SetAnimTime(seconds);
            _world._action_one.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
        }

        public void bkf()
        {
            if (_current == null)
            {
                return;
            }
            _current._action.ReverseInterval();
            _debug["from frame"] = _current._action.OriginKeyframe.ToString();
        }

        public void fkf()
        {
            if (_current == null)
            {
                return;
            }
            _current._action.NextInterval();
            _debug["from frame"] = _current._action.OriginKeyframe.ToString();
        }

        // sets the blend value for current keyframe
        public void blend(double percent)
        {
            if (_current == null)
            {
                return;
            }
            _current._action.KfBlend = percent / 100.0;
            _debug["blend"] = _current._action.KfBlend.ToString();
        }

        // applies the animation to the armature
        public void applyanim()
        {
            if (_current == null)
            {
                return;
            }
            _world._action_one.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
        }

        public void playall(bool on)
        {
            if (_current == null)
            {
                return;
            }
            if (on)
            {
                //_current._action.SetTime(0);
                _current._action.Loop = true;
                _timer.Tick += StepAll;
                if (_timer.Enabled == false)
                {
                    _timer.Start();
                }
            }
            else
            {
                _current._action.Loop = false;
                _timer.Tick -= StepAll;
            }
        }

        public void playinterval()
        {
            if (_current == null)
            {
                return;
            }
            _timer.Tick += StepInterval;
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
            if (_current._action.KfBlend < 0.99)
            {
                _current._action.KfBlend += 0.1;
            }
            else
            {
                _current._action.KfBlend = 0.0;
                _current._action.NextInterval();
            }
            _world._action_one.ApplyAnimation(_current._armature
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
            if (_current._action.KfBlend < 0.99)
            {
                _current._action.KfBlend += 0.1;
            }
            _world._action_one.ApplyAnimation(_current._armature
                , _current._action);
            _window.Invalidate();
            _form.SetAnimTime(_current._action.CurrentTimeSeconds);
        }

        public void help()
        {
            _debug["help"] = string.Join(", ", this.GetType().GetMethods().Select(f => f.Name));
            ShowDebug();
        }

        public void SetError(string msg)
        {
            _debug["err"] = msg;
            ShowDebug();
        }

        public void RunCmd(string input)
        {
            IEnumerable<string> tokens = input.Trim(' ').Split(' ');
            int qty_args = tokens.Count() - 1;
            string fname = tokens.First();
            // find the function
            MethodInfo cmdinfo = this.GetType()
                .GetMethod((string)fname, BindingFlags.Public | BindingFlags.Instance);
            if (cmdinfo == null)
            {
                SetError("cmd wrong name");
                help();
                return;
            }
            // get converter for each parameter
            IEnumerable<TypeConverter> arg_converters = cmdinfo.GetParameters()
                .Select(p => TypeDescriptor.GetConverter(p.ParameterType));
            if (qty_args < arg_converters.Count())
            {
                SetError("cmd takes " + arg_converters.Count() + " args");
                return;
            }
            // for converted output
            var fargs = new List<object>(qty_args);
            foreach (var pair in tokens.Skip(1).Zip(arg_converters, (token,conv) => new { t = token, c = conv}))
            {
                if (! pair.c.IsValid(pair.t))
                {
                    SetError("can not convert '" + pair.t + "'");
                    return;
                }
                fargs.Add(pair.c.ConvertFromString(pair.t));
            }
            cmdinfo.Invoke(this, fargs.ToArray());
            ShowDebug();
        }
    }
}
