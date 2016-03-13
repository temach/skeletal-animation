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

namespace WinFormAnimation2D
{
    public partial class MainForm : Form
    {

        MouseState _m_status = new MouseState();

        private World _world;
        private Timer tm = new Timer();

        // State of the camera currently. We can affect this with buttons.
        public Drawing2DCamera _camera = new Drawing2DCamera();
        private GUIConfig _gui_conf = new GUIConfig();
        private Entity _currently_selected;

        public Drawing2DCamera Cam
        {
            get { return _camera; }
        }

        public double CamRotation
        {
            get { return _camera.GetRotationAngleDeg; }
        }

        public MainForm()
        {
            InitializeComponent();
            // we have to manually register the mousewheel event handler.
            this.MouseWheel += new MouseEventHandler(this.pictureBox_main_MouseMove);
            tm.Interval = 150;
            tm.Tick += delegate { this.pictureBox_main.Invalidate(); };
            // world.RenderModel(this.button_start.CreateGraphics());
            _world = new World(this.pictureBox_main);
            // Bind rotation to text field
             var rotobind = new Binding("Text", _camera, "GetRotationAngleDeg");
            rotobind.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            rotobind.DataSourceUpdateMode = DataSourceUpdateMode.Never;
            this.label_CurrentRotoAngle.DataBindings.Add(rotobind);
        }

        /// <summary>
        /// Intercept arrow keys to send input to the picture box.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //for the active control to see the keypress, return false
            switch (keyData)
            {

                case Keys.I:
                case Keys.O:
                    _camera.RotateByKey(new KeyEventArgs(keyData));
                    this.pictureBox_main.Invalidate();
                    return true;
                case Keys.Left:
                case Keys.Right:
                case Keys.Down:
                case Keys.Up:
                    _camera.MoveByKey(new KeyEventArgs(keyData));
                    this.pictureBox_main.Invalidate();
                    return true; // hide this key event from other controls
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        // functions to move the world objects left/right/up/down on 2D canvas
        private void button_up_Click(object sender, EventArgs e) {
            _camera.MoveBy(0, -1);
            pictureBox_main.Invalidate();
        }
        private void button_left_Click(object sender, EventArgs e) {
            _camera.MoveBy(-1, 0);
            pictureBox_main.Invalidate();
        }
        private void button_right_Click(object sender, EventArgs e) {
            _camera.MoveBy(1, 0);
            pictureBox_main.Invalidate();
        }
        private void button_down_Click(object sender, EventArgs e) {
            _camera.MoveBy(0, 1);
            pictureBox_main.Invalidate();
        }

        // change the tranbslation part of the matrix to all zeros
        private void button_resetpos_Click(object sender, EventArgs e)
        {
            pictureBox_main.Invalidate();
        }
        private void button_resetzoom_Click(object sender, EventArgs e)
        {
            _camera.CamMatrix = _camera.CamMatrix.eSnapScale(1.0);
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            pictureBox_main.Invalidate();
            tm.Start();
        }
        private void button_stop_colors_Click(object sender, EventArgs e)
        {
            tm.Stop();
        }

        private void pictureBox_main_MouseUp(object sender, MouseEventArgs e)
        {
            _m_status.IsPressed = false;
        }
        private void pictureBox_main_MouseDown(object sender, MouseEventArgs e)
        {
            _m_status.RecordMouseClick(e);
            if (_world.CheckMouseEntitySelect(_m_status))
            {
                _currently_selected = _world.CurrentlySelected;
                this.Text = "HAS ENTITY";
               // this.dataGridView_EntityInfo.DataSource 
               //     = _currently_selected.GetExposedProperties();
               // this.dataGridView_EntityInfo.Invalidate();
            }
            else
            {
                _currently_selected = null;
                this.Text = "_____ empty ____";
            }
        }

        private void pictureBox_main_MouseMove(object sender, MouseEventArgs e)
        {
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
            if (Math.Abs(delta_x) < _m_status.HorizHysteresis 
                && Math.Abs(delta_y)  < _m_status.VertHysteresis)
            {
                return;
            }
            // time to do some rotation
            _camera.ProcessMouse(delta_x, delta_y);
            pictureBox_main.Invalidate();
        }

        private void pictureBox_main_Paint(object sender, PaintEventArgs e)
        {
            // Draw program elements
            // Set GR field so that we can use Sysem.Drawing2D as if it was like OpenGL
            Util.GR = e.Graphics;
            _world.RenderWorld(_camera.CamMatrix);
        }

    }
}
