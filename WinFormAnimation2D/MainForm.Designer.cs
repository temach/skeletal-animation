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
            this.button_up = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_right = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_time)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_main
            // 
            this.pictureBox_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_main.Location = new System.Drawing.Point(403, 37);
            this.pictureBox_main.Name = "pictureBox_main";
            this.pictureBox_main.Size = new System.Drawing.Size(330, 466);
            this.pictureBox_main.TabIndex = 1;
            this.pictureBox_main.TabStop = false;
            this.pictureBox_main.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_main_Paint);
            this.pictureBox_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseDown);
            this.pictureBox_main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseMove);
            this.pictureBox_main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseUp);
            // 
            // button_up
            // 
            this.button_up.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_up.Location = new System.Drawing.Point(771, 232);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(52, 23);
            this.button_up.TabIndex = 9;
            this.button_up.Text = "up";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_down
            // 
            this.button_down.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_down.Location = new System.Drawing.Point(771, 288);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(52, 23);
            this.button_down.TabIndex = 10;
            this.button_down.Text = "down";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // button_left
            // 
            this.button_left.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_left.Location = new System.Drawing.Point(741, 259);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(52, 23);
            this.button_left.TabIndex = 11;
            this.button_left.Text = "left";
            this.button_left.UseVisualStyleBackColor = true;
            this.button_left.Click += new System.EventHandler(this.button_left_Click);
            // 
            // button_right
            // 
            this.button_right.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_right.Location = new System.Drawing.Point(799, 259);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(52, 23);
            this.button_right.TabIndex = 12;
            this.button_right.Text = "right";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Click += new System.EventHandler(this.button_right_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(739, 313);
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
            this.treeView_entity_info.Location = new System.Drawing.Point(742, 329);
            this.treeView_entity_info.Name = "treeView_entity_info";
            this.treeView_entity_info.Size = new System.Drawing.Size(194, 172);
            this.treeView_entity_info.TabIndex = 26;
            this.treeView_entity_info.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_entity_info_AfterSelect);
            // 
            // checkBox_breakpoints_on
            // 
            this.checkBox_breakpoints_on.AutoSize = true;
            this.checkBox_breakpoints_on.Location = new System.Drawing.Point(840, 288);
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
            this.trackBar_time.Size = new System.Drawing.Size(390, 45);
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
            this.button1.Location = new System.Drawing.Point(871, 242);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 35);
            this.button1.TabIndex = 38;
            this.button1.Text = "Camera reset";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox_cli
            // 
            this.textBox_cli.Location = new System.Drawing.Point(585, 11);
            this.textBox_cli.Name = "textBox_cli";
            this.textBox_cli.Size = new System.Drawing.Size(280, 20);
            this.textBox_cli.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(502, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Command Line";
            // 
            // listBox_display
            // 
            this.listBox_display.FormattingEnabled = true;
            this.listBox_display.HorizontalScrollbar = true;
            this.listBox_display.Location = new System.Drawing.Point(743, 50);
            this.listBox_display.Name = "listBox_display";
            this.listBox_display.Size = new System.Drawing.Size(203, 95);
            this.listBox_display.TabIndex = 41;
            // 
            // button_RunCli
            // 
            this.button_RunCli.Location = new System.Drawing.Point(871, 9);
            this.button_RunCli.Name = "button_RunCli";
            this.button_RunCli.Size = new System.Drawing.Size(75, 23);
            this.button_RunCli.TabIndex = 42;
            this.button_RunCli.Text = "Run";
            this.button_RunCli.UseVisualStyleBackColor = true;
            this.button_RunCli.Click += new System.EventHandler(this.button_RunCli_Click);
            // 
            // checkBox_renderBones
            // 
            this.checkBox_renderBones.AutoSize = true;
            this.checkBox_renderBones.Location = new System.Drawing.Point(743, 152);
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
            this.checkBox_render_boxes.Location = new System.Drawing.Point(856, 152);
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
            this.checkBox_triangulate.Location = new System.Drawing.Point(743, 176);
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
            this.checkBox_moveCamera.Location = new System.Drawing.Point(856, 176);
            this.checkBox_moveCamera.Name = "checkBox_moveCamera";
            this.checkBox_moveCamera.Size = new System.Drawing.Size(91, 17);
            this.checkBox_moveCamera.TabIndex = 46;
            this.checkBox_moveCamera.Text = "Move camera";
            this.checkBox_moveCamera.UseVisualStyleBackColor = true;
            this.checkBox_moveCamera.CheckedChanged += new System.EventHandler(this.checkBox_moveCamera_CheckedChanged);
            // 
            // glControl1
            // 
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
            this.checkBox_forceFrameRedraw.Location = new System.Drawing.Point(856, 199);
            this.checkBox_forceFrameRedraw.Name = "checkBox_forceFrameRedraw";
            this.checkBox_forceFrameRedraw.Size = new System.Drawing.Size(104, 17);
            this.checkBox_forceFrameRedraw.TabIndex = 48;
            this.checkBox_forceFrameRedraw.Text = "Forced Redraws";
            this.checkBox_forceFrameRedraw.UseVisualStyleBackColor = true;
            this.checkBox_forceFrameRedraw.CheckedChanged += new System.EventHandler(this.checkBox_forceFrameRedraw_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 528);
            this.Controls.Add(this.checkBox_forceFrameRedraw);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.checkBox_moveCamera);
            this.Controls.Add(this.checkBox_triangulate);
            this.Controls.Add(this.checkBox_render_boxes);
            this.Controls.Add(this.checkBox_renderBones);
            this.Controls.Add(this.button_RunCli);
            this.Controls.Add(this.listBox_display);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_cli);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox_breakpoints_on);
            this.Controls.Add(this.treeView_entity_info);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_right);
            this.Controls.Add(this.button_left);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.pictureBox_main);
            this.Controls.Add(this.trackBar_time);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_time)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox_main;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_right;
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
    }
}

