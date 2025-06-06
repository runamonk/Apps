﻿namespace Apps
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.notifyApps = new System.Windows.Forms.NotifyIcon(this.components);
            this.pTop = new System.Windows.Forms.Panel();
            this.pMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // notifyApps
            // 
            this.notifyApps.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyApps.Icon")));
            this.notifyApps.Text = "zuul Apps";
            this.notifyApps.Visible = true;
            this.notifyApps.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyApps_MouseDoubleClick);
            // 
            // pTop
            // 
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Margin = new System.Windows.Forms.Padding(0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(198, 25);
            this.pTop.TabIndex = 3;
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PTop_MouseDown);
            // 
            // pMain
            // 
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 25);
            this.pMain.Margin = new System.Windows.Forms.Padding(0);
            this.pMain.Name = "pMain";
            this.pMain.Padding = new System.Windows.Forms.Padding(1);
            this.pMain.Size = new System.Drawing.Size(198, 63);
            this.pMain.TabIndex = 4;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(198, 88);
            this.ControlBox = false;
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(9000, 9000);
            this.MinimumSize = new System.Drawing.Size(200, 90);
            this.Name = "Main";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Apps";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.Main_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResizeEnd += new System.EventHandler(this.Main_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyApps;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pMain;
    }
}

