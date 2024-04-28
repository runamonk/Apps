namespace Apps.Forms
{
    partial class Programs
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("loading programs, please wait...");
            this.panelButtons = new System.Windows.Forms.Panel();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.searchLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.search = new System.Windows.Forms.TextBox();
            this.panelPrograms = new System.Windows.Forms.Panel();
            this.listPrograms = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelPrograms.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.SystemColors.Control;
            this.panelButtons.Controls.Add(this.ButtonOK);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 362);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(393, 40);
            this.panelButtons.TabIndex = 8;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOK.Location = new System.Drawing.Point(309, 8);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 4;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.searchLabel);
            this.panelSearch.Controls.Add(this.panel1);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(393, 29);
            this.panelSearch.TabIndex = 9;
            // 
            // searchLabel
            // 
            this.searchLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchLabel.Location = new System.Drawing.Point(0, 0);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(393, 29);
            this.searchLabel.TabIndex = 4;
            this.searchLabel.Text = "search";
            this.searchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.searchLabel.Click += new System.EventHandler(this.searchLabel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panel1.Size = new System.Drawing.Size(393, 29);
            this.panel1.TabIndex = 5;
            // 
            // search
            // 
            this.search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.search.Location = new System.Drawing.Point(0, 5);
            this.search.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.search.Multiline = true;
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(393, 24);
            this.search.TabIndex = 6;
            this.search.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            this.search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.search_KeyDown);
            // 
            // panelPrograms
            // 
            this.panelPrograms.Controls.Add(this.listPrograms);
            this.panelPrograms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrograms.Location = new System.Drawing.Point(0, 29);
            this.panelPrograms.Name = "panelPrograms";
            this.panelPrograms.Size = new System.Drawing.Size(393, 333);
            this.panelPrograms.TabIndex = 10;
            // 
            // listPrograms
            // 
            this.listPrograms.BackColor = System.Drawing.SystemColors.Control;
            this.listPrograms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listPrograms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPrograms.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listPrograms.FullRowSelect = true;
            this.listPrograms.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listPrograms.HideSelection = false;
            this.listPrograms.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listPrograms.Location = new System.Drawing.Point(0, 0);
            this.listPrograms.Name = "listPrograms";
            this.listPrograms.ShowGroups = false;
            this.listPrograms.Size = new System.Drawing.Size(393, 333);
            this.listPrograms.SmallImageList = this.imageList;
            this.listPrograms.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listPrograms.TabIndex = 0;
            this.toolTip.SetToolTip(this.listPrograms, "Click and select programs, drag and drop onto Apps.");
            this.listPrograms.UseCompatibleStateImageBehavior = false;
            this.listPrograms.View = System.Windows.Forms.View.Details;
            this.listPrograms.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPrograms_MouseDown);
            this.listPrograms.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listPrograms_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            this.columnHeader1.Width = 25;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(22, 22);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Programs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 402);
            this.Controls.Add(this.panelPrograms);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelButtons);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(1000, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 400);
            this.Name = "Programs";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Programs";
            this.TopMost = true;
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPrograms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Panel panelPrograms;
        private System.Windows.Forms.ListView listPrograms;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolTip toolTip;
    }
}