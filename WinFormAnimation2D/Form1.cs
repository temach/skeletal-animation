using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WinFormAnimation2D
{
    public partial class MyForm : Form
    {
        private World world = new World();
        private bool drawModel = false;

        public Timer tm = new Timer();
        public int cur_count = 1;
        public bool animate = false;

        public MyForm()
        {
            InitializeComponent();
            tm.Interval = 150;
            tm.Tick += delegate { this.pictureBox_main.Invalidate(); };
            // world.RenderModel(this.button_start.CreateGraphics());
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


        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Starting everything");
            animate = true;
            //tm.Start();
        }

        private void pictureBox_main_Paint(object sender, PaintEventArgs e)
        {
 			// should do this a long time ago
			Graphics g = e.Graphics;
            // g.TranslateTransform(100, 100);
            //RecursiveRender(Current_Scene.mRootNode);
            // g.DrawRectangle(new Pen(Color.Blue, 10.0f), new Rectangle(500, 200, 40, 70));
            g.DrawRectangle(new Pen(Color.Blue, 10.0f), pictureBox_main.DisplayRectangle);
            // g.DrawEllipse(Pens.Green, new Rectangle(0, 0, 10, 10));

            // g.DrawPoint(new Point(500, 50));

            // g.ScaleTransform(15.0f, 15.0f);
            GraphicsState gr1 = g.Save();
            g.ScaleTransform(3.0f, 3.0f);
            world.ApplyMatrix(g);
            g.Restore(gr1);
            world.RenderModel(g);

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
    }
}
