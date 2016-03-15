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
    public partial class MainForm : Form
    {

        MouseState _m_status = new MouseState();

        private World _world;
        private Timer tm = new Timer();

        // State of the camera currently. We can affect this with buttons.
        private Drawing2DCamera _camera = new Drawing2DCamera();
        private GUIConfig _gui_conf = new GUIConfig();
        private Entity _currently_selected;

        public MainForm()
        {
            InitializeComponent();
            // we have to manually register the mousewheel event handler.
            this.MouseWheel += new MouseEventHandler(this.pictureBox_main_MouseMove);
            tm.Interval = 150;
            tm.Tick += delegate { this.pictureBox_main.Invalidate(); };
            // world.RenderModel(this.button_start.CreateGraphics());
            _world = new World(this.pictureBox_main);
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
                    if (_currently_selected == null)
                    {
                        _camera.RotateByKey(new KeyEventArgs(keyData));
                        this.toolStripStatusLabel_camera_rotation.Text = _camera.GetRotationAngleDeg.ToString();
                    }
                    else
                    { 
                        _currently_selected.RotateByKey(new KeyEventArgs(keyData));
                    }
                    this.pictureBox_main.Invalidate();
                    return true;
                case Keys.Left:
                case Keys.Right:
                case Keys.Down:
                case Keys.Up:
                    if (_currently_selected == null)
                    {
                        _camera.MoveByKey(new KeyEventArgs(keyData));
                        this.toolStripStatusLabel_camera_position.Text = _camera.GetTranslation.ToString();
                    }
                    else
                    {
                        _currently_selected.MoveByKey(new KeyEventArgs(keyData));
                    }
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

        public PointF PointFromMouseEvent(MouseEventArgs e)
        {
            return new PointF(e.X, e.Y);
        }

        private void pictureBox_main_MouseDown(object sender, MouseEventArgs e)
        {
            var mouse_world_pos = PointFromMouseEvent(e);
            mouse_world_pos = _camera.ConvertScreen2WorldCoordinates(mouse_world_pos);
            _m_status.RecordMouseClick(e, mouse_world_pos);
            if (_world.CheckMouseEntitySelect(_m_status))
            {
                _currently_selected = _world.CurrentlySelected;
                this.toolStripStatusLabel_entity_position.Text = _currently_selected.GetTranslation.ToString();
                this.toolStripStatusLabel_entity_rotation.Text = _currently_selected.GetRotationAngleDeg.ToString();
                FillTreeFromCurrentEntity();
            }
            else
            {
                _currently_selected = null;
                this.toolStripStatusLabel_entity_position.Text = "none";
                this.toolStripStatusLabel_entity_rotation.Text = "none";
                this.treeView_entity_info.Nodes.Clear();
            }
        }

        private void pictureBox_main_MouseMove(object sender, MouseEventArgs e)
        {
            PointF mouse_world_pos = PointFromMouseEvent(e);
            mouse_world_pos = _camera.ConvertScreen2WorldCoordinates(mouse_world_pos);
            _m_status.RecordMouseMove(e, mouse_world_pos);
            if (_world.CheckMouseEntitySelect(_m_status))
            {
                this.toolStripStatusLabel_is_selected.Text = "HAS ENTITY";
            }
            else
            {
                this.toolStripStatusLabel_is_selected.Text = "___empty___";
            }

            if (Math.Abs(e.Delta) >= 1)
            {
                _camera.ProcessScroll(e.Delta);
            }
            // Process mouse motion only if it is pressed
            if (! _m_status.IsPressed) {
                return;
            }
            // this.Text = _m_status._mouse_x_captured.ToString();
            var delta_x = e.X - _m_status.ClickPos.X;
            var delta_y = e.Y - _m_status.ClickPos.Y;
            if (Math.Abs(delta_x) < _m_status.HorizHysteresis 
                && Math.Abs(delta_y)  < _m_status.VertHysteresis)
            {
                return;
            }
            // time to do some rotation
            //_camera.ProcessMouse(delta_x, delta_y);
            pictureBox_main.Invalidate();
        }

        private void FillTreeFromCurrentEntity()
        {
            this.treeView_entity_info.Nodes.Clear();
            // make root node and build whole tree
            var root_nd = new TreeNode();
            root_nd.Text = "scene";
            var ent_tree = FillTreeeRecur(_currently_selected);
            root_nd.Nodes.Add(ent_tree);
            // expand nodes until you find one with at least two children
            var cur_nd = root_nd;
            while (cur_nd.Nodes.Count == 1)
            {
                cur_nd.Toggle();
                cur_nd = cur_nd.Nodes[0];
            }
            // attach and refresh
            this.treeView_entity_info.Nodes.Add(root_nd);
            this.treeView_entity_info.Invalidate();
        }

        /// Render the model stored in EntityScene useing the Graphics object.
        private TreeNode FillTreeeRecur(Entity ent)
        {
            var papa = new TreeNode();
            papa.Text = ent.Name;
            RecursiveRenderSystemDrawing(papa, ent._scene, ent._scene.RootNode);
            return papa;
        }

        //-------------------------------------------------------------------------------------------------
        // TODO: move all the drawing code elsewhere. This should only show the logic of drawing not the actual OpenGL commands.
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        private void RecursiveRenderSystemDrawing(TreeNode view_nd, Scene sc, Node nd)
        {
            string fmt = "face {0}: ";
            foreach(int mesh_id in nd.MeshIndices)
            {
                var mesh_view_nd = new TreeNode();
                view_nd.Nodes.Add(mesh_view_nd);
                mesh_view_nd.Text = nd.Name;
                Mesh cur_mesh = sc.Meshes[mesh_id];
                for (int i = 0; i < cur_mesh.FaceCount; i++)
                {
                    var cur_view_nd = new TreeNode();
                    cur_view_nd.Text = string.Format(fmt, i) + cur_mesh.Faces[i].ToString();
                    mesh_view_nd.Nodes.Add(cur_view_nd);
                }
            }
            foreach (Node child in nd.Children)
            {
                RecursiveRenderSystemDrawing(view_nd, sc, child);
            }
        }


        private void pictureBox_main_Paint(object sender, PaintEventArgs e)
        {
            // Set GR field so that we can use Sysem.Drawing2D as if it was like OpenGL
            Util.GR = e.Graphics;
            _world._renderer.SetupRender(Util.GR);
            // Render the mouse and model (unaffected by transforms)
            Util.GR.eDrawBigPoint(_m_status.InnerWorldPos);
            GraphicsState gs = Util.GR.Save();
            _world._enttity_one.RenderModel(_world._renderer.GlobalDrawConf);
            Util.GR.Restore(gs);
            // Applying camera transform is good here.
            Util.GR.MultiplyTransform(_camera.CamMatrix);
            // draw in camera coordinates
            Util.GR.eDrawBigPoint(_m_status.InnerWorldPos);
            this.toolStripStatusLabel_mouse_coords.Text = _m_status.InnerWorldPos.ToString();
            // apply entity specific matrix on top of camera matrix and render the entity
            _world.RenderWorld(_camera.CamMatrix);
        }

        private void checkBox_breakpoints_on_CheckedChanged(object sender, EventArgs e)
        {
            Breakpoints.Allow = this.checkBox_breakpoints_on.Checked;
        }
    }
}
