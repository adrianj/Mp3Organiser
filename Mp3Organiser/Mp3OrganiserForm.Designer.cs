namespace Mp3Organiser
{
    partial class Mp3OrganiserForm
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
			this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.srcBox = new System.Windows.Forms.TextBox();
			this.srcButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.destButton = new System.Windows.Forms.Button();
			this.destBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.formatButton = new System.Windows.Forms.Button();
			this.formatBox = new System.Windows.Forms.TextBox();
			this.progressBar = new DTALib.ProgressBarControl();
			this.autoCheck = new System.Windows.Forms.CheckBox();
			this.deleteFilesCheck = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.formatCompBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.preferredTypeCombo = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// srcBox
			// 
			this.srcBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.srcBox.Location = new System.Drawing.Point(108, 12);
			this.srcBox.Name = "srcBox";
			this.srcBox.Size = new System.Drawing.Size(226, 20);
			this.srcBox.TabIndex = 0;
			this.srcBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.srcButton_Click);
			// 
			// srcButton
			// 
			this.srcButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.srcButton.Location = new System.Drawing.Point(340, 9);
			this.srcButton.Name = "srcButton";
			this.srcButton.Size = new System.Drawing.Size(109, 23);
			this.srcButton.TabIndex = 1;
			this.srcButton.Text = "Change Source";
			this.srcButton.UseVisualStyleBackColor = true;
			this.srcButton.Click += new System.EventHandler(this.srcButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Source Folder";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Destination Folder";
			// 
			// destButton
			// 
			this.destButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.destButton.Location = new System.Drawing.Point(340, 38);
			this.destButton.Name = "destButton";
			this.destButton.Size = new System.Drawing.Size(109, 23);
			this.destButton.TabIndex = 4;
			this.destButton.Text = "Change Destination";
			this.destButton.UseVisualStyleBackColor = true;
			this.destButton.Click += new System.EventHandler(this.destButton_Click);
			// 
			// destBox
			// 
			this.destBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.destBox.Location = new System.Drawing.Point(108, 41);
			this.destBox.Name = "destBox";
			this.destBox.Size = new System.Drawing.Size(226, 20);
			this.destBox.TabIndex = 3;
			this.destBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.destButton_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Normal Format";
			// 
			// formatButton
			// 
			this.formatButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.formatButton.Location = new System.Drawing.Point(340, 67);
			this.formatButton.Name = "formatButton";
			this.formatButton.Size = new System.Drawing.Size(109, 23);
			this.formatButton.TabIndex = 7;
			this.formatButton.Text = "Change Format";
			this.formatButton.UseVisualStyleBackColor = true;
			this.formatButton.Click += new System.EventHandler(this.formatButton_Click);
			// 
			// formatBox
			// 
			this.formatBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.formatBox.Location = new System.Drawing.Point(108, 66);
			this.formatBox.Name = "formatBox";
			this.formatBox.Size = new System.Drawing.Size(226, 20);
			this.formatBox.TabIndex = 6;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.ButtonWidth = 116;
			this.progressBar.CanCancel = true;
			this.progressBar.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.progressBar.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.progressBar.EnableTextBox = true;
			this.progressBar.Location = new System.Drawing.Point(11, 195);
			this.progressBar.Maximum = 100;
			this.progressBar.Minimum = 0;
			this.progressBar.Name = "progressBar";
			this.progressBar.Result = null;
			this.progressBar.Size = new System.Drawing.Size(438, 153);
			this.progressBar.TabIndex = 9;
			this.progressBar.Value = 0;
			this.progressBar.ButtonClick += new System.EventHandler(this.progressBar_ButtonClick);
			this.progressBar.Load += new System.EventHandler(this.progressBar_Load);
			// 
			// autoCheck
			// 
			this.autoCheck.AutoSize = true;
			this.autoCheck.Checked = true;
			this.autoCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoCheck.Location = new System.Drawing.Point(15, 150);
			this.autoCheck.Name = "autoCheck";
			this.autoCheck.Size = new System.Drawing.Size(171, 17);
			this.autoCheck.TabIndex = 10;
			this.autoCheck.Text = "Automatically correct filenames";
			this.autoCheck.UseVisualStyleBackColor = true;
			this.autoCheck.CheckedChanged += new System.EventHandler(this.autoCheck_CheckedChanged);
			// 
			// deleteFilesCheck
			// 
			this.deleteFilesCheck.AutoSize = true;
			this.deleteFilesCheck.Checked = true;
			this.deleteFilesCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.deleteFilesCheck.Location = new System.Drawing.Point(15, 173);
			this.deleteFilesCheck.Name = "deleteFilesCheck";
			this.deleteFilesCheck.Size = new System.Drawing.Size(244, 17);
			this.deleteFilesCheck.TabIndex = 11;
			this.deleteFilesCheck.Text = "Delete supported file types not in source folder";
			this.deleteFilesCheck.UseVisualStyleBackColor = true;
			this.deleteFilesCheck.CheckedChanged += new System.EventHandler(this.deleteFilesCheck_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 95);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Compilation Format";
			// 
			// formatCompBox
			// 
			this.formatCompBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.formatCompBox.Location = new System.Drawing.Point(108, 92);
			this.formatCompBox.Name = "formatCompBox";
			this.formatCompBox.Size = new System.Drawing.Size(226, 20);
			this.formatCompBox.TabIndex = 12;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 119);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(77, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Preferred Type";
			// 
			// preferredTypeCombo
			// 
			this.preferredTypeCombo.FormattingEnabled = true;
			this.preferredTypeCombo.Location = new System.Drawing.Point(108, 116);
			this.preferredTypeCombo.Name = "preferredTypeCombo";
			this.preferredTypeCombo.Size = new System.Drawing.Size(121, 21);
			this.preferredTypeCombo.TabIndex = 15;
			// 
			// Mp3OrganiserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(461, 360);
			this.Controls.Add(this.preferredTypeCombo);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.formatCompBox);
			this.Controls.Add(this.deleteFilesCheck);
			this.Controls.Add(this.autoCheck);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.formatButton);
			this.Controls.Add(this.formatBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.destButton);
			this.Controls.Add(this.destBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.srcButton);
			this.Controls.Add(this.srcBox);
			this.Name = "Mp3OrganiserForm";
			this.Text = "MP3 Organiser";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.TextBox srcBox;
        private System.Windows.Forms.Button srcButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button destButton;
        private System.Windows.Forms.TextBox destBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button formatButton;
        private System.Windows.Forms.TextBox formatBox;
        private DTALib.ProgressBarControl progressBar;
        private System.Windows.Forms.CheckBox autoCheck;
        private System.Windows.Forms.CheckBox deleteFilesCheck;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox formatCompBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox preferredTypeCombo;
    }
}

