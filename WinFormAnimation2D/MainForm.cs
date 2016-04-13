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
using OpenTK;

namespace WinFormAnimation2D
{
    public partial class MainForm : Form
    {

        MouseState _m_status = new MouseState();

        private World _world;
        private Timer tm = new Timer();

        // State of the camera currently. We can affect this with buttons.
        private GUIConfig _gui_conf = new GUIConfig();
        private CommandLine _cmd;

        private Entity _current;
        private Entity Current
        {
            get { return _current; }
            set {
                _current = value;
                _cmd._current = value;
            }
        }

        // camera related stuff
        private Drawing2DCamera _camera;
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
            Matrix4 init_camera = Matrix4.Identity;
            _camera = new Drawing2DCamera(init_camera);
            // manually register the mousewheel event handler.
            this.MouseWheel += new MouseEventHandler(this.pictureBox_main_MouseMove);
            tm.Interval = 1;
            _world = new World(this.pictureBox_main);
            try
            {
                _world.LoadScene(Properties.Resources.triple_fold_3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _cmd = new CommandLine(this.pictureBox_main, _world, tm, this.listBox_display, this);
            InitFillTreeFromWorldSingleEntity();
        }

        public void SetAnimTime(double val)
        {
            this.toolStripStatusLabel_AnimTime.Text = val.ToString();
        }

        /// <summary>
        /// Intercept arrow keys to send input to the picture box.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.textBox_cli.Focused == true)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            //for the active control to see the keypress, return false
            switch (keyData)
            {
                case Keys.I:
                case Keys.O:
                    if (Current == null)
                    {
                        _camera.RotateByKey(new KeyEventArgs(keyData));
                    }
                    else
                    { 
                        Current.RotateByKey(new KeyEventArgs(keyData));
                    }
                    this.pictureBox_main.Invalidate();
                    return true;
                case Keys.A:
                case Keys.D:
                case Keys.S:
                case Keys.W:
                    if (Current == null)
                    {
                        _camera.MoveByKey(new KeyEventArgs(keyData));
                        this.toolStripStatusLabel_camera_position.Text = _camera.GetTranslation.ToString();
                    }
                    else
                    {
                        Current.MoveByKey(new KeyEventArgs(keyData));
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
            tm.Tick += _cmd.ClearScreen;
            if (tm.Enabled == false)
            {
                tm.Start();
            }
        }
        private void button_stop_colors_Click(object sender, EventArgs e)
        {
            tm.Tick -= _cmd.ClearScreen;
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
                Current = _world.CurrentlySelected;
                this.toolStripStatusLabel_entity_position.Text = Current.GetTranslation.ToString();
                this.treeView_entity_info.SelectedNode = this.treeView_entity_info.Nodes[Current.Name];
            }
            else
            {
                Current = null;
                this.toolStripStatusLabel_entity_position.Text = "none";
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
            var root_nd = new SceneTreeNode("root");
            var scene_nd = new SceneTreeNode("Scene");
            // make entity tree
            var ent_one = new EntityTreeNode(_world._enttity_one.Name);
            ent_one.DrawData = _world._enttity_one._extra_geometry.EntityBox;
            // make entity mesh
            MeshTreeNode ent_mesh_nodes = MakeMeshTree(_world._enttity_one, _world._enttity_one._node);
            // make entity armature
            ArmatureTreeNode ent_arma_nodes = MakeArmatureTree(_world._enttity_one, _world._enttity_one._armature);
            root_nd.Nodes.Add(ent_one);
            ent_one.Nodes.Add(ent_mesh_nodes);
            ent_one.Nodes.Add(ent_arma_nodes);
            ent_arma_nodes.BackColor = Color.LightBlue;
            ent_mesh_nodes.BackColor = Color.LightGreen;
            ent_one.BackColor = Color.Gold;
            // attach and refresh
            this.treeView_entity_info.Nodes.Add(root_nd);
            // show the entity node
            ent_one.EnsureVisible();
            this.treeView_entity_info.Invalidate();
        }

        private MeshTreeNode MakeMeshTree(Entity ent, Node nd)
        {
            var current = new MeshTreeNode(nd.Name);
            foreach (int mesh_id in nd.MeshIndices)
            {
                AxiAlignedBoundingBox aabb = ent._extra_geometry._mesh_id2box[mesh_id];
                string mesh_name = _world._cur_scene._inner.Meshes[mesh_id].Name;
                var mesh_view_nd = new MeshTreeNode(mesh_name);
                mesh_view_nd.DrawData = aabb;
                current.Nodes.Add(mesh_view_nd);
            }
            // get a bounding box that covers all of the meshes assigned to this node
            current.DrawData = ent._extra_geometry.GetCoveringBoundingBox(
                current.Nodes.Cast<MeshTreeNode>().Select(mesh_nd => mesh_nd.DrawData)
            );
            foreach (var child_nd in nd.Children)
            {
                var treeview_child = MakeMeshTree(ent, child_nd);
                current.Nodes.Add(treeview_child);
            }
            return current;
        }

        private ArmatureTreeNode MakeArmatureTree(Entity ent, BoneNode nd)
        {
            var current = new ArmatureTreeNode(nd._inner.Name);
            Vector3 start = nd.Parent != null 
                ? nd.Parent.GlobalTransform.ExtractTranslation()
                : Vector3.Zero;
            Vector3 end = nd.GlobalTransform.ExtractTranslation();
            VectorBoundingTriangle btri = new VectorBoundingTriangle(start, end);
            current.DrawData = btri;
            foreach (var child_nd in nd.Children)
            {
                var treeview_child = MakeArmatureTree(ent, child_nd);
                current.Nodes.Add(treeview_child);
            }
            return current;
        }

        private void HighlightSlectedNode()
        {
            var view_nd = (IHighlightableNode)this.treeView_entity_info.SelectedNode;
            if (view_nd != null)
            {
                view_nd.Render();
            }
        }

        private void pictureBox_main_Paint(object sender, PaintEventArgs e)
        {
            this.toolStripStatusLabel_mouse_coords.Text = _m_status.InnerWorldPos.ToString();
            // Set GR field so that we can use Sysem.Drawing2D as if it was like OpenGL
            Util.GR = e.Graphics;
            _world._renderer.SetupRender(Util.GR);
            // change to world (i.e. camera) coordinates
            Util.GR.MultiplyTransform(_camera.MatrixToDrawing2D().eTo3x2());
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

        // use unix style command invocation
        // cmdname arg1 arg2 arg3
        private void button_RunCli_Click(object sender, EventArgs e)
        {
            this._cmd.RunCmd(this.textBox_cli.Text);
        }

        private void trackBar_AnimationTime_ValueChanged(object sender, EventArgs e)
        {
            if (Current == null)
            {
                return;
            }
            double factor = Current._action.TotalDurationSeconds / 10.0;
            double time_seconds = (sender as TrackBar).Value * factor;
            Current._action.SetTime(time_seconds);
            _world._action_one.ApplyAnimation(Current._armature
                , Current._action);
            this.pictureBox_main.Invalidate();
        }

        private void treeView_entity_info_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.pictureBox_main.Invalidate();
        }
    }
}
