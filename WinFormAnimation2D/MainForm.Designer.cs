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
            this.button_start = new System.Windows.Forms.Button();
            this.pictureBox_main = new System.Windows.Forms.PictureBox();
            this.button_up = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_right = new System.Windows.Forms.Button();
            this.button_resetzoom = new System.Windows.Forms.Button();
            this.button_resetpos = new System.Windows.Forms.Button();
            this.button_stop_colors = new System.Windows.Forms.Button();
            this.label_zoom = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_is_selected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_mouse_coords = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_camera_rotation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_camera_position = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_entity_rotation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_entity_position = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView_entity_info = new System.Windows.Forms.TreeView();
            this.checkBox_breakpoints_on = new System.Windows.Forms.CheckBox();
            this.button_NextKeyframe = new System.Windows.Forms.Button();
            this.button_PreviousKeyframe = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ApplyCurrentAnimState = new System.Windows.Forms.Button();
            this.button_PlayKeyframeInterval = new System.Windows.Forms.Button();
            this.button_AllKeyframeIntervals = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(832, 235);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "Start colors";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // pictureBox_main
            // 
            this.pictureBox_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_main.Location = new System.Drawing.Point(12, 37);
            this.pictureBox_main.Name = "pictureBox_main";
            this.pictureBox_main.Size = new System.Drawing.Size(721, 466);
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
            this.button_up.Location = new System.Drawing.Point(845, 148);
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
            this.button_down.Location = new System.Drawing.Point(845, 204);
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
            this.button_left.Location = new System.Drawing.Point(804, 175);
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
            this.button_right.Location = new System.Drawing.Point(884, 175);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(52, 23);
            this.button_right.TabIndex = 12;
            this.button_right.Text = "right";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Click += new System.EventHandler(this.button_right_Click);
            // 
            // button_resetzoom
            // 
            this.button_resetzoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_resetzoom.Location = new System.Drawing.Point(847, 119);
            this.button_resetzoom.Name = "button_resetzoom";
            this.button_resetzoom.Size = new System.Drawing.Size(75, 23);
            this.button_resetzoom.TabIndex = 16;
            this.button_resetzoom.Text = "Reset zoom";
            this.button_resetzoom.UseVisualStyleBackColor = true;
            this.button_resetzoom.Click += new System.EventHandler(this.button_resetzoom_Click);
            // 
            // button_resetpos
            // 
            this.button_resetpos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_resetpos.Location = new System.Drawing.Point(750, 119);
            this.button_resetpos.Name = "button_resetpos";
            this.button_resetpos.Size = new System.Drawing.Size(91, 23);
            this.button_resetpos.TabIndex = 18;
            this.button_resetpos.Text = "Reset Position";
            this.button_resetpos.UseVisualStyleBackColor = true;
            this.button_resetpos.Click += new System.EventHandler(this.button_resetpos_Click);
            // 
            // button_stop_colors
            // 
            this.button_stop_colors.Location = new System.Drawing.Point(832, 264);
            this.button_stop_colors.Name = "button_stop_colors";
            this.button_stop_colors.Size = new System.Drawing.Size(75, 23);
            this.button_stop_colors.TabIndex = 19;
            this.button_stop_colors.Text = "Stop colors";
            this.button_stop_colors.UseVisualStyleBackColor = true;
            this.button_stop_colors.Click += new System.EventHandler(this.button_stop_colors_Click);
            // 
            // label_zoom
            // 
            this.label_zoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_zoom.AutoSize = true;
            this.label_zoom.Location = new System.Drawing.Point(928, 124);
            this.label_zoom.Name = "label_zoom";
            this.label_zoom.Size = new System.Drawing.Size(22, 13);
            this.label_zoom.TabIndex = 17;
            this.label_zoom.Text = "1.0";
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
            this.toolStripStatusLabel_camera_rotation,
            this.toolStripStatusLabel_camera_position,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel_entity_rotation,
            this.toolStripStatusLabel_entity_position});
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
            // toolStripStatusLabel_camera_rotation
            // 
            this.toolStripStatusLabel_camera_rotation.AutoSize = false;
            this.toolStripStatusLabel_camera_rotation.Name = "toolStripStatusLabel_camera_rotation";
            this.toolStripStatusLabel_camera_rotation.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabel_camera_rotation.Text = "toolStripStatusLabel1";
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
            // toolStripStatusLabel_entity_rotation
            // 
            this.toolStripStatusLabel_entity_rotation.AutoSize = false;
            this.toolStripStatusLabel_entity_rotation.Name = "toolStripStatusLabel_entity_rotation";
            this.toolStripStatusLabel_entity_rotation.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_entity_rotation.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel_entity_position
            // 
            this.toolStripStatusLabel_entity_position.AutoSize = false;
            this.toolStripStatusLabel_entity_position.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_entity_position.Name = "toolStripStatusLabel_entity_position";
            this.toolStripStatusLabel_entity_position.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel_entity_position.Text = "toolStripStatusLabel1";
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
            this.checkBox_breakpoints_on.Location = new System.Drawing.Point(804, 293);
            this.checkBox_breakpoints_on.Name = "checkBox_breakpoints_on";
            this.checkBox_breakpoints_on.Size = new System.Drawing.Size(118, 17);
            this.checkBox_breakpoints_on.TabIndex = 27;
            this.checkBox_breakpoints_on.Text = "Breakpoints On/Off";
            this.checkBox_breakpoints_on.UseVisualStyleBackColor = true;
            this.checkBox_breakpoints_on.CheckedChanged += new System.EventHandler(this.checkBox_breakpoints_on_CheckedChanged);
            // 
            // button_NextKeyframe
            // 
            this.button_NextKeyframe.Location = new System.Drawing.Point(629, 8);
            this.button_NextKeyframe.Name = "button_NextKeyframe";
            this.button_NextKeyframe.Size = new System.Drawing.Size(104, 23);
            this.button_NextKeyframe.TabIndex = 28;
            this.button_NextKeyframe.Text = "Next Keyframe";
            this.button_NextKeyframe.UseVisualStyleBackColor = true;
            this.button_NextKeyframe.Click += new System.EventHandler(this.button_NextKeyframe_Click);
            // 
            // button_PreviousKeyframe
            // 
            this.button_PreviousKeyframe.Location = new System.Drawing.Point(511, 8);
            this.button_PreviousKeyframe.Name = "button_PreviousKeyframe";
            this.button_PreviousKeyframe.Size = new System.Drawing.Size(112, 23);
            this.button_PreviousKeyframe.TabIndex = 29;
            this.button_PreviousKeyframe.Text = "Previous Keyframe";
            this.button_PreviousKeyframe.UseVisualStyleBackColor = true;
            this.button_PreviousKeyframe.Click += new System.EventHandler(this.button_PreviousKeyframe_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(350, 5);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 30;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(247, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Blend for animation";
            // 
            // button_ApplyCurrentAnimState
            // 
            this.button_ApplyCurrentAnimState.Location = new System.Drawing.Point(785, 8);
            this.button_ApplyCurrentAnimState.Name = "button_ApplyCurrentAnimState";
            this.button_ApplyCurrentAnimState.Size = new System.Drawing.Size(151, 23);
            this.button_ApplyCurrentAnimState.TabIndex = 32;
            this.button_ApplyCurrentAnimState.Text = "Apply current frame+blend";
            this.button_ApplyCurrentAnimState.UseVisualStyleBackColor = true;
            this.button_ApplyCurrentAnimState.Click += new System.EventHandler(this.button_ApplyCurrentAnimState_Click);
            // 
            // button_PlayKeyframeInterval
            // 
            this.button_PlayKeyframeInterval.Location = new System.Drawing.Point(781, 38);
            this.button_PlayKeyframeInterval.Name = "button_PlayKeyframeInterval";
            this.button_PlayKeyframeInterval.Size = new System.Drawing.Size(165, 24);
            this.button_PlayKeyframeInterval.TabIndex = 33;
            this.button_PlayKeyframeInterval.Text = "Play current keyframe interval";
            this.button_PlayKeyframeInterval.UseVisualStyleBackColor = true;
            this.button_PlayKeyframeInterval.Click += new System.EventHandler(this.button_PlayKeyframeInterval_Click);
            // 
            // button_AllKeyframeIntervals
            // 
            this.button_AllKeyframeIntervals.Location = new System.Drawing.Point(785, 68);
            this.button_AllKeyframeIntervals.Name = "button_AllKeyframeIntervals";
            this.button_AllKeyframeIntervals.Size = new System.Drawing.Size(151, 23);
            this.button_AllKeyframeIntervals.TabIndex = 34;
            this.button_AllKeyframeIntervals.Text = "Play all keyframes";
            this.button_AllKeyframeIntervals.UseVisualStyleBackColor = true;
            this.button_AllKeyframeIntervals.Click += new System.EventHandler(this.button_AllKeyframeIntervals_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 528);
            this.Controls.Add(this.button_AllKeyframeIntervals);
            this.Controls.Add(this.button_PlayKeyframeInterval);
            this.Controls.Add(this.button_ApplyCurrentAnimState);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_PreviousKeyframe);
            this.Controls.Add(this.button_NextKeyframe);
            this.Controls.Add(this.checkBox_breakpoints_on);
            this.Controls.Add(this.treeView_entity_info);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_zoom);
            this.Controls.Add(this.button_stop_colors);
            this.Controls.Add(this.button_resetpos);
            this.Controls.Add(this.button_resetzoom);
            this.Controls.Add(this.button_right);
            this.Controls.Add(this.button_left);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.pictureBox_main);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.trackBar1);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.PictureBox pictureBox_main;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_right;
        private System.Windows.Forms.Button button_resetzoom;
        private System.Windows.Forms.Button button_resetpos;
        private System.Windows.Forms.Button button_stop_colors;
        private System.Windows.Forms.Label label_zoom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_is_selected;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_mouse_coords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_camera_rotation;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_camera_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_entity_position;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_entity_rotation;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.TreeView treeView_entity_info;
        private System.Windows.Forms.CheckBox checkBox_breakpoints_on;
        private System.Windows.Forms.Button button_NextKeyframe;
        private System.Windows.Forms.Button button_PreviousKeyframe;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_ApplyCurrentAnimState;
        private System.Windows.Forms.Button button_PlayKeyframeInterval;
        private System.Windows.Forms.Button button_AllKeyframeIntervals;
    }
}

