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
using OpenTK.Graphics.OpenGL;

namespace WinFormAnimation2D
{
    public partial class MainForm : Form
    {

        MouseState _mouse = new MouseState();

        private World _world;

        private Stopwatch _last_frame_sw = new Stopwatch();
        private double LastFrameDelay;

        private bool LoadOpenGLDone;

        // State of the camera currently. We can affect this with buttons.
        private GUIConfig _gui_conf = new GUIConfig();
        private CommandLine _cmd;

        private IHighlightableNode last_selected_node;

        private Entity _current;
        private Entity Current
        {
            get { return _world._enttity_one; }
            set {
                _current = value;
                _cmd._current = value;
            }
        }

        private int TrackBarTimeRange
        {
            get { return this.trackBar_time.Maximum - this.trackBar_time.Minimum; }
        }

        private KeyboardInput _kbd;

        // camera related stuff
        private CameraDevice _camera;

        public MainForm()
        {
            InitializeComponent();
            _kbd = new KeyboardInput();
            Matrix4 opengl_camera_init = Matrix4.LookAt(0, 50, 500, 0, 0, 0, 0, 1, 0).Inverted();
            _camera = new CameraDevice(opengl_camera_init);
            // manually register the mousewheel event handler.
            this.glControl1.MouseWheel += new MouseEventHandler(this.glControl1_MouseWheel);
            _world = new World();
            try
            {
                _world.LoadScene(Properties.Resources.ninja_simple_rig_1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _cmd = new CommandLine(_world, this);
            _cmd._current = _world._enttity_one;
            InitFillTreeFromWorldSingleEntity();
        }

        public void SetAnimTime(double val)
        {
            this.toolStripStatusLabel_AnimTime.Text = val.ToString();
            // if the user is not working with the track bar
            if (! this.trackBar_time.Focused)
            {
                double factor = TrackBarTimeRange / Current._action.TotalDurationSeconds;
                int track_val = (int)(val * factor);
                this.trackBar_time.Value = track_val;
            }
        }

        /// <summary>
        /// Intercept arrow keys to send input to the picture box.
        /// (for the active control to see the keypress, return false)
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyboardAction action = _kbd.ProcessKeydown(keyData);
            if (action == KeyboardAction.DoRotation)
            {
                Vector3 rotation_axis = _kbd.GetRotationAxis(_kbd.RecentKey);
                _camera.RotateAround(rotation_axis);
                return true;
            }
            else if (action == KeyboardAction.DoMotion)
            {
                Vector3 direction = _kbd.GetDirectionNormalized(_kbd.RecentKey);
                _camera.MoveBy(direction);
                this.toolStripStatusLabel_camera_position.Text = _camera.GetTranslation.ToString();
                return true; // hide this key event from other controls
            }
            else if (action == KeyboardAction.RunCommand)
            {
                // _cmd.RunCmd(this.textBox_cli.Text);
                return true;
            }
            else if (action == KeyboardAction.None)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            Debug.Assert(false, "You forgot to handle some keyboard action");
            return false;
        }

        /// <summary>
        /// Initialise the side tree view to show the scene.
        /// </summary>
        private void InitFillTreeFromWorldSingleEntity()
        {
            this.treeView_entity_info.Nodes.Clear();
            // make root node and build whole tree
            var root_nd = new SceneTreeNode("root");
            // make entity tree
            var ent_one = new EntityTreeNode(_world._enttity_one.Name);
            ent_one.DrawMeshBounds = new BoundingBoxGroup(_world._enttity_one._extra_geometry._mesh_id2box.Values);
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
            // ent_one.EnsureVisible();
            this.treeView_entity_info.ExpandAll();
            ent_arma_nodes.EnsureVisible();
            this.treeView_entity_info.Invalidate();
        }

        private MeshTreeNode MakeMeshTree(Entity ent, Node nd)
        {
            var current = new MeshTreeNode(nd.Name);
            var child_boxes = new List<MeshBounds>();
            if (nd.MeshCount > 1)
            {
                foreach (int mesh_id in nd.MeshIndices)
                {
                    MeshBounds aabb = ent._extra_geometry._mesh_id2box[mesh_id];
                    string mesh_name = _world._cur_scene._inner.Meshes[mesh_id].Name;
                    var mesh_view_nd = new MeshTreeNode(mesh_name);
                    var list = new List<MeshBounds>() { aabb };
                    mesh_view_nd.DrawData = new BoundingBoxGroup(list);
                    child_boxes.Add(aabb);
                    current.Nodes.Add(mesh_view_nd);
                }
                // get a bounding box that covers all of the meshes assigned to this node
                current.DrawData = new BoundingBoxGroup(child_boxes);
            }
            else
            {
                // Place the bounding box of mesh as self bounding box
                MeshBounds aabb = ent._extra_geometry._mesh_id2box[nd.MeshIndices[0]];
                var list = new List<MeshBounds>() { aabb };
                current.DrawData = new BoundingBoxGroup(list);
            }
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
            current.DrawData = ent._extra_geometry._bone_id2triangle[nd._inner.Name];
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
                last_selected_node = view_nd;
                view_nd.Render();
            }
            // last_selected_node is null only on scene load
            else if (last_selected_node != null)
            {
                last_selected_node.Render();
            }
        }

