namespace Apps.Forms
{
    partial class AppProperties
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
            this.PanelBack = new System.Windows.Forms.Panel();
            this.PanelLeft = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Browse = new System.Windows.Forms.Button();
            this.AppFilePath = new System.Windows.Forms.TextBox();
            this.AppIcon = new System.Windows.Forms.PictureBox();
            this.AppName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.FileArgs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PanelBack.SuspendLayout();
            this.PanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelBack
            // 
            this.PanelBack.Controls.Add(this.FileArgs);
            this.PanelBack.Controls.Add(this.PanelLeft);
            this.PanelBack.Controls.Add(this.Browse);
            this.PanelBack.Controls.Add(this.AppFilePath);
            this.PanelBack.Controls.Add(this.AppIcon);
            this.PanelBack.Controls.Add(this.AppName);
            this.PanelBack.Controls.Add(this.ButtonCancel);
            this.PanelBack.Controls.Add(this.ButtonOK);
            this.PanelBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBack.Location = new System.Drawing.Point(0, 0);
            this.PanelBack.Name = "PanelBack";
            this.PanelBack.Size = new System.Drawing.Size(397, 186);
            this.PanelBack.TabIndex = 0;
            // 
            // PanelLeft
            // 
            this.PanelLeft.Controls.Add(this.label4);
            this.PanelLeft.Controls.Add(this.label3);
            this.PanelLeft.Controls.Add(this.label2);
            this.PanelLeft.Controls.Add(this.label1);
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelLeft.Location = new System.Drawing.Point(0, 0);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(68, 186);
            this.PanelLeft.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Icon";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Path";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(368, 29);
            this.Browse.Margin = new System.Windows.Forms.Padding(0);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(24, 24);
            this.Browse.TabIndex = 3;
            this.Browse.Text = "...";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // AppFilePath
            // 
            this.AppFilePath.Location = new System.Drawing.Point(72, 30);
            this.AppFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.AppFilePath.Multiline = true;
            this.AppFilePath.Name = "AppFilePath";
            this.AppFilePath.ReadOnly = true;
            this.AppFilePath.Size = new System.Drawing.Size(294, 60);
            this.AppFilePath.TabIndex = 2;
            // 
            // AppIcon
            // 
            this.AppIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AppIcon.Location = new System.Drawing.Point(72, 117);
            this.AppIcon.Name = "AppIcon";
            this.AppIcon.Size = new System.Drawing.Size(64, 64);
            this.AppIcon.TabIndex = 8;
            this.AppIcon.TabStop = false;
            this.AppIcon.Click += new System.EventHandler(this.AppIcon_Click);
            // 
            // AppName
            // 
            this.AppName.Location = new System.Drawing.Point(72, 5);
            this.AppName.Name = "AppName";
            this.AppName.Size = new System.Drawing.Size(320, 20);
            this.AppName.TabIndex = 1;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(317, 158);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 6;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(236, 158);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 5;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // FileArgs
            // 
            this.FileArgs.Location = new System.Drawing.Point(72, 93);
            this.FileArgs.Name = "FileArgs";
            this.FileArgs.Size = new System.Drawing.Size(294, 20);
            this.FileArgs.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Arguments";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AppProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 186);
            this.Controls.Add(this.PanelBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AppProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.PanelBack.ResumeLayout(false);
            this.PanelBack.PerformLayout();
            this.PanelLeft.ResumeLayout(false);
            this.PanelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelBack;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        public System.Windows.Forms.TextBox AppName;
        public System.Windows.Forms.PictureBox AppIcon;
        private System.Windows.Forms.Button Browse;
        public System.Windows.Forms.TextBox AppFilePath;
        private System.Windows.Forms.Panel PanelLeft;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox FileArgs;
        private System.Windows.Forms.Label label4;
    }
}