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
        private World world = new World();
        private bool drawModel = false;

        public Timer tm = new Timer();
        public int cur_count = 1;
        // should animation be playing. This should really go into GUISettings
        public bool animate = false;

        // State of the camera currently. We can affect this with buttons.
        public Matrix camera_matrix = new Matrix();

        private GUISettings _settings = new GUISettings();

        private float _zoom = 1.0f;
        public float Zoom
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
			Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(Color.Blue, 10.0f), pictureBox_main.DisplayRectangle);

            // Draw world elements
            // g.MultiplyTransform(camera_matrix);

            // g.DrawEllipse(Pens.Green, new Rectangle(0, 0, 10, 10));
            // g.DrawPoint(new Point(500, 50));

            // g.TranslateTransform(100, 100);
            // RecursiveRender(Current_Scene.mRootNode);
            // g.DrawRectangle(new Pen(Color.Blue, 10.0f), new Rectangle(500, 200, 40, 70));

            // g.ScaleTransform(15.0f, 15.0f);
            // g.ScaleTransform(3.0f, 3.0f);
            world.RenderWorld(g, camera_matrix,);

            /***
            if (animate)
            {
                Random rand = new Random();
                g.MultiplyTransform(world.debug_mat);
                foreach (Point p in world.debug_vertices.Take(cur_count))
                {
                    Point p = world.debug_mat.TransformPoints()
                    g.DrawPoint(p);
                }
                cur_count++;
                if (cur_count >= world.debug_vertices.Count)
                {
                    animate = false;
                    tm.Stop();
                }
            }
            ***/

            // How to draw example
            /*
			GraphicsState transState = g.Save();
 			g.TranslateTransform (1024/2, 740/2);		// center of screen
			g.TranslateTransform(0, this.tilt_value);		// lower ground by altitude modifications
			g.RotateTransform (this.roll_value);
			g.SetClip (tiltrect.Rect);
			info.TriggerPaint (pea);
			g.ResetClip ();
			g.Restore(transState);	
            */
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
