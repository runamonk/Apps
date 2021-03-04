
namespace Apps.Forms
{
    partial class FolderLink
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
            this.PanelMain = new System.Windows.Forms.Panel();
            this.PanelRight = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.BrowseWF = new System.Windows.Forms.Button();
            this.EditFolderPath = new System.Windows.Forms.TextBox();
            this.EditFolderName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.PanelMain.SuspendLayout();
            this.PanelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelMain
            // 
            this.PanelMain.Controls.Add(this.PanelRight);
            this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMain.Location = new System.Drawing.Point(0, 0);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(378, 111);
            this.PanelMain.TabIndex = 0;
            // 
            // PanelRight
            // 
            this.PanelRight.Controls.Add(this.label1);
            this.PanelRight.Controls.Add(this.NameLabel);
            this.PanelRight.Controls.Add(this.BrowseWF);
            this.PanelRight.Controls.Add(this.EditFolderPath);
            this.PanelRight.Controls.Add(this.EditFolderName);
            this.PanelRight.Controls.Add(this.ButtonCancel);
            this.PanelRight.Controls.Add(this.ButtonOK);
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelRight.Location = new System.Drawing.Point(0, 0);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(378, 111);
            this.PanelRight.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Path";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(10, 9);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(35, 13);
            this.NameLabel.TabIndex = 23;
            this.NameLabel.Text = "Name";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BrowseWF
            // 
            this.BrowseWF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseWF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseWF.Location = new System.Drawing.Point(346, 32);
            this.BrowseWF.Margin = new System.Windows.Forms.Padding(0);
            this.BrowseWF.Name = "BrowseWF";
            this.BrowseWF.Size = new System.Drawing.Size(24, 20);
            this.BrowseWF.TabIndex = 22;
            this.BrowseWF.Text = "...";
            this.BrowseWF.UseCompatibleTextRendering = true;
            this.BrowseWF.UseVisualStyleBackColor = true;
            this.BrowseWF.Click += new System.EventHandler(this.BrowseWF_Click);
            // 
            // EditFolderPath
            // 
            this.EditFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditFolderPath.Location = new System.Drawing.Point(47, 32);
            this.EditFolderPath.Multiline = true;
            this.EditFolderPath.Name = "EditFolderPath";
            this.EditFolderPath.Size = new System.Drawing.Size(294, 40);
            this.EditFolderPath.TabIndex = 5;
            // 
            // EditFolderName
            // 
            this.EditFolderName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditFolderName.Location = new System.Drawing.Point(47, 7);
            this.EditFolderName.Name = "EditFolderName";
            this.EditFolderName.Size = new System.Drawing.Size(294, 20);
            this.EditFolderName.TabIndex = 0;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonCancel.Location = new System.Drawing.Point(200, 80);
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
            this.ButtonOK.Location = new System.Drawing.Point(119, 80);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 7;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // FolderLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 111);
            this.Controls.Add(this.PanelMain);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderLink";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FolderLink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FolderLink_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.PanelMain.ResumeLayout(false);
            this.PanelRight.ResumeLayout(false);
            this.PanelRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelMain;
        private System.Windows.Forms.Panel PanelRight;
        private System.Windows.Forms.Button BrowseWF;
        public System.Windows.Forms.TextBox EditFolderPath;
        private System.Windows.Forms.TextBox EditFolderName;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label NameLabel;
    }
}