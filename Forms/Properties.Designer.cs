namespace Apps.Forms
{
    partial class Properties
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
            this.PanelRight = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ArgsLabel = new System.Windows.Forms.Label();
            this.PathLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.BrowseWF = new System.Windows.Forms.Button();
            this.EditWorkingFolder = new System.Windows.Forms.TextBox();
            this.ButtonParseShortcut = new System.Windows.Forms.Button();
            this.EditFileArgs = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.EditAppFilePath = new System.Windows.Forms.TextBox();
            this.EditAppIcon = new System.Windows.Forms.PictureBox();
            this.EditAppName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PanelBack.SuspendLayout();
            this.PanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditAppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelBack
            // 
            this.PanelBack.Controls.Add(this.PanelRight);
            this.PanelBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBack.Location = new System.Drawing.Point(0, 0);
            this.PanelBack.Name = "PanelBack";
            this.PanelBack.Size = new System.Drawing.Size(419, 175);
            this.PanelBack.TabIndex = 0;
            // 
            // PanelRight
            // 
            this.PanelRight.Controls.Add(this.label1);
            this.PanelRight.Controls.Add(this.ArgsLabel);
            this.PanelRight.Controls.Add(this.PathLabel);
            this.PanelRight.Controls.Add(this.NameLabel);
            this.PanelRight.Controls.Add(this.BrowseWF);
            this.PanelRight.Controls.Add(this.EditWorkingFolder);
            this.PanelRight.Controls.Add(this.ButtonParseShortcut);
            this.PanelRight.Controls.Add(this.EditFileArgs);
            this.PanelRight.Controls.Add(this.Browse);
            this.PanelRight.Controls.Add(this.EditAppFilePath);
            this.PanelRight.Controls.Add(this.EditAppIcon);
            this.PanelRight.Controls.Add(this.EditAppName);
            this.PanelRight.Controls.Add(this.ButtonCancel);
            this.PanelRight.Controls.Add(this.ButtonOK);
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelRight.Location = new System.Drawing.Point(0, 0);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(419, 175);
            this.PanelRight.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Working Folder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ArgsLabel
            // 
            this.ArgsLabel.AutoSize = true;
            this.ArgsLabel.Location = new System.Drawing.Point(29, 113);
            this.ArgsLabel.Name = "ArgsLabel";
            this.ArgsLabel.Size = new System.Drawing.Size(57, 13);
            this.ArgsLabel.TabIndex = 25;
            this.ArgsLabel.Text = "Arguments";
            this.ArgsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(57, 36);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(29, 13);
            this.PathLabel.TabIndex = 23;
            this.PathLabel.Text = "Path";
            this.PathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(51, 10);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 22;
            this.NameLabel.Text = "Name";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BrowseWF
            // 
            this.BrowseWF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseWF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseWF.Location = new System.Drawing.Point(386, 84);
            this.BrowseWF.Margin = new System.Windows.Forms.Padding(0);
            this.BrowseWF.Name = "BrowseWF";
            this.BrowseWF.Size = new System.Drawing.Size(24, 20);
            this.BrowseWF.TabIndex = 5;
            this.BrowseWF.Text = "...";
            this.BrowseWF.UseCompatibleTextRendering = true;
            this.BrowseWF.UseVisualStyleBackColor = true;
            this.BrowseWF.Click += new System.EventHandler(this.BrowseWF_Click);
            // 
            // EditWorkingFolder
            // 
            this.EditWorkingFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditWorkingFolder.Location = new System.Drawing.Point(87, 84);
            this.EditWorkingFolder.Name = "EditWorkingFolder";
            this.EditWorkingFolder.Size = new System.Drawing.Size(294, 20);
            this.EditWorkingFolder.TabIndex = 4;
            // 
            // ButtonParseShortcut
            // 
            this.ButtonParseShortcut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonParseShortcut.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonParseShortcut.Location = new System.Drawing.Point(386, 58);
            this.ButtonParseShortcut.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonParseShortcut.Name = "ButtonParseShortcut";
            this.ButtonParseShortcut.Size = new System.Drawing.Size(24, 20);
            this.ButtonParseShortcut.TabIndex = 3;
            this.ButtonParseShortcut.Text = "✔";
            this.ButtonParseShortcut.UseCompatibleTextRendering = true;
            this.ButtonParseShortcut.UseVisualStyleBackColor = true;
            this.ButtonParseShortcut.Click += new System.EventHandler(this.ButtonParseShortcut_Click);
            // 
            // EditFileArgs
            // 
            this.EditFileArgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditFileArgs.Location = new System.Drawing.Point(87, 110);
            this.EditFileArgs.Name = "EditFileArgs";
            this.EditFileArgs.Size = new System.Drawing.Size(294, 20);
            this.EditFileArgs.TabIndex = 6;
            // 
            // Browse
            // 
            this.Browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Browse.Location = new System.Drawing.Point(386, 33);
            this.Browse.Margin = new System.Windows.Forms.Padding(0);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(24, 20);
            this.Browse.TabIndex = 2;
            this.Browse.Text = "...";
            this.Browse.UseCompatibleTextRendering = true;
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // EditAppFilePath
            // 
            this.EditAppFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppFilePath.Location = new System.Drawing.Point(87, 33);
            this.EditAppFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.EditAppFilePath.Multiline = true;
            this.EditAppFilePath.Name = "EditAppFilePath";
            this.EditAppFilePath.ReadOnly = true;
            this.EditAppFilePath.Size = new System.Drawing.Size(294, 45);
            this.EditAppFilePath.TabIndex = 1;
            this.EditAppFilePath.TabStop = false;
            this.EditAppFilePath.TextChanged += new System.EventHandler(this.EditAppFilePath_TextChanged);
            // 
            // EditAppIcon
            // 
            this.EditAppIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppIcon.Location = new System.Drawing.Point(7, 7);
            this.EditAppIcon.Name = "EditAppIcon";
            this.EditAppIcon.Size = new System.Drawing.Size(32, 32);
            this.EditAppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.EditAppIcon.TabIndex = 21;
            this.EditAppIcon.TabStop = false;
            this.toolTip.SetToolTip(this.EditAppIcon, "Click to override the icon with an icon from another file.");
            this.EditAppIcon.Click += new System.EventHandler(this.AppIcon_Click);
            // 
            // EditAppName
            // 
            this.EditAppName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditAppName.Location = new System.Drawing.Point(87, 7);
            this.EditAppName.Name = "EditAppName";
            this.EditAppName.Size = new System.Drawing.Size(323, 20);
            this.EditAppName.TabIndex = 0;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonCancel.Location = new System.Drawing.Point(212, 144);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 8;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOK.Location = new System.Drawing.Point(131, 144);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 7;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // Properties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 175);
            this.Controls.Add(this.PanelBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Properties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppProperties_FormClosing);
            this.Load += new System.EventHandler(this.Properties_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AppProperties_KeyDown);
            this.PanelBack.ResumeLayout(false);
            this.PanelRight.ResumeLayout(false);
            this.PanelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditAppIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelBack;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel PanelRight;
        private System.Windows.Forms.Button ButtonParseShortcut;
        private System.Windows.Forms.TextBox EditFileArgs;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox EditAppFilePath;
        private System.Windows.Forms.PictureBox EditAppIcon;
        private System.Windows.Forms.TextBox EditAppName;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button BrowseWF;
        public System.Windows.Forms.TextBox EditWorkingFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ArgsLabel;
        private System.Windows.Forms.Label PathLabel;
        private System.Windows.Forms.Label NameLabel;
    }
}