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
            var ent_tree = FillTreeRecur(_world._enttity_one);
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
        private TreeNode FillTreeRecur(Entity ent)
        {
            var papa = new CustomTreeNode(NodeType.Entity);
            papa.Name = ent.Name;
            papa.Text = ent.Name;
            papa.DrawData = Rectangle.Round(ent._extra_geometry._entity_border._rect);
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
                var mesh_view_nd = new CustomTreeNode(NodeType.Mesh);
                view_nd.Nodes.Add(mesh_view_nd);
                mesh_view_nd.Text = nd.Name;
                List<Point> tri_faces = new List<Point>();
                Mesh cur_mesh = sc.Meshes[mesh_id];
                for (int i = 0; i < cur_mesh.FaceCount; i++)
                {
                    var cur_view_nd = new CustomTreeNode(NodeType.TriangleFace);
                    cur_view_nd.Text = string.Format(fmt, i) + cur_mesh.Faces[i].ToString();
                    // list of 3 vertices as PointF
                    var array_tri_face = cur_mesh.Faces[i].Indices.Select(index => cur_mesh.Vertices[index].eToPoint()).ToArray();
                    cur_view_nd.DrawData = array_tri_face;
                    tri_faces.AddRange(array_tri_face);
                    mesh_view_nd.Nodes.Add(cur_view_nd);
                }
                mesh_view_nd.DrawData = tri_faces.ToArray();
            }
            foreach (Node child in nd.Children)
            {
                RecursiveRenderSystemDrawing(view_nd, sc, child);
            }
        }

        public Point GetCenterOfPoints(IEnumerable<Point> input)
        {
            var res = new Point();
            foreach (var p in input)
            {
                res.X += p.X;
                res.Y += p.Y;
            }
            res.X /= input.Count();
            res.Y /= input.Count();
            return res;
        }

        public Point ShiftOutwardFrom(Point p, Point from)
        {
            // p + (p - midpoint).Normalise() * 5
            Point unit = p.Minus(from);
            double len = Math.Sqrt(unit.X * unit.X + unit.Y * unit.Y);
            unit.X = (int)(100 * unit.X);
            unit.Y = (int)(100 * unit.Y);
            //unit.X = (int)(40 * unit.X / len);
            //unit.Y = (int)(40 * unit.Y / len);
            return p.Add(unit);
        }

        private void HighlightSlectedNode()
        {
            var view_nd = (CustomTreeNode)this.treeView_entity_info.SelectedNode;
            if (view_nd != null)
            {
                if (view_nd.NodeType == NodeType.Entity)
                {
                    // we must draw every mesh inside the entity
                    Rectangle draw_data = (Rectangle)view_nd.DrawData;
                    // inflate by 5 pixels
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
                else if (view_nd.NodeType == NodeType.Mesh)
                {
                    // we must draw every triangle inside the mesh
                    Point[] draw_data = (Point[])view_nd.DrawData;
                    // draw in triplets (each face is  vertices
                    for (int i = 0; i < draw_data.Length; )
                    {
                        var tri_face = draw_data.Skip(i).Take(3);
                        Point mid_point = GetCenterOfPoints(tri_face);
                        tri_face = tri_face.Select(p => ShiftOutwardFrom(p, mid_point));
                        Util.GR.DrawClosedCurve(Pens.Black, tri_face.ToArray());
                        i += 3;     // Note the +3
                    }
                }
                else if (view_nd.NodeType == NodeType.TriangleFace)
                {
                    // we must draw every vertex in the triangle (and join them)
                    Point[] draw_data = (Point[]) view_nd.DrawData;
                    Point mid_point = GetCenterOfPoints(draw_data);
                    var tri_face = draw_data.Select(p => ShiftOutwardFrom(p, mid_point));
                    Util.GR.DrawClosedCurve(Pens.Black, tri_face.ToArray());
                }
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
            // show what is currently selected in tree view
            HighlightSlectedNode();
            // Applying camera transform is good here.
            Util.GR.MultiplyTransform(_camera.CamMatrix);
            // draw in camera coordinates
            Util.GR.eDrawBigPoint(_m_status.InnerWorldPos);
            this.toolStripStatusLabel_mouse_coords.Text = _m_status.InnerWorldPos.ToString();
            // apply entity specific matrix on top of camera matrix and render the entity
            _world.RenderWorld(_camera.CamMatrix);
            // show what is currently selected in tree view, but in world coords now
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

    }
}
