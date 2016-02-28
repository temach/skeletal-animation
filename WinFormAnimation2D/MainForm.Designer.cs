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
            this.label1 = new System.Windows.Forms.Label();
            this.label_CurrentRotoAngle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(845, 238);
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
            this.pictureBox_main.Size = new System.Drawing.Size(747, 479);
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
            this.button_up.Location = new System.Drawing.Point(855, 105);
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
            this.button_down.Location = new System.Drawing.Point(855, 161);
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
            this.button_left.Location = new System.Drawing.Point(814, 132);
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
            this.button_right.Location = new System.Drawing.Point(894, 132);
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
            this.button_resetzoom.Location = new System.Drawing.Point(832, 56);
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
            this.button_resetpos.Location = new System.Drawing.Point(829, 27);
            this.button_resetpos.Name = "button_resetpos";
            this.button_resetpos.Size = new System.Drawing.Size(91, 23);
            this.button_resetpos.TabIndex = 18;
            this.button_resetpos.Text = "Reset Position";
            this.button_resetpos.UseVisualStyleBackColor = true;
            this.button_resetpos.Click += new System.EventHandler(this.button_resetpos_Click);
            // 
            // button_stop_colors
            // 
            this.button_stop_colors.Location = new System.Drawing.Point(845, 267);
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
            this.label_zoom.Location = new System.Drawing.Point(913, 61);
            this.label_zoom.Name = "label_zoom";
            this.label_zoom.Size = new System.Drawing.Size(22, 13);
            this.label_zoom.TabIndex = 17;
            this.label_zoom.Text = "1.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(765, 343);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Current rotation angle:";
            // 
            // label_CurrentRotoAngle
            // 
            this.label_CurrentRotoAngle.AutoSize = true;
            this.label_CurrentRotoAngle.Location = new System.Drawing.Point(879, 343);
            this.label_CurrentRotoAngle.Name = "label_CurrentRotoAngle";
            this.label_CurrentRotoAngle.Size = new System.Drawing.Size(25, 13);
            this.label_CurrentRotoAngle.TabIndex = 21;
            this.label_CurrentRotoAngle.Text = "___";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 528);
            this.Controls.Add(this.label_CurrentRotoAngle);
            this.Controls.Add(this.label1);
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
            this.Name = "MainForm";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_CurrentRotoAngle;
    }
}

