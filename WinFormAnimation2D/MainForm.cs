﻿using Assimp;
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

namespace WinFormAnimation2D
{
    public partial class MainForm : Form
    {
        MouseState _m_status = new MouseState();

        // make this readonly becasue its a singleton
        private World _world;

        private Timer tm = new Timer();
        private int cur_count = 1;

        // State of the camera currently. We can affect this with buttons.
        private Drawing2DCamera _camera = new Drawing2DCamera();
        private GUIConfig _gui_conf = new GUIConfig();

        public MainForm()
        {
            InitializeComponent();
            // we have to manually register the mousewheel event handler.
            this.MouseWheel += new MouseEventHandler(this.pictureBox_main_MouseMove);
            tm.Interval = 150;
            tm.Tick += delegate { this.pictureBox_main.Invalidate(); };
            // world.RenderModel(this.button_start.CreateGraphics());
            // Allow to use arrow keys for navigation
            _world = new World(this.pictureBox_main);
        }

        /// <summary>
        /// Intercept arrow keys to send input to the picture box.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //for the active control to see the keypress, return false
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Down:
                case Keys.Up:
                    this.MyForm_KeyDown(this, new KeyEventArgs(keyData));
                    return true; // hide this key event from others
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            pictureBox_main.Invalidate();
            // MessageBox.Show("Starting everything");
            // animate = true;
            tm.Start();
        }

        private void pictureBox_main_Paint(object sender, PaintEventArgs e)
        {
            // Draw program elements
            // Set GRPH so we can use Sysem.Drawing2D as if it was like OpenGL
            Util.GR = e.Graphics;

            Util.GR.DrawRectangle(new Pen(Color.Blue, 10.0f), pictureBox_main.DisplayRectangle);

            _world.RenderWorld(_camera.CamMatrix);
        }

        // functions to move the world objects left/right/up/down on 2D canvas
        private void button_up_Click(object sender, EventArgs e) {
            _camera.CamMatrix.Translate(0, (-1)*Util.stepsize);
            pictureBox_main.Invalidate();
        }
        private void button_left_Click(object sender, EventArgs e) {
            _camera.CamMatrix.Translate((-1)*Util.stepsize, 0);
            pictureBox_main.Invalidate();
        }
        private void button_right_Click(object sender, EventArgs e) {
            _camera.CamMatrix.Translate(Util.stepsize, 0);
            pictureBox_main.Invalidate();
        }
        private void button_down_Click(object sender, EventArgs e) {
            _camera.CamMatrix.Translate(0, Util.stepsize);
            pictureBox_main.Invalidate();
        }

        // change the tranbslation part of the matrix to all zeros
        private void button_resetpos_Click(object sender, EventArgs e)
        {
            _camera.CamMatrix = _camera.CamMatrix.eSnapTranslate(0.0, 0.0);
            pictureBox_main.Invalidate();
        }

        private void button_resetzoom_Click(object sender, EventArgs e)
        {
            _camera.CamMatrix = _camera.CamMatrix.eSnapScale(1.0);
        }

        private void MyForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    button_left_Click(null, EventArgs.Empty);
                    break;
                case Keys.Up:
                    button_up_Click(null, EventArgs.Empty);
                    break;
                case Keys.Right:
                    button_right_Click(null, EventArgs.Empty);
                    break;
                case Keys.Down:
                    button_down_Click(null, EventArgs.Empty);
                    break;
                default:
                    break;
            }
        }

        private void button_stop_colors_Click(object sender, EventArgs e)
        {
            tm.Stop();
        }

        private void pictureBox_main_MouseDown(object sender, MouseEventArgs e)
        {
            _m_status.RecordMouseClick(e);
        }

        private void pictureBox_main_MouseMove(object sender, MouseEventArgs e)
        {
            // process scroll wheel
            // TODO: add some jitter checking
            if (Math.Abs(e.Delta) >= 1)
            {
                _camera.ProcessScroll(e.Delta);
            }

            // Process mouse motion only if it is pressed
            if (! _m_status.IsPressed) {
                return;
            }
            // this.Text = _m_status._mouse_x_captured.ToString();
            var delta_x = e.X - _m_status.ClickX;
            var delta_y = e.Y - _m_status.ClickY;
            // Check if its just mouse jitter. Don't bother updating the screen.
            if (Math.Abs(delta_x) < _m_status.HorizHysteresis 
                && Math.Abs(delta_y)  < _m_status.VertHysteresis)
            {
                return;
            }
            else
            {
                // time to do some rotation
                _camera.ProcessMouse(delta_x, delta_y);
            }
            pictureBox_main.Invalidate();
        }

        private void pictureBox_main_MouseUp(object sender, MouseEventArgs e)
        {
            // stop timer to poll mouse position
            // or
            // just turn off the mouse down bool
            _m_status.IsPressed = false;
        }

    }
}
