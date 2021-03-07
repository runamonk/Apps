
namespace Apps.Forms
{
    partial class Confirm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.ButtonTwo = new System.Windows.Forms.Button();
            this.ButtonOne = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ConfirmText = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ButtonTwo);
            this.panel2.Controls.Add(this.ButtonOne);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(409, 43);
            this.panel2.TabIndex = 1;
            // 
            // ButtonTwo
            // 
            this.ButtonTwo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonTwo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonTwo.Location = new System.Drawing.Point(207, 9);
            this.ButtonTwo.Name = "ButtonTwo";
            this.ButtonTwo.Size = new System.Drawing.Size(75, 23);
            this.ButtonTwo.TabIndex = 5;
            this.ButtonTwo.Text = "Cancel";
            this.ButtonTwo.UseVisualStyleBackColor = true;
            // 
            // ButtonOne
            // 
            this.ButtonOne.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOne.Location = new System.Drawing.Point(126, 9);
            this.ButtonOne.Name = "ButtonOne";
            this.ButtonOne.Size = new System.Drawing.Size(75, 23);
            this.ButtonOne.TabIndex = 4;
            this.ButtonOne.Text = "OK";
            this.ButtonOne.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.ConfirmText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 37);
            this.panel1.TabIndex = 2;
            // 
            // ConfirmText
            // 
            this.ConfirmText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmText.Location = new System.Drawing.Point(0, 0);
            this.ConfirmText.Name = "ConfirmText";
            this.ConfirmText.Size = new System.Drawing.Size(409, 37);
            this.ConfirmText.TabIndex = 0;
            this.ConfirmText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ConfirmText.UseCompatibleTextRendering = true;
            this.ConfirmText.UseMnemonic = false;
            // 
            // Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 80);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Confirm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Confirm_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ButtonTwo;
        private System.Windows.Forms.Button ButtonOne;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ConfirmText;
    }
}