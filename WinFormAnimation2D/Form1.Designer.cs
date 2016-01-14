namespace WinFormAnimation2D
{
    partial class MyForm
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
            this.trackBar_zoom = new System.Windows.Forms.TrackBar();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.trackBar5 = new System.Windows.Forms.TrackBar();
            this.trackBar6 = new System.Windows.Forms.TrackBar();
            this.button_up = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_right = new System.Windows.Forms.Button();
            this.textBox_stepsize = new System.Windows.Forms.TextBox();
            this.label_stepsize = new System.Windows.Forms.Label();
            this.button_zoom = new System.Windows.Forms.Button();
            this.button_resetzoom = new System.Windows.Forms.Button();
            this.label_zoom = new System.Windows.Forms.Label();
            this.button_resetpos = new System.Windows.Forms.Button();
            this.button_stop_colors = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar6)).BeginInit();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(12, 2);
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
            // 
            // trackBar_zoom
            // 
            this.trackBar_zoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trackBar_zoom.Location = new System.Drawing.Point(842, 64);
            this.trackBar_zoom.Minimum = -10;
            this.trackBar_zoom.Name = "trackBar_zoom";
            this.trackBar_zoom.Size = new System.Drawing.Size(104, 45);
            this.trackBar_zoom.TabIndex = 2;
            this.trackBar_zoom.ValueChanged += new System.EventHandler(this.trackBar_zoom_ValueChanged);
            // 
            // trackBar4
            // 
            this.trackBar4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trackBar4.Location = new System.Drawing.Point(842, 296);
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(104, 45);
            this.trackBar4.TabIndex = 5;
            // 
            // trackBar5
            // 
            this.trackBar5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trackBar5.Location = new System.Drawing.Point(842, 375);
            this.trackBar5.Name = "trackBar5";
            this.trackBar5.Size = new System.Drawing.Size(104, 45);
            this.trackBar5.TabIndex = 6;
            // 
            // trackBar6
            // 
            this.trackBar6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trackBar6.Location = new System.Drawing.Point(842, 451);
            this.trackBar6.Name = "trackBar6";
            this.trackBar6.Size = new System.Drawing.Size(104, 45);
            this.trackBar6.TabIndex = 7;
            // 
            // button_up
            // 
            this.button_up.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_up.Location = new System.Drawing.Point(855, 115);
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
            this.button_down.Location = new System.Drawing.Point(855, 171);
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
            this.button_left.Location = new System.Drawing.Point(814, 142);
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
            this.button_right.Location = new System.Drawing.Point(894, 142);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(52, 23);
            this.button_right.TabIndex = 12;
            this.button_right.Text = "right";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Click += new System.EventHandler(this.button_right_Click);
            // 
            // textBox_stepsize
            // 
            this.textBox_stepsize.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_stepsize.Location = new System.Drawing.Point(829, 225);
            this.textBox_stepsize.Name = "textBox_stepsize";
            this.textBox_stepsize.Size = new System.Drawing.Size(117, 20);
            this.textBox_stepsize.TabIndex = 13;
            // 
            // label_stepsize
            // 
            this.label_stepsize.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_stepsize.AutoSize = true;
            this.label_stepsize.Location = new System.Drawing.Point(857, 209);
            this.label_stepsize.Name = "label_stepsize";
            this.label_stepsize.Size = new System.Drawing.Size(50, 13);
            this.label_stepsize.TabIndex = 14;
            this.label_stepsize.Text = "Step size";
            // 
            // button_zoom
            // 
            this.button_zoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_zoom.Location = new System.Drawing.Point(856, 35);
            this.button_zoom.Name = "button_zoom";
            this.button_zoom.Size = new System.Drawing.Size(90, 23);
            this.button_zoom.TabIndex = 15;
            this.button_zoom.Text = "Change zoom";
            this.button_zoom.UseVisualStyleBackColor = true;
            this.button_zoom.Click += new System.EventHandler(this.button_zoom_Click);
            // 
            // button_resetzoom
            // 
            this.button_resetzoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_resetzoom.Location = new System.Drawing.Point(775, 35);
            this.button_resetzoom.Name = "button_resetzoom";
            this.button_resetzoom.Size = new System.Drawing.Size(75, 23);
            this.button_resetzoom.TabIndex = 16;
            this.button_resetzoom.Text = "Reset zoom";
            this.button_resetzoom.UseVisualStyleBackColor = true;
            this.button_resetzoom.Click += new System.EventHandler(this.button_resetzoom_Click);
            // 
            // label_zoom
            // 
            this.label_zoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_zoom.AutoSize = true;
            this.label_zoom.Location = new System.Drawing.Point(798, 65);
            this.label_zoom.Name = "label_zoom";
            this.label_zoom.Size = new System.Drawing.Size(22, 13);
            this.label_zoom.TabIndex = 17;
            this.label_zoom.Text = "1.0";
            // 
            // button_resetpos
            // 
            this.button_resetpos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_resetpos.Location = new System.Drawing.Point(775, 2);
            this.button_resetpos.Name = "button_resetpos";
            this.button_resetpos.Size = new System.Drawing.Size(91, 23);
            this.button_resetpos.TabIndex = 18;
            this.button_resetpos.Text = "Reset Position";
            this.button_resetpos.UseVisualStyleBackColor = true;
            this.button_resetpos.Click += new System.EventHandler(this.button_resetpos_Click);
            // 
            // button_stop_colors
            // 
            this.button_stop_colors.Location = new System.Drawing.Point(93, 2);
            this.button_stop_colors.Name = "button_stop_colors";
            this.button_stop_colors.Size = new System.Drawing.Size(75, 23);
            this.button_stop_colors.TabIndex = 19;
            this.button_stop_colors.Text = "Stop colors";
            this.button_stop_colors.UseVisualStyleBackColor = true;
            this.button_stop_colors.Click += new System.EventHandler(this.button_stop_colors_Click);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 528);
            this.Controls.Add(this.button_stop_colors);
            this.Controls.Add(this.button_resetpos);
            this.Controls.Add(this.label_zoom);
            this.Controls.Add(this.button_resetzoom);
            this.Controls.Add(this.button_zoom);
            this.Controls.Add(this.label_stepsize);
            this.Controls.Add(this.textBox_stepsize);
            this.Controls.Add(this.button_right);
            this.Controls.Add(this.button_left);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.trackBar6);
            this.Controls.Add(this.trackBar5);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.trackBar_zoom);
            this.Controls.Add(this.pictureBox_main);
            this.Controls.Add(this.button_start);
            this.Name = "MyForm";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.PictureBox pictureBox_main;
        private System.Windows.Forms.TrackBar trackBar_zoom;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.TrackBar trackBar5;
        private System.Windows.Forms.TrackBar trackBar6;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_right;
        private System.Windows.Forms.TextBox textBox_stepsize;
        private System.Windows.Forms.Label label_stepsize;
        private System.Windows.Forms.Button button_zoom;
        private System.Windows.Forms.Button button_resetzoom;
        private System.Windows.Forms.Label label_zoom;
        private System.Windows.Forms.Button button_resetpos;
        private System.Windows.Forms.Button button_stop_colors;
    }
}

