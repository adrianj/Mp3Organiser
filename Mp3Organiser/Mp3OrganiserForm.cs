﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mp3Organiser
{
    public partial class Mp3OrganiserForm : Form
    {
        private Mp3Organiser mOrganiser = new Mp3Organiser();

		public string SourceFolder
		{
			get { return mOrganiser.SourceFolder; }
			set
			{
				try
				{
					mOrganiser.SourceFolder = value; srcBox.Text = mOrganiser.SourceFolder;
				}
				catch (ArgumentException)
				{
					RequestSourceFolder();
				}
			}
		}
		public string DestinationFolder
		{
			get { return mOrganiser.DestinationFolder; }
			set
			{
				try
				{
					mOrganiser.DestinationFolder = value; destBox.Text = mOrganiser.DestinationFolder;
				}
				catch (ArgumentException)
				{
					RequestDestFolder();
				}
			}
		}


        public Mp3OrganiserForm()
        {
            InitializeComponent();
            autoCheck.Checked = true;
            deleteFilesCheck.Checked = false;
            PopulatePreferredFileCombo();
			UpdateFormatField();
        }

        private void PopulatePreferredFileCombo()
        {
			preferredTypeCombo.Items.Clear();
			preferredTypeCombo.Items.Add(Mp3Organiser.NullExtension);
			preferredTypeCombo.Items.AddRange(Mp3Organiser.SupportedExtensions);
			preferredTypeCombo.SelectedIndex = 0;
        }

		private void UpdateFormatField()
		{
			formatBox.Text = mOrganiser.FormatString;
			formatCompBox.Text = mOrganiser.FormatStringCompilation;
		}

		private void UpdateOrganiser()
		{
			Console.WriteLine("progBarStatus: " + progressBar.DialogResult);
			mOrganiser.PreferredFileExtenstion = ".mp3";
			mOrganiser.AutomaticallyCorrectFilenames = autoCheck.Checked;
			mOrganiser.DeleteSupportedFilesNotInSource = deleteFilesCheck.Checked;
			mOrganiser.FormatString = formatBox.Text;
			mOrganiser.FormatStringCompilation = formatCompBox.Text;
			mOrganiser.PreferredFileExtenstion = preferredTypeCombo.SelectedItem as string;
		}

        private void RequestDestFolder()
        {
            folderBrowser.Description = "Select Destination Folder";
            folderBrowser.SelectedPath = Properties.Settings.Default.DestFolder;
            folderBrowser.ShowDialog();
            if (folderBrowser.SelectedPath != null && folderBrowser.SelectedPath.Length > 0)
            {
                mOrganiser.DestinationFolder = folderBrowser.SelectedPath;
                destBox.Text = mOrganiser.DestinationFolder;
                Properties.Settings.Default.DestFolder = folderBrowser.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void RequestSourceFolder()
        {
            folderBrowser.Description = "Select Source Folder";
            folderBrowser.SelectedPath = Properties.Settings.Default.SourceFolder;
            folderBrowser.ShowDialog();
            if (folderBrowser.SelectedPath != null && folderBrowser.SelectedPath.Length > 0)
            {
                mOrganiser.SourceFolder = folderBrowser.SelectedPath;
                srcBox.Text = mOrganiser.SourceFolder;
                Properties.Settings.Default.SourceFolder = folderBrowser.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }
        #region Button Handlers
        private void srcButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender == srcButton) {
					RequestSourceFolder();
                }
                else if (sender == srcBox)
                {
                    KeyEventArgs k = e as KeyEventArgs;
                    if (k.KeyCode == Keys.Enter)
                    {
                        mOrganiser.SourceFolder = srcBox.Text;
                        srcBox.Text = mOrganiser.SourceFolder;
                    }
                }
                progressBar.Text = printCommands();
            }
            catch (Exception ex) { MessageBox.Show("" + ex); }
        }

        private void destButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender == destButton)
                {
					RequestDestFolder();
                }
                else if (sender == destBox)
                {
                    KeyEventArgs k = e as KeyEventArgs;
                    if (k.KeyCode == Keys.Enter)
                    {
                        mOrganiser.DestinationFolder = destBox.Text;
                        destBox.Text = mOrganiser.DestinationFolder;
                    }
                }
                progressBar.Text = printCommands();
            }
            catch (Exception ex) { MessageBox.Show("" + ex); }
        }

        private void formatButton_Click(object sender, EventArgs e)
        {

        }


        private void progressBar_ButtonClick(object sender, EventArgs e)
        {
			UpdateOrganiser();
            progressBar.StartWorker(mOrganiser.Organise);

        }
        #endregion


        private string printCommands()
        {
            string s = mOrganiser.SourceFolder;
            s += " " + mOrganiser.DestinationFolder;
            return s;
        }

        private void autoCheck_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void progressBar_Load(object sender, EventArgs e)
        {

        }

        private void deleteFilesCheck_CheckedChanged(object sender, EventArgs e)
        {
            
        }

    }
}
