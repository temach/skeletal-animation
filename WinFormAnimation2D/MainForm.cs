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
        private GUIConfig _gui_conf = new GUIConfig();
        private Entity _currently_selected;

        // camera related stuff
        private Drawing2DCamera _camera;
        private readonly float _init_zoom = 1.0f;
        private Point PictureBoxCenterPoint
        {
            get
            {
                return new Point(this.pictureBox_main.Width / 2, this.pictureBox_main.Height / 2);
            }
        }


        public MainForm()
        {
            InitializeComponent();
            Matrix init_camera = new Matrix();
            //init_camera.Scale(_init_zoom, _init_zoom);
            _camera = new Drawing2DCamera(init_camera);
            // we have to manually register the mousewheel event handler.
            this.MouseWheel += new MouseEventHandler(this.pictureBox_main_MouseMove);
            tm.Interval = 150;
            tm.Tick += delegate { this.pictureBox_main.Invalidate(); };
            // world.RenderModel(this.button_start.CreateGraphics());
            _world = new World(this.pictureBox_main);
            InitFillTreeFromWorldSingleEntity();
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
            //_camera.CamMatrix = _camera.CamMatrix.eSnapScale(1.0);
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
                this.treeView_entity_info.SelectedNode = this.treeView_entity_info.Nodes[_currently_selected.Name];
            }
            else
            {
                _currently_selected = null;
                this.toolStripStatusLabel_entity_position.Text = "none";
                this.toolStripStatusLabel_entity_rotation.Text = "none";
                this.treeView_entity_info.SelectedNode = null;
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

        /// <summary>
        /// Initialise the side tree view to show the scene.
        /// </summary>
        private void InitFillTreeFromWorldSingleEntity()
        {
            this.treeView_entity_info.Nodes.Clear();
            // make root node and build whole tree
            var root_nd = new CustomTreeNode(NodeType.Other);
            root_nd.Text = "scene";
            var ent_tree = FillTree(_world._enttity_one);
            root_nd.Nodes.Add(ent_tree);
            // expand nodes until you find one with at least two children
            TreeNode cur_nd = root_nd;
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
        private TreeNode FillTree(Entity ent)
        {
            var papa = new CustomTreeNode(NodeType.Entity);
            papa.Name = ent.Name;
            papa.Text = ent.Name;
            papa.DrawData = Rectangle.Round(ent._extra_geometry._entity_border._rect);
            foreach (AxiAlignedBoundingBox border in ent._extra_geometry._mesh_borders)
            {
                var mesh_view_nd = new CustomTreeNode(NodeType.Mesh);
                papa.Nodes.Add(mesh_view_nd);
                mesh_view_nd.Text = ((Mesh)border.Source).Name;
                mesh_view_nd.DrawData = Rectangle.Round(border._rect);
            }
            return papa;
        }

        private void HighlightSlectedNode()
        {
            var view_nd = (CustomTreeNode)this.treeView_entity_info.SelectedNode;
            if (view_nd != null)
            {
                if (view_nd.NodeType == NodeType.Entity || view_nd.NodeType == NodeType.Mesh)
                {
                    // we must draw every mesh inside the entity
                    Rectangle draw_data = (Rectangle)view_nd.DrawData;
                    draw_data.Inflate(10, 10);
                    var coords = new Point[]
                    {
                        new Point(draw_data.Left, draw_data.Top),
                        new Point(draw_data.Right, draw_data.Top),
                        new Point(draw_data.Right, draw_data.Bottom),
                        new Point(draw_data.Left, draw_data.Bottom),
                    };
                    Util.GR.DrawClosedCurve(Pens.Black, coords);
                }
            }
        }

        private void pictureBox_main_Paint(object sender, PaintEventArgs e)
        {
            this.toolStripStatusLabel_mouse_coords.Text = _m_status.InnerWorldPos.ToString();
            // Set GR field so that we can use Sysem.Drawing2D as if it was like OpenGL
            Util.GR = e.Graphics;
            _world._renderer.SetupRender(Util.GR);
            // camera center in sceen coords
            Util.GR.eDrawCircle(Util.pp4, PictureBoxCenterPoint, 3);
            // mouse unaffected
            Util.GR.eDrawCircle(Util.pp2, Point.Round(_m_status.InnerWorldPos), 3);
            // model unaffected
            GraphicsState gs = Util.GR.Save();
            _world._enttity_one.RenderModel(_world._renderer.GlobalDrawConf);
            Util.GR.Restore(gs);
            // currently selected in tree view, unaffected
            HighlightSlectedNode();
            // change to world (i.e. camera) coordinates
            Util.GR.MultiplyTransform(_camera.MatrixToDrawing2D());
            // center
            Util.GR.eDrawCircle(Util.pp2, new Point(0,0), 3);
            // mouse position, big-green-circle should be under the mouse arrow
            Util.GR.eDrawBigPoint(_m_status.InnerWorldPos);
            // render entity
            _world.RenderWorld();
            // currently selected in tree view
            HighlightSlectedNode();
        }

        private void checkBox_breakpoints_on_CheckedChanged(object sender, EventArgs e)
        {
            Breakpoints.Allow = this.checkBox_breakpoints_on.Checked;
        }

        // when the selected item has been changed
        private void treeView_entity_info_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void button_PreviousKeyframe_Click(object sender, EventArgs e)
        {
            if (_currently_selected == null)
            {
                return;
            }
            _world._silly_waving_action.SnapToKeyframe(_currently_selected._armature
                , _world._silly_waving_action._keyframe - 1
                , _world._keyframe_blend);
            this.Text = _world._silly_waving_action._keyframe.ToString();
        }

        private void button_NextKeyframe_Click(object sender, EventArgs e)
        {
            if (_currently_selected == null)
            {
                return;
            }
            _world._silly_waving_action.SnapToKeyframe(_currently_selected._armature
                , _world._silly_waving_action._keyframe + 1
                , _world._keyframe_blend);
            this.Text = _world._silly_waving_action._keyframe.ToString();
        }

        // sets the blend value for current keyframe
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _world._keyframe_blend = (sender as TrackBar).Value / 10.0;
        }
    }
}