        private void PrepareOpenGLRenderFrame()
        {
            // guard if GLControl has not loaded yet
            if (! LoadOpenGLDone)
            {
                return;
            }
            _world._renderer.ClearOpenglFrameForRender(_camera.MatrixToOpenGL());
            if (Properties.Settings.Default.OpenGLDrawAxis)
            {
                _world._renderer.DrawAxis3D();
            }

            UpdateFrame();

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color3(Color.Green);
        }

        private void RenderBones(Entity ent)
        {
            foreach (var bounds in ent._extra_geometry._bone_id2triangle.Values)
            {
                bounds.Render(Pens.Black);
            }
        }

        // use unix style command invocation
        // cmdname arg1 arg2 arg3
        private void button_RunCli_Click(object sender, EventArgs e)
        {
            // this._cmd.RunCmd(this.textBox_cli.Text);
        }

        private void trackBar_AnimationTime_ValueChanged(object sender, EventArgs e)
        {
            if (Current == null)
            {
                return;
            }
            // if the user changed the value
            if (this.trackBar_time.Focused)
            {
                double factor = Current._action.TotalDurationSeconds / TrackBarTimeRange;
                double time_seconds = (sender as TrackBar).Value * factor;
                Current._action.SetTime(time_seconds);
                _world._action_one.ApplyAnimation(Current._armature
                    , Current._action);
                this.toolStripStatusLabel_AnimTime.Text = time_seconds.ToString();
            }
        }

        private void checkBox_renderBones_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RenderAllBoneBounds = this.checkBox_renderBones.Checked;
            // this._cmd.RunCmd("set RenderAllBoneBounds " + this.checkBox_renderBones.Checked);
        }

