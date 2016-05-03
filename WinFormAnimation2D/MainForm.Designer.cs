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
            this.pictureBox_main = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.treeView_entity_info = new System.Windows.Forms.TreeView();
            this.checkBox_breakpoints_on = new System.Windows.Forms.CheckBox();
            this.trackBar_time = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_cli = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox_display = new System.Windows.Forms.ListBox();
            this.button_RunCli = new System.Windows.Forms.Button();
            this.checkBox_renderBones = new System.Windows.Forms.CheckBox();
            this.checkBox_render_boxes = new System.Windows.Forms.CheckBox();
            this.checkBox_triangulate = new System.Windows.Forms.CheckBox();
            this.checkBox_moveCamera = new System.Windows.Forms.CheckBox();
            this.glControl1 = new OpenTK.GLControl();
            this.checkBox_forceFrameRedraw = new System.Windows.Forms.CheckBox();
            this.checkBox_FixCameraPlane = new System.Windows.Forms.CheckBox();
            this.tabControl_panel = new System.Windows.Forms.TabControl();
            this.tabPage_TreeView = new System.Windows.Forms.TabPage();
            this.tabPage_RenderOptions = new System.Windows.Forms.TabPage();
            this.tabPage_CmdLine = new System.Windows.Forms.TabPage();
            this.checkBox_OrbitingCamera = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_time)).BeginInit();
            this.tabControl_panel.SuspendLayout();
            this.tabPage_TreeView.SuspendLayout();
            this.tabPage_RenderOptions.SuspendLayout();
            this.tabPage_CmdLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_main
            // 
            this.pictureBox_main.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox_main.Location = new System.Drawing.Point(403, 37);
            this.pictureBox_main.Name = "pictureBox_main";
            this.pictureBox_main.Size = new System.Drawing.Size(330, 464);
            this.pictureBox_main.TabIndex = 1;
            this.pictureBox_main.TabStop = false;
            this.pictureBox_main.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_main_Paint);
            this.pictureBox_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseDown);
            this.pictureBox_main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseMove);
            this.pictureBox_main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseUp);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 504);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(958, 24);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(46, 19);
            this.toolStripStatusLabel1.Text = "mouse:";
            // 
            // toolStripStatusLabel_is_selected
            // 
            this.toolStripStatusLabel_is_selected.AutoSize = false;
            this.toolStripStatusLabel_is_selected.Name = "toolStripStatusLabel_is_selected";
            this.toolStripStatusLabel_is_selected.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabel_is_selected.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel_mouse_coords
            // 
            this.toolStripStatusLabel_mouse_coords.AutoSize = false;
            this.toolStripStatusLabel_mouse_coords.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_mouse_coords.Name = "toolStripStatusLabel_mouse_coords";
            this.toolStripStatusLabel_mouse_coords.Size = new System.Drawing.Size(140, 19);
            this.toolStripStatusLabel_mouse_coords.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(49, 19);
            this.toolStripStatusLabel2.Text = "camera:";
            // 
            // toolStripStatusLabel_camera_position
            // 
            this.toolStripStatusLabel_camera_position.AutoSize = false;
            this.toolStripStatusLabel_camera_position.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_camera_position.Name = "toolStripStatusLabel_camera_position";
            this.toolStripStatusLabel_camera_position.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_camera_position.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(40, 19);
            this.toolStripStatusLabel3.Text = "entity:";
            // 
            // toolStripStatusLabel_entity_position
            // 
            this.toolStripStatusLabel_entity_position.AutoSize = false;
            this.toolStripStatusLabel_entity_position.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_entity_position.Name = "toolStripStatusLabel_entity_position";
            this.toolStripStatusLabel_entity_position.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_entity_position.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(34, 19);
            this.toolStripStatusLabel4.Text = "time:";
            // 
            // toolStripStatusLabel_AnimTime
            // 
            this.toolStripStatusLabel_AnimTime.Name = "toolStripStatusLabel_AnimTime";
            this.toolStripStatusLabel_AnimTime.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_AnimTime.Text = "toolStripStatusLabel4";
            // 
            // treeView_entity_info
            // 
            this.treeView_entity_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_entity_info.Location = new System.Drawing.Point(6, 19);
            this.treeView_entity_info.Name = "treeView_entity_info";
            this.treeView_entity_info.Size = new System.Drawing.Size(199, 413);
            this.treeView_entity_info.TabIndex = 26;
            this.treeView_entity_info.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_entity_info_AfterSelect);
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
            this.checkBox_breakpoints_on.CheckedChanged += new System.EventHandler(this.checkBox_breakpoints_on_CheckedChanged);
            // 
            // trackBar_time
            // 
            this.trackBar_time.Location = new System.Drawing.Point(91, 4);
            this.trackBar_time.Name = "trackBar_time";
            this.trackBar_time.Size = new System.Drawing.Size(555, 45);
            this.trackBar_time.TabIndex = 36;
            this.trackBar_time.ValueChanged += new System.EventHandler(this.trackBar_AnimationTime_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Time seconds";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 35);
            this.button1.TabIndex = 38;
            this.button1.Text = "Camera reset";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox_cli
            // 
            this.textBox_cli.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cli.Location = new System.Drawing.Point(6, 42);
            this.textBox_cli.Name = "textBox_cli";
            this.textBox_cli.Size = new System.Drawing.Size(202, 20);
            this.textBox_cli.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Command Line";
            // 
            // listBox_display
            // 
            this.listBox_display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_display.FormattingEnabled = true;
            this.listBox_display.HorizontalScrollbar = true;
            this.listBox_display.Location = new System.Drawing.Point(6, 77);
            this.listBox_display.Name = "listBox_display";
            this.listBox_display.Size = new System.Drawing.Size(202, 355);
            this.listBox_display.TabIndex = 41;
            // 
            // button_RunCli
            // 
            this.button_RunCli.Location = new System.Drawing.Point(86, 13);
            this.button_RunCli.Name = "button_RunCli";
            this.button_RunCli.Size = new System.Drawing.Size(61, 23);
            this.button_RunCli.TabIndex = 42;
            this.button_RunCli.Text = "Run";
            this.button_RunCli.UseVisualStyleBackColor = true;
            this.button_RunCli.Click += new System.EventHandler(this.button_RunCli_Click);
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
            // checkBox_render_boxes
            // 
            this.checkBox_render_boxes.AutoSize = true;
            this.checkBox_render_boxes.Location = new System.Drawing.Point(19, 117);
            this.checkBox_render_boxes.Name = "checkBox_render_boxes";
            this.checkBox_render_boxes.Size = new System.Drawing.Size(93, 17);
            this.checkBox_render_boxes.TabIndex = 44;
            this.checkBox_render_boxes.Text = "Render Boxes";
            this.checkBox_render_boxes.UseVisualStyleBackColor = true;
            this.checkBox_render_boxes.CheckedChanged += new System.EventHandler(this.checkBox_render_boxes_CheckedChanged);
            // 
            // checkBox_triangulate
            // 
            this.checkBox_triangulate.AutoSize = true;
            this.checkBox_triangulate.Location = new System.Drawing.Point(19, 164);
            this.checkBox_triangulate.Name = "checkBox_triangulate";
            this.checkBox_triangulate.Size = new System.Drawing.Size(108, 17);
            this.checkBox_triangulate.TabIndex = 45;
            this.checkBox_triangulate.Text = "Triangulate Mesh";
            this.checkBox_triangulate.UseVisualStyleBackColor = true;
            this.checkBox_triangulate.CheckedChanged += new System.EventHandler(this.checkBox_triangulate_CheckedChanged);
            // 
            // checkBox_moveCamera
            // 
            this.checkBox_moveCamera.AutoSize = true;
            this.checkBox_moveCamera.Location = new System.Drawing.Point(19, 221);
            this.checkBox_moveCamera.Name = "checkBox_moveCamera";
            this.checkBox_moveCamera.Size = new System.Drawing.Size(113, 17);
            this.checkBox_moveCamera.TabIndex = 46;
            this.checkBox_moveCamera.Text = "Move only camera";
            this.checkBox_moveCamera.UseVisualStyleBackColor = true;
            this.checkBox_moveCamera.CheckedChanged += new System.EventHandler(this.checkBox_moveCamera_CheckedChanged);
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(12, 35);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(385, 466);
            this.glControl1.TabIndex = 47;
            this.glControl1.VSync = true;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            // 
            // checkBox_forceFrameRedraw
            // 
            this.checkBox_forceFrameRedraw.AutoSize = true;
            this.checkBox_forceFrameRedraw.Location = new System.Drawing.Point(19, 88);
            this.checkBox_forceFrameRedraw.Name = "checkBox_forceFrameRedraw";
            this.checkBox_forceFrameRedraw.Size = new System.Drawing.Size(104, 17);
            this.checkBox_forceFrameRedraw.TabIndex = 48;
            this.checkBox_forceFrameRedraw.Text = "Forced Redraws";
            this.checkBox_forceFrameRedraw.UseVisualStyleBackColor = true;
            this.checkBox_forceFrameRedraw.CheckedChanged += new System.EventHandler(this.checkBox_forceFrameRedraw_CheckedChanged);
            // 
            // checkBox_FixCameraPlane
            // 
            this.checkBox_FixCameraPlane.AutoSize = true;
            this.checkBox_FixCameraPlane.Location = new System.Drawing.Point(19, 198);
            this.checkBox_FixCameraPlane.Name = "checkBox_FixCameraPlane";
            this.checkBox_FixCameraPlane.Size = new System.Drawing.Size(108, 17);
            this.checkBox_FixCameraPlane.TabIndex = 49;
            this.checkBox_FixCameraPlane.Text = "Fix Camera Plane";
            this.checkBox_FixCameraPlane.UseVisualStyleBackColor = true;
            this.checkBox_FixCameraPlane.CheckedChanged += new System.EventHandler(this.checkBox_FixCameraPlane_CheckedChanged);
            // 
            // tabControl_panel
            // 
            this.tabControl_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_panel.Controls.Add(this.tabPage_TreeView);
            this.tabControl_panel.Controls.Add(this.tabPage_RenderOptions);
            this.tabControl_panel.Controls.Add(this.tabPage_CmdLine);
            this.tabControl_panel.Location = new System.Drawing.Point(739, 37);
            this.tabControl_panel.Name = "tabControl_panel";
            this.tabControl_panel.SelectedIndex = 0;
            this.tabControl_panel.Size = new System.Drawing.Size(219, 464);
            this.tabControl_panel.TabIndex = 50;
            // 
            // tabPage_TreeView
            // 
            this.tabPage_TreeView.Controls.Add(this.label2);
            this.tabPage_TreeView.Controls.Add(this.treeView_entity_info);
            this.tabPage_TreeView.Location = new System.Drawing.Point(4, 22);
            this.tabPage_TreeView.Name = "tabPage_TreeView";
            this.tabPage_TreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TreeView.Size = new System.Drawing.Size(211, 438);
            this.tabPage_TreeView.TabIndex = 0;
            this.tabPage_TreeView.Text = "Tree";
            this.tabPage_TreeView.UseVisualStyleBackColor = true;
            // 
            // tabPage_RenderOptions
            // 
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_OrbitingCamera);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_moveCamera);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_FixCameraPlane);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_breakpoints_on);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_forceFrameRedraw);
            this.tabPage_RenderOptions.Controls.Add(this.button1);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_renderBones);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_render_boxes);
            this.tabPage_RenderOptions.Controls.Add(this.checkBox_triangulate);
            this.tabPage_RenderOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPage_RenderOptions.Name = "tabPage_RenderOptions";
            this.tabPage_RenderOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_RenderOptions.Size = new System.Drawing.Size(211, 438);
            this.tabPage_RenderOptions.TabIndex = 1;
            this.tabPage_RenderOptions.Text = "Render";
            this.tabPage_RenderOptions.UseVisualStyleBackColor = true;
            // 
            // tabPage_CmdLine
            // 
            this.tabPage_CmdLine.Controls.Add(this.listBox_display);
            this.tabPage_CmdLine.Controls.Add(this.label1);
            this.tabPage_CmdLine.Controls.Add(this.button_RunCli);
            this.tabPage_CmdLine.Controls.Add(this.textBox_cli);
            this.tabPage_CmdLine.Location = new System.Drawing.Point(4, 22);
            this.tabPage_CmdLine.Name = "tabPage_CmdLine";
            this.tabPage_CmdLine.Size = new System.Drawing.Size(211, 438);
            this.tabPage_CmdLine.TabIndex = 2;
            this.tabPage_CmdLine.Text = "Cmd Line";
            this.tabPage_CmdLine.UseVisualStyleBackColor = true;
            // 
            // checkBox_OrbitingCamera
            // 
            this.checkBox_OrbitingCamera.AutoSize = true;
            this.checkBox_OrbitingCamera.Location = new System.Drawing.Point(19, 245);
            this.checkBox_OrbitingCamera.Name = "checkBox_OrbitingCamera";
            this.checkBox_OrbitingCamera.Size = new System.Drawing.Size(101, 17);
            this.checkBox_OrbitingCamera.TabIndex = 50;
            this.checkBox_OrbitingCamera.Text = "Orbiting Camera";
            this.checkBox_OrbitingCamera.UseVisualStyleBackColor = true;
            this.checkBox_OrbitingCamera.CheckedChanged += new System.EventHandler(this.checkBox_OrbitingCamera_CheckedChanged);
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
            this.Controls.Add(this.pictureBox_main);
            this.Controls.Add(this.trackBar_time);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_time)).EndInit();
            this.tabControl_panel.ResumeLayout(false);
            this.tabPage_TreeView.ResumeLayout(false);
            this.tabPage_TreeView.PerformLayout();
            this.tabPage_RenderOptions.ResumeLayout(false);
            this.tabPage_RenderOptions.PerformLayout();
            this.tabPage_CmdLine.ResumeLayout(false);
            this.tabPage_CmdLine.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox_main;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_is_selected;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_mouse_coords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_camera_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TreeView treeView_entity_info;
        private System.Windows.Forms.CheckBox checkBox_breakpoints_on;
        private System.Windows.Forms.TrackBar trackBar_time;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_entity_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_AnimTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_cli;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox_display;
        private System.Windows.Forms.Button button_RunCli;
        private System.Windows.Forms.CheckBox checkBox_renderBones;
        private System.Windows.Forms.CheckBox checkBox_render_boxes;
        private System.Windows.Forms.CheckBox checkBox_triangulate;
        private System.Windows.Forms.CheckBox checkBox_moveCamera;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.CheckBox checkBox_forceFrameRedraw;
        private System.Windows.Forms.CheckBox checkBox_FixCameraPlane;
        private System.Windows.Forms.TabControl tabControl_panel;
        private System.Windows.Forms.TabPage tabPage_TreeView;
        private System.Windows.Forms.TabPage tabPage_RenderOptions;
        private System.Windows.Forms.TabPage tabPage_CmdLine;
        private System.Windows.Forms.CheckBox checkBox_OrbitingCamera;
    }
}

