namespace WinFormAnimation2D
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_is_selected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_mouse_coords = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_camera_position = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_entity_position = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_AnimTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackBar_time = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.glControl1 = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tabPage_RenderOptions = new System.Windows.Forms.TabPage();
            this.checkBox_render_boxes = new System.Windows.Forms.CheckBox();
            this.checkBox_renderBones = new System.Windows.Forms.CheckBox();
            this.button_ResetCamera = new System.Windows.Forms.Button();
            this.checkBox_breakpoints_on = new System.Windows.Forms.CheckBox();
            this.checkBox_OrbitingCamera = new System.Windows.Forms.CheckBox();
            this.checkBox_RenderNormals = new System.Windows.Forms.CheckBox();
            this.checkBox_playall = new System.Windows.Forms.CheckBox();
            this.checkBox_OpenGL_Material = new System.Windows.Forms.CheckBox();
            this.button_step_frame = new System.Windows.Forms.Button();
            this.checkBox_OpenGLDrawAxis = new System.Windows.Forms.CheckBox();
            this.tabPage_TreeView = new System.Windows.Forms.TabPage();
            this.treeView_entity_info = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl_panel = new System.Windows.Forms.TabControl();
            this.button_back_one_frame = new System.Windows.Forms.Button();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_time)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabPage_RenderOptions.SuspendLayout();
            this.tabPage_TreeView.SuspendLayout();
            this.tabControl_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel_is_selected,
            this.toolStripStatusLabel_mouse_coords,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel_camera_position,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel_entity_position,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel_AnimTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(958, 22);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(46, 19);
            this.toolStripStatusLabel1.Text = "mouse:";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // toolStripStatusLabel_is_selected
            // 
            this.toolStripStatusLabel_is_selected.AutoSize = false;
            this.toolStripStatusLabel_is_selected.Name = "toolStripStatusLabel_is_selected";
            this.toolStripStatusLabel_is_selected.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabel_is_selected.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel_is_selected.Visible = false;
            // 
            // toolStripStatusLabel_mouse_coords
            // 
            this.toolStripStatusLabel_mouse_coords.AutoSize = false;
            this.toolStripStatusLabel_mouse_coords.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_mouse_coords.Name = "toolStripStatusLabel_mouse_coords";
            this.toolStripStatusLabel_mouse_coords.Size = new System.Drawing.Size(140, 19);
            this.toolStripStatusLabel_mouse_coords.Text = "toolStripStatusLabel2";
            this.toolStripStatusLabel_mouse_coords.Visible = false;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(49, 19);
            this.toolStripStatusLabel2.Text = "camera:";
            this.toolStripStatusLabel2.Visible = false;
            // 
            // toolStripStatusLabel_camera_position
            // 
            this.toolStripStatusLabel_camera_position.AutoSize = false;
            this.toolStripStatusLabel_camera_position.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_camera_position.Name = "toolStripStatusLabel_camera_position";
            this.toolStripStatusLabel_camera_position.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_camera_position.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel_camera_position.Visible = false;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(40, 19);
            this.toolStripStatusLabel3.Text = "entity:";
            this.toolStripStatusLabel3.Visible = false;
            // 
            // toolStripStatusLabel_entity_position
            // 
            this.toolStripStatusLabel_entity_position.AutoSize = false;
            this.toolStripStatusLabel_entity_position.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_entity_position.Name = "toolStripStatusLabel_entity_position";
            this.toolStripStatusLabel_entity_position.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_entity_position.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel_entity_position.Visible = false;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(34, 17);
            this.toolStripStatusLabel4.Text = "time:";
            // 
            // toolStripStatusLabel_AnimTime
            // 
            this.toolStripStatusLabel_AnimTime.Name = "toolStripStatusLabel_AnimTime";
            this.toolStripStatusLabel_AnimTime.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel_AnimTime.Text = "toolStripStatusLabel4";
            // 
            // trackBar_time
            // 
            this.trackBar_time.Location = new System.Drawing.Point(88, 38);
            this.trackBar_time.Maximum = 20;
            this.trackBar_time.Name = "trackBar_time";
            this.trackBar_time.Size = new System.Drawing.Size(578, 45);
            this.trackBar_time.TabIndex = 36;
            this.trackBar_time.ValueChanged += new System.EventHandler(this.trackBar_AnimationTime_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Time bar";
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(12, 78);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(721, 423);
            this.glControl1.TabIndex = 47;
            this.glControl1.VSync = true;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(958, 24);
            this.menuStrip1.TabIndex = 51;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tabPage_RenderOptions
            // 
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_OpenGLDrawAxis);
            this.tabPage_RenderOptions.Controls.Add(this.button_back_one_frame);
            this.tabPage_RenderOptions.Controls.Add(this.button_step_frame);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_OpenGL_Material);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_playall);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_RenderNormals);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_OrbitingCamera);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_breakpoints_on);
            this.tabPage_RenderOptions.Controls.Add(this.button_ResetCamera);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_renderBones);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_render_boxes);
            this.tabPage_RenderOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPage_RenderOptions.Name = "tabPage_RenderOptions";
            this.tabPage_RenderOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_RenderOptions.Size = new System.Drawing.Size(211, 397);
            this.tabPage_RenderOptions.TabIndex = 1;
            this.tabPage_RenderOptions.Text = "Render";
            this.tabPage_RenderOptions.UseVisualStyleBackColor = true;
            // 
            // checkBox_render_boxes
            // 
            this.checkBox_render_boxes.AutoSize = true;
            this.checkBox_render_boxes.Location = new System.Drawing.Point(19, 117);
            this.checkBox_render_boxes.Name = "checkBox_render_boxes";
            this.checkBox_render_boxes.Size = new System.Drawing.Size(93, 17);
            this.checkBox_render_boxes.TabIndex = 44;
            this.checkBox_render_boxes.Text = "Render Boxes";
            this.checkBox_render_boxes.UseVisualStyleBackColor = true;
            this.checkBox_render_boxes.Visible = false;
            this.checkBox_render_boxes.CheckedChanged += new System.EventHandler(this.checkBox_render_boxes_CheckedChanged);
            // 
            // checkBox_renderBones
            // 
            this.checkBox_renderBones.AutoSize = true;
            this.checkBox_renderBones.Location = new System.Drawing.Point(19, 140);
            this.checkBox_renderBones.Name = "checkBox_renderBones";
            this.checkBox_renderBones.Size = new System.Drawing.Size(94, 17);
            this.checkBox_renderBones.TabIndex = 43;
            this.checkBox_renderBones.Text = "Render Bones";
            this.checkBox_renderBones.UseVisualStyleBackColor = true;
            this.checkBox_renderBones.CheckedChanged += new System.EventHandler(this.checkBox_renderBones_CheckedChanged);
            // 
            // button_ResetCamera
            // 
            this.button_ResetCamera.Location = new System.Drawing.Point(19, 11);
            this.button_ResetCamera.Name = "button_ResetCamera";
            this.button_ResetCamera.Size = new System.Drawing.Size(75, 35);
            this.button_ResetCamera.TabIndex = 38;
            this.button_ResetCamera.Text = "Camera reset";
            this.button_ResetCamera.UseVisualStyleBackColor = true;
            this.button_ResetCamera.Click += new System.EventHandler(this.button_ResetCamera_Click);
            // 
            // checkBox_breakpoints_on
            // 
            this.checkBox_breakpoints_on.AutoSize = true;
            this.checkBox_breakpoints_on.Location = new System.Drawing.Point(19, 52);
            this.checkBox_breakpoints_on.Name = "checkBox_breakpoints_on";
            this.checkBox_breakpoints_on.Size = new System.Drawing.Size(118, 17);
            this.checkBox_breakpoints_on.TabIndex = 27;
            this.checkBox_breakpoints_on.Text = "Breakpoints On/Off";
            this.checkBox_breakpoints_on.UseVisualStyleBackColor = true;
            this.checkBox_breakpoints_on.Visible = false;
            this.checkBox_breakpoints_on.CheckedChanged += new System.EventHandler(this.checkBox_breakpoints_on_CheckedChanged);
            // 
            // checkBox_OrbitingCamera
            // 
            this.checkBox_OrbitingCamera.AutoSize = true;
            this.checkBox_OrbitingCamera.Location = new System.Drawing.Point(19, 163);
            this.checkBox_OrbitingCamera.Name = "checkBox_OrbitingCamera";
            this.checkBox_OrbitingCamera.Size = new System.Drawing.Size(101, 17);
            this.checkBox_OrbitingCamera.TabIndex = 50;
            this.checkBox_OrbitingCamera.Text = "Orbiting Camera";
            this.checkBox_OrbitingCamera.UseVisualStyleBackColor = true;
            this.checkBox_OrbitingCamera.CheckedChanged += new System.EventHandler(this.checkBox_OrbitingCamera_CheckedChanged);
            // 
            // checkBox_RenderNormals
            // 
            this.checkBox_RenderNormals.AutoSize = true;
            this.checkBox_RenderNormals.Location = new System.Drawing.Point(19, 76);
            this.checkBox_RenderNormals.Name = "checkBox_RenderNormals";
            this.checkBox_RenderNormals.Size = new System.Drawing.Size(122, 17);
            this.checkBox_RenderNormals.TabIndex = 51;
            this.checkBox_RenderNormals.Text = "Render with normals";
            this.checkBox_RenderNormals.UseVisualStyleBackColor = true;
            this.checkBox_RenderNormals.CheckedChanged += new System.EventHandler(this.checkBox_RenderNormals_CheckedChanged);
            // 
            // checkBox_playall
            // 
            this.checkBox_playall.AutoSize = true;
            this.checkBox_playall.Location = new System.Drawing.Point(19, 199);
            this.checkBox_playall.Name = "checkBox_playall";
            this.checkBox_playall.Size = new System.Drawing.Size(94, 17);
            this.checkBox_playall.TabIndex = 52;
            this.checkBox_playall.Text = "Play animation";
            this.checkBox_playall.UseVisualStyleBackColor = true;
            this.checkBox_playall.CheckedChanged += new System.EventHandler(this.checkBox_playall_CheckedChanged);
            // 
            // checkBox_OpenGL_Material
            // 
            this.checkBox_OpenGL_Material.AutoSize = true;
            this.checkBox_OpenGL_Material.Location = new System.Drawing.Point(19, 222);
            this.checkBox_OpenGL_Material.Name = "checkBox_OpenGL_Material";
            this.checkBox_OpenGL_Material.Size = new System.Drawing.Size(91, 17);
            this.checkBox_OpenGL_Material.TabIndex = 53;
            this.checkBox_OpenGL_Material.Text = "Apply material";
            this.checkBox_OpenGL_Material.UseVisualStyleBackColor = true;
            this.checkBox_OpenGL_Material.CheckedChanged += new System.EventHandler(this.checkBox_OpenGL_Material_CheckedChanged);
            // 
            // button_step_frame
            // 
            this.button_step_frame.Location = new System.Drawing.Point(19, 245);
            this.button_step_frame.Name = "button_step_frame";
            this.button_step_frame.Size = new System.Drawing.Size(129, 23);
            this.button_step_frame.TabIndex = 54;
            this.button_step_frame.Text = "Small jump forward";
            this.button_step_frame.UseVisualStyleBackColor = true;
            this.button_step_frame.Click += new System.EventHandler(this.button_step_frame_Click);
            // 
            // checkBox_OpenGLDrawAxis
            // 
            this.checkBox_OpenGLDrawAxis.AutoSize = true;
            this.checkBox_OpenGLDrawAxis.Location = new System.Drawing.Point(19, 304);
            this.checkBox_OpenGLDrawAxis.Name = "checkBox_OpenGLDrawAxis";
            this.checkBox_OpenGLDrawAxis.Size = new System.Drawing.Size(89, 17);
            this.checkBox_OpenGLDrawAxis.TabIndex = 57;
            this.checkBox_OpenGLDrawAxis.Text = "Draw 3D axis";
            this.checkBox_OpenGLDrawAxis.UseVisualStyleBackColor = true;
            this.checkBox_OpenGLDrawAxis.CheckedChanged += new System.EventHandler(this.checkBox_OpenGLDrawAxis_CheckedChanged);
            // 
            // tabPage_TreeView
            // 
            this.tabPage_TreeView.Controls.Add(this.label2);
            this.tabPage_TreeView.Controls.Add(this.treeView_entity_info);
            this.tabPage_TreeView.Location = new System.Drawing.Point(4, 22);
            this.tabPage_TreeView.Name = "tabPage_TreeView";
            this.tabPage_TreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TreeView.Size = new System.Drawing.Size(211, 397);
            this.tabPage_TreeView.TabIndex = 0;
            this.tabPage_TreeView.Text = "Tree";
            this.tabPage_TreeView.UseVisualStyleBackColor = true;
            // 
            // treeView_entity_info
            // 
            this.treeView_entity_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_entity_info.Location = new System.Drawing.Point(6, 19);
            this.treeView_entity_info.Name = "treeView_entity_info";
            this.treeView_entity_info.Size = new System.Drawing.Size(199, 372);
            this.treeView_entity_info.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Currently selected entity:";
            // 
            // tabControl_panel
            // 
            this.tabControl_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_panel.Controls.Add(this.tabPage_TreeView);
            this.tabControl_panel.Controls.Add(this.tabPage_RenderOptions);
            this.tabControl_panel.Location = new System.Drawing.Point(739, 78);
            this.tabControl_panel.Name = "tabControl_panel";
            this.tabControl_panel.SelectedIndex = 0;
            this.tabControl_panel.Size = new System.Drawing.Size(219, 423);
            this.tabControl_panel.TabIndex = 50;
            // 
            // button_back_one_frame
            // 
            this.button_back_one_frame.Location = new System.Drawing.Point(19, 274);
            this.button_back_one_frame.Name = "button_back_one_frame";
            this.button_back_one_frame.Size = new System.Drawing.Size(129, 23);
            this.button_back_one_frame.TabIndex = 56;
            this.button_back_one_frame.Text = "Play back one keyframe";
            this.button_back_one_frame.UseVisualStyleBackColor = true;
            this.button_back_one_frame.Visible = false;
            this.button_back_one_frame.Click += new System.EventHandler(this.button_back_one_frame_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("recentToolStripMenuItem.Image")));
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recentToolStripMenuItem.Text = "Open &Recent";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 528);
            this.Controls.Add(this.tabControl_panel);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.trackBar_time);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_time)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage_RenderOptions.ResumeLayout(false);
            this.tabPage_RenderOptions.PerformLayout();
            this.tabPage_TreeView.ResumeLayout(false);
            this.tabPage_TreeView.PerformLayout();
            this.tabControl_panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_is_selected;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_mouse_coords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_camera_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TrackBar trackBar_time;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_entity_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_AnimTime;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TabPage tabPage_RenderOptions;
        private System.Windows.Forms.CheckBox checkBox_OpenGLDrawAxis;
        private System.Windows.Forms.Button button_back_one_frame;
        private System.Windows.Forms.Button button_step_frame;
        private System.Windows.Forms.CheckBox checkBox_OpenGL_Material;
        private System.Windows.Forms.CheckBox checkBox_playall;
        private System.Windows.Forms.CheckBox checkBox_RenderNormals;
        private System.Windows.Forms.CheckBox checkBox_OrbitingCamera;
        private System.Windows.Forms.CheckBox checkBox_breakpoints_on;
        private System.Windows.Forms.Button button_ResetCamera;
        private System.Windows.Forms.CheckBox checkBox_renderBones;
        private System.Windows.Forms.CheckBox checkBox_render_boxes;
        private System.Windows.Forms.TabPage tabPage_TreeView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView_entity_info;
        private System.Windows.Forms.TabControl tabControl_panel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

