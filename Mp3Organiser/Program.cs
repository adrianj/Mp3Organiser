using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mp3Organiser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Mp3OrganiserForm form = new Mp3OrganiserForm();
            if (args.Length > 0) form.SourceFolder = args[0];
            else form.SourceFolder = Properties.Settings.Default.SourceFolder;
            if (args.Length > 1) form.DestinationFolder = args[1];
            else form.DestinationFolder = Properties.Settings.Default.DestFolder;
            Application.Run(form);
        }
    }
}
