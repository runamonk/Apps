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
            this.EditFileArgs = new System.Windows.Forms.TextBox();
            this.PanelLeft = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Browse = new System.Windows.Forms.Button();
            this.EditAppFilePath = new System.Windows.Forms.TextBox();
            this.EditAppIcon = new System.Windows.Forms.PictureBox();
            this.EditAppName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.PanelBack.SuspendLayout();
            this.PanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditAppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelBack
            // 
            this.PanelBack.Controls.Add(this.EditFileArgs);
            this.PanelBack.Controls.Add(this.PanelLeft);
            this.PanelBack.Controls.Add(this.Browse);
            this.PanelBack.Controls.Add(this.EditAppFilePath);
            this.PanelBack.Controls.Add(this.EditAppIcon);
            this.PanelBack.Controls.Add(this.EditAppName);
            this.PanelBack.Controls.Add(this.ButtonCancel);
            this.PanelBack.Controls.Add(this.ButtonOK);
            this.PanelBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBack.Location = new System.Drawing.Point(0, 0);
            this.PanelBack.Name = "PanelBack";
            this.PanelBack.Size = new System.Drawing.Size(397, 186);
            this.PanelBack.TabIndex = 0;
            // 
            // EditFileArgs
            // 
            this.EditFileArgs.Location = new System.Drawing.Point(72, 93);
            this.EditFileArgs.Name = "EditFileArgs";
            this.EditFileArgs.Size = new System.Drawing.Size(294, 20);
            this.EditFileArgs.TabIndex = 4;
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
            // EditAppFilePath
            // 
            this.EditAppFilePath.Location = new System.Drawing.Point(72, 30);
            this.EditAppFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.EditAppFilePath.Multiline = true;
            this.EditAppFilePath.Name = "EditAppFilePath";
            this.EditAppFilePath.ReadOnly = true;
            this.EditAppFilePath.Size = new System.Drawing.Size(294, 60);
            this.EditAppFilePath.TabIndex = 2;
            // 
            // EditAppIcon
            // 
            this.EditAppIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppIcon.Location = new System.Drawing.Point(72, 117);
            this.EditAppIcon.Name = "EditAppIcon";
            this.EditAppIcon.Size = new System.Drawing.Size(64, 64);
            this.EditAppIcon.TabIndex = 8;
            this.EditAppIcon.TabStop = false;
            this.EditAppIcon.Click += new System.EventHandler(this.AppIcon_Click);
            // 
            // EditAppName
            // 
            this.EditAppName.Location = new System.Drawing.Point(72, 5);
            this.EditAppName.Name = "EditAppName";
            this.EditAppName.Size = new System.Drawing.Size(320, 20);
            this.EditAppName.TabIndex = 1;
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
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
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
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppProperties_FormClosing);
            this.PanelBack.ResumeLayout(false);
            this.PanelBack.PerformLayout();
            this.PanelLeft.ResumeLayout(false);
            this.PanelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditAppIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelBack;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Panel PanelLeft;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox EditAppName;
        private System.Windows.Forms.PictureBox EditAppIcon;
        private System.Windows.Forms.TextBox EditAppFilePath;
        private System.Windows.Forms.TextBox EditFileArgs;
    }
}