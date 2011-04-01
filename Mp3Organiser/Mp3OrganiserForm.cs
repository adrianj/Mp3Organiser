using System;
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

        public string SourceFolder { get { return mOrganiser.SourceFolder; } set { mOrganiser.SourceFolder = value; srcBox.Text = mOrganiser.SourceFolder; } }
        public string DestinationFolder { get { return mOrganiser.DestinationFolder; } set { mOrganiser.DestinationFolder = value; destBox.Text = mOrganiser.DestinationFolder; } }


        public Mp3OrganiserForm()
        {
            mOrganiser.AutomaticallyCorrectFilenames = true;
            InitializeComponent();
        }

        #region Button Handlers
        private void srcButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (sender == srcButton) { 
                    folderBrowser.ShowDialog();
                    if (folderBrowser.SelectedPath != null && folderBrowser.SelectedPath.Length > 0) mOrganiser.SourceFolder = folderBrowser.SelectedPath;
                    srcBox.Text = mOrganiser.SourceFolder;
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
                    folderBrowser.ShowDialog();
                    if (folderBrowser.SelectedPath != null && folderBrowser.SelectedPath.Length > 0) mOrganiser.DestinationFolder = folderBrowser.SelectedPath;
                    destBox.Text = mOrganiser.DestinationFolder;
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
            mOrganiser.AutomaticallyCorrectFilenames = autoCheck.Checked;
        }

    }
}
