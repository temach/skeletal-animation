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
    public partial class MyForm : Form
    {
        // readonly becasue its a singleton
        private readonly World world;

        private Timer tm = new Timer();
        private int cur_count = 1;
        // should animation be playing. This should really go into GUISettings
        private bool animate = false;

        // State of the camera currently. We can affect this with buttons.
        private Matrix camera_matrix = new Matrix();
        private GUIConfig _gui_conf = new GUIConfig();

        private float _zoom = 1.0f;
        private float Zoom
        {
            get {
                return _zoom;
            } set {
                _zoom = value;
                label_zoom.Text = _zoom.ToString();
                pictureBox_main.Invalidate();
            }
        }

        public MyForm()
        {
            InitializeComponent();
            tm.Interval = 150;
            tm.Tick += delegate { this.pictureBox_main.Invalidate(); };
            // world.RenderModel(this.button_start.CreateGraphics());

            // Allow to use arrow keys for navigation
            KeyPreview = true;
            world = new World(this.pictureBox_main);
        }
        
		protected void DoRedraw()
		{
			Invalidate ();
			foreach (Control ctrl in Controls)
				ctrl.Invalidate();
		}

		protected override void OnMove (EventArgs ea)
		{
			base.OnMove(ea);
            DoRedraw();
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

            world.RenderWorld(camera_matrix);
        }

        // functions to move the world objects left/right/up/down on 2D canvas
        private void button_up_Click(object sender, EventArgs e) {
            camera_matrix.Translate(0, (-1)*Util.stepsize);
            pictureBox_main.Invalidate();
        }
        private void button_left_Click(object sender, EventArgs e) {
            camera_matrix.Translate((-1)*Util.stepsize, 0);
            pictureBox_main.Invalidate();
        }
        private void button_right_Click(object sender, EventArgs e) {
            camera_matrix.Translate(Util.stepsize, 0);
            pictureBox_main.Invalidate();
        }
        private void button_down_Click(object sender, EventArgs e) {
            camera_matrix.Translate(0, Util.stepsize);
            pictureBox_main.Invalidate();
        }

        // change the tranbslation part of the matrix to all zeros
        private void button_resetpos_Click(object sender, EventArgs e)
        {
            camera_matrix = camera_matrix.eSnapTranslate(0.0, 0.0);
            pictureBox_main.Invalidate();
        }

        private void button_resetzoom_Click(object sender, EventArgs e)
        {
            camera_matrix = camera_matrix.eSnapScale(1.0);
            Zoom = 1.0f;
            trackBar_zoom.Value = 0;
        }

        private void button_zoom_Click(object sender, EventArgs e)
        {
            float delta = 0;
            int val = trackBar_zoom.Value;
            // Weird formula to get both zoom in and zoom out.
            // Check when 0 (then we assign 1.0f) 
            // when less than 0 (then we divide) 
            // when more than zero (then we simply assign)
            delta = (val == 0) ? (1.0f) :
                (val < 0) ? (-1.0f / val) :         // note the (-1)
                val;
            Zoom *= delta;
            camera_matrix.Scale(delta, delta);
        }

        private void trackBar_zoom_ValueChanged(object sender, EventArgs e)
        {
            float delta = 0;
            int val = trackBar_zoom.Value;
            // Check when 0 (then we assign 1.0f) 
            // when less than 0 (then we divide) 
            // when more than zero (then we simply assign)
            delta = (val == 0) ? (1.0f) :
                (val < 0) ? (-1.0f / val) :         // note the (-1)
                val;
            label_zoom.Text = (delta * Zoom).ToString();
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

    }
}
