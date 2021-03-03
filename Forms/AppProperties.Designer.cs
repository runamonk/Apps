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
            this.components = new System.ComponentModel.Container();
            this.PanelBack = new System.Windows.Forms.Panel();
            this.ButtonParseShortcut = new System.Windows.Forms.Button();
            this.EditFileArgs = new System.Windows.Forms.TextBox();
            this.PanelLeft = new System.Windows.Forms.Panel();
            this.ArgsLabel = new System.Windows.Forms.Label();
            this.IconLabel = new System.Windows.Forms.Label();
            this.PathLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.Browse = new System.Windows.Forms.Button();
            this.EditAppFilePath = new System.Windows.Forms.TextBox();
            this.EditAppIcon = new System.Windows.Forms.PictureBox();
            this.EditAppName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PanelBack.SuspendLayout();
            this.PanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditAppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelBack
            // 
            this.PanelBack.Controls.Add(this.ButtonParseShortcut);
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
            // ButtonParseShortcut
            // 
            this.ButtonParseShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonParseShortcut.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonParseShortcut.Location = new System.Drawing.Point(368, 58);
            this.ButtonParseShortcut.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonParseShortcut.Name = "ButtonParseShortcut";
            this.ButtonParseShortcut.Size = new System.Drawing.Size(24, 24);
            this.ButtonParseShortcut.TabIndex = 13;
            this.ButtonParseShortcut.Text = "✔";
            this.ButtonParseShortcut.UseCompatibleTextRendering = true;
            this.ButtonParseShortcut.UseVisualStyleBackColor = true;
            this.ButtonParseShortcut.Click += new System.EventHandler(this.ButtonParseShortcut_Click);
            // 
            // EditFileArgs
            // 
            this.EditFileArgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditFileArgs.Location = new System.Drawing.Point(72, 93);
            this.EditFileArgs.Name = "EditFileArgs";
            this.EditFileArgs.Size = new System.Drawing.Size(294, 20);
            this.EditFileArgs.TabIndex = 4;
            // 
            // PanelLeft
            // 
            this.PanelLeft.Controls.Add(this.ArgsLabel);
            this.PanelLeft.Controls.Add(this.IconLabel);
            this.PanelLeft.Controls.Add(this.PathLabel);
            this.PanelLeft.Controls.Add(this.NameLabel);
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelLeft.Location = new System.Drawing.Point(0, 0);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(68, 186);
            this.PanelLeft.TabIndex = 12;
            // 
            // ArgsLabel
            // 
            this.ArgsLabel.AutoSize = true;
            this.ArgsLabel.Location = new System.Drawing.Point(9, 95);
            this.ArgsLabel.Name = "ArgsLabel";
            this.ArgsLabel.Size = new System.Drawing.Size(57, 13);
            this.ArgsLabel.TabIndex = 15;
            this.ArgsLabel.Text = "Arguments";
            this.ArgsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IconLabel
            // 
            this.IconLabel.AutoSize = true;
            this.IconLabel.Location = new System.Drawing.Point(38, 120);
            this.IconLabel.Name = "IconLabel";
            this.IconLabel.Size = new System.Drawing.Size(28, 13);
            this.IconLabel.TabIndex = 14;
            this.IconLabel.Text = "Icon";
            this.IconLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(37, 33);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(29, 13);
            this.PathLabel.TabIndex = 13;
            this.PathLabel.Text = "Path";
            this.PathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(31, 8);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 12;
            this.NameLabel.Text = "Name";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Browse
            // 
            this.Browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Browse.Location = new System.Drawing.Point(368, 29);
            this.Browse.Margin = new System.Windows.Forms.Padding(0);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(24, 24);
            this.Browse.TabIndex = 3;
            this.Browse.Text = "...";
            this.Browse.UseCompatibleTextRendering = true;
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // EditAppFilePath
            // 
            this.EditAppFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppFilePath.Location = new System.Drawing.Point(72, 29);
            this.EditAppFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.EditAppFilePath.Multiline = true;
            this.EditAppFilePath.Name = "EditAppFilePath";
            this.EditAppFilePath.ReadOnly = true;
            this.EditAppFilePath.Size = new System.Drawing.Size(294, 60);
            this.EditAppFilePath.TabIndex = 2;
            this.EditAppFilePath.TextChanged += new System.EventHandler(this.EditAppFilePath_TextChanged);
            // 
            // EditAppIcon
            // 
            this.EditAppIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppIcon.Location = new System.Drawing.Point(72, 117);
            this.EditAppIcon.Name = "EditAppIcon";
            this.EditAppIcon.Size = new System.Drawing.Size(64, 64);
            this.EditAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.EditAppIcon.TabIndex = 8;
            this.EditAppIcon.TabStop = false;
            this.EditAppIcon.Click += new System.EventHandler(this.AppIcon_Click);
            // 
            // EditAppName
            // 
            this.EditAppName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppName.Location = new System.Drawing.Point(72, 5);
            this.EditAppName.Name = "EditAppName";
            this.EditAppName.Size = new System.Drawing.Size(320, 20);
            this.EditAppName.TabIndex = 1;
            this.EditAppName.TextChanged += new System.EventHandler(this.EditAppName_TextChanged);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
        private System.Windows.Forms.Label IconLabel;
        private System.Windows.Forms.Label PathLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label ArgsLabel;
        private System.Windows.Forms.TextBox EditAppName;
        private System.Windows.Forms.PictureBox EditAppIcon;
        private System.Windows.Forms.TextBox EditAppFilePath;
        private System.Windows.Forms.TextBox EditFileArgs;
        private System.Windows.Forms.Button ButtonParseShortcut;
        private System.Windows.Forms.ToolTip toolTip;
    }
}