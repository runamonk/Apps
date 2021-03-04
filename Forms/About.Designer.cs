namespace Apps
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDesc = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.linkEmail = new System.Windows.Forms.LinkLabel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblDesc);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 150);
            this.panel1.TabIndex = 0;
            // 
            // lblDesc
            // 
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.Location = new System.Drawing.Point(0, 45);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(273, 73);
            this.lblDesc.TabIndex = 6;
            this.lblDesc.Text = "A simple application launcher.";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDesc.UseCompatibleTextRendering = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.linkEmail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 118);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 30);
            this.panel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "©2021";
            // 
            // linkEmail
            // 
            this.linkEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkEmail.Location = new System.Drawing.Point(0, 0);
            this.linkEmail.Name = "linkEmail";
            this.linkEmail.Size = new System.Drawing.Size(273, 30);
            this.linkEmail.TabIndex = 7;
            this.linkEmail.TabStop = true;
            this.linkEmail.Text = "runamonk@onlyzuul.org";
            this.linkEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVersion.Location = new System.Drawing.Point(0, 22);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(273, 23);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "[VERSION]";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            this.lblName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(273, 22);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "[NAME]";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 150);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "                                                         ";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.About_Deactivate);
            this.Load += new System.EventHandler(this.About_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.About_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkEmail;
        private System.Windows.Forms.Label lblDesc;
    }
}