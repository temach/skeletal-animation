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
        public MyForm()
        {
            InitializeComponent();
            LoadModel();
        }
        
		protected override void OnPaint(PaintEventArgs pea)
		{
			// should do this a long time ago
			Graphics g = pea.Graphics;
            g.DrawRectangle(new Pen(Color.AliceBlue, 3.0f), new Rectangle(10, 10, 500, 500));

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


		protected void DoRedraw()
		{
			Invalidate ();
			foreach (Control ctrl in Controls)
				ctrl.Invalidate();
		}

		protected override void OnMove (EventArgs ea)
		{
			base.OnMove (ea);
            DoRedraw();
		}

        static void LoadModel()
        {
            //Filepath to our model
            string curdir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
            String fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Seymour.dae");

            //Create a new importer
            AssimpContext importer = new AssimpContext();

            //This is how we add a configuration (each config is its own class)
            NormalSmoothingAngleConfig config = new NormalSmoothingAngleConfig(66.0f);
            importer.SetConfig(config);

            //This is how we add a logging callback 
            LogStream logstream = new LogStream(delegate (String msg, String userData) {
                Console.WriteLine(msg);
            });
            logstream.Attach();

            //Import the model. All configs are set. The model
            //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
            Scene model = importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeMaximumQuality);

            Console.WriteLine("All is good artem !!!!!");
            //TODO: Load the model data into your own structures

            //End of example
            importer.Dispose();
        }

    }
}