        private void checkBox_render_boxes_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RenderAllMeshBounds = this.checkBox_render_boxes.Checked;
            // this._cmd.RunCmd("set RenderAllMeshBounds " + this.checkBox_render_boxes.Checked);
        }

        private void checkBox_breakpoints_on_CheckedChanged(object sender, EventArgs e)
        {
            Breakpoints.Allow = this.checkBox_breakpoints_on.Checked;
        }

        private void checkBox_triangulate_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.TriangulateMesh = this.checkBox_triangulate.Checked;
            // this._cmd.RunCmd("set TriangulateMesh " + this.checkBox_triangulate.Checked);
        }

        private void checkBox_moveCamera_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.MoveCamera = this.checkBox_moveCamera.Checked;
            // this._cmd.RunCmd("set MoveCamera " + this.checkBox_triangulate.Checked);
        }

        private void checkBox_RenderNormals_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RenderNormals = this.checkBox_RenderNormals.Checked;
        }

        private void checkBox_OrbitingCamera_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OrbitingCamera = this.checkBox_OrbitingCamera.Checked;
            button_ResetCamera_Click(null, null);
        }

        private void checkBox_FixCameraPlane_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.FixCameraPlane = this.checkBox_FixCameraPlane.Checked;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            _world._renderer.ResizeOpenGL(this.glControl1.Width, this.glControl1.Height);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            _world._renderer.InitOpenGL();
            // we have to wait for OpenGL to load before uploading VBOs to OpenGL server
            _world._enttity_one.UploadMeshVBO(_world._cur_scene._inner.Materials);
            _world._renderer.ResizeOpenGL(this.glControl1.Width, this.glControl1.Height);
            LoadOpenGLDone = true;
            // register Idle event so we get regular callbacks for drawing
            Application.Idle += ApplicationIdle;
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {           
            if(this.IsDisposed)
            {
                return;
            }
            while (glControl1.IsIdle)
            {
                UpdateFrame();
                RenderFrame();
            }
        }

        private void RenderFrame()
        {
            PrepareOpenGLRenderFrame();
            // render entity
            _world.RenderWorld();
            // currently selected in tree view
            // Disable depth test because we want bones to always be visible
            GL.Disable(EnableCap.DepthTest);
            if (Current != null)
            {
                Current._extra_geometry.UpdateBonePositions(Current._armature);
                if (Properties.Settings.Default.RenderAllBoneBounds)
                {
                    RenderBones(Current);
                }
            }
            HighlightSlectedNode();
            glControl1.SwapBuffers();
            // picture box was not made for such fast updates, we will update it with a timer
            // enable to see the slow speed of OpenGL update
            // glControl1.SwapBuffers();
        }

        private void UpdateFrame()
        {          
            this.toolStripStatusLabel_mouse_coords.Text = _mouse.InnerWorldPos.ToString();
            LastFrameDelay = _last_frame_sw.ElapsedMilliseconds;
            _last_frame_sw.Restart();
            _world.Update(LastFrameDelay);
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            _mouse.RecordMouseClick(e);
            _mouse.RecordInnerWorldMouseClick(_camera.ConvertScreen2WorldCoordinates(_mouse.ClickPos));
            this.toolStripStatusLabel_entity_position.Text = Current.GetTranslation.ToString();
            this.treeView_entity_info.SelectedNode = this.treeView_entity_info.Nodes[Current.Name];
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            _mouse.RecordMouseMove(e);
            _mouse.RecordInnerWorldMouseMove(_camera.ConvertScreen2WorldCoordinates(_mouse.CurrentPos));
            if (_world.CheckMouseEntitySelect(_mouse))
            {
                //this.toolStripStatusLabel_is_selected.Text = "HAS ENTITY";
            }
            else
            {
                //this.toolStripStatusLabel_is_selected.Text = "___empty___";
            }

            // Process mouse motion only if it is pressed
            if (! _mouse.IsPressed) {
                return;
            }
            // time to do some rotation
            _camera.OnMouseMove(_mouse.FrameDelta.X, _mouse.FrameDelta.Y);
            this.toolStripStatusLabel_is_selected.Text = _mouse.FrameDelta.ToString();
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouse.IsPressed = false;
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            _camera.Scroll(Math.Sign(e.Delta));
        }

        private void button_ResetCamera_Click(object sender, EventArgs e)
        {
            Matrix4 opengl_camera_init = Matrix4.LookAt(0, 50, 500, 0, 0, 0, 0, 1, 0).Inverted();
            _camera = new CameraDevice(opengl_camera_init);
        }

        private void checkBox_playall_CheckedChanged(object sender, EventArgs e)
        {
            _cmd.playall(this.checkBox_playall.Checked);
        }

        private void checkBox_OpenGL_Material_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenGLMaterial = this.checkBox_OpenGL_Material.Checked;
        }

        private void button_step_frame_Click(object sender, EventArgs e)
        {
            _cmd.stepall();
        }

        private void button_back_one_frame_Click(object sender, EventArgs e)
        {
            _cmd.bkf();
        }

        private void checkBox_OpenGLDrawAxis_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenGLDrawAxis = this.checkBox_OpenGLDrawAxis.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Курсовая работа \n \"Программа скелетная анимация\" \n Выполнил студент БПИ 151 \n Абрамов Артем Михайлович");
        }
    }
}
