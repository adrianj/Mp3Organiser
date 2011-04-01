using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using TagLib;
using System.Windows.Forms;

namespace Mp3Organiser
{
    public class Mp3Organiser
    {
        /// <summary>
        /// Assumes parameters are:
        /// {0} = Track number
        /// {1} = First Album Artist
        /// {2} = Album
        /// {3} = Title
        /// {4} = File extension (typically ".mp3")
        /// </summary>
        public string FormatString { get; set; }
        /// <summary>
        /// Specifies whether to automatically replace invalid path characters with ' '
        /// </summary>
        public bool AutomaticallyCorrectFilenames { get; set; }
        private string mPreferredExt;
        /// <summary>
        /// Defines behaviour for copying files with same details but different extensions.
        /// When 'null' both files are kept, otherwise the preferred extenstion is used.
        /// </summary>
        public string PreferredFileExtenstion
        {
            get { return mPreferredExt; }
            set { if (value != null) mPreferredExt = value.ToLower(); else mPreferredExt = null; }
        }
        public DialogResult ReplaceSameFileDialogResult { get; set; }
        private string mSupportedExtensions = ".mp3|.m4a";
        public string SupportedExtenstions
        {
            get { return mSupportedExtensions; }
            set { if (value != null) mSupportedExtensions = value.ToLower(); else mSupportedExtensions = ".mp3|.m4a"; }
        }
        private string mSourceFolder;
        private string mDestFolder;
        public string SourceFolder
        {
            get { return mSourceFolder; }
            set
            {
                if(value == null) return;
                if (Directory.Exists(value)) 
                    if(!value.Equals(mDestFolder))
                        mSourceFolder = value;
                    else throw new ArgumentException("Destination folder cannot be the same as source folder.");
                else throw new ArgumentException("Source Folder '"+value+"' does not exist.");
            }
        }
        public string DestinationFolder
        {
            get { return mDestFolder; }
            set
            {
                if(value == null) return;
                if (Directory.Exists(value))
                    if (mSourceFolder != null && !mSourceFolder.Contains(value)) mDestFolder = value;
                    else throw new ArgumentException("Destination folder cannot be within source folder.");
                else throw new ArgumentException("Destination Folder '" + value + "' does not exist.");
            }
        }

        public Mp3Organiser() {
        // Default FormatString
            FormatString = "\"{1}\\{2}\\{0:00} - {3}{4}";
        }

        private int mTotalFiles = 1;
        private int mFileCount = 0;
        private int getProgress()
        {
            if (mTotalFiles < 1) return 0;
            return (int)((float)mFileCount * 100 / (float)mTotalFiles);
        }

        public object Organise(object p, BackgroundWorker w, DoWorkEventArgs e)
        {
            //PreferredFileExtenstion = ".m4a";
            ReplaceSameFileDialogResult = DialogResult.Abort;
            if (mSourceFolder == null)// || mDestFolder == null)  // add second half later.
            {
                w.ReportProgress(0, "Source or Destination invalid");
                return null;
            }
            w.ReportProgress(0,"Counting files...");
            mTotalFiles = 0;
            mTotalFiles = countFiles("\\");
            if (mTotalFiles < 1)
            {
                w.ReportProgress(100, "No supported file types found in Source folder.");
                return null;
            }
            w.ReportProgress(getProgress(), "Found "+mTotalFiles+" files.");

            Organise("\\", w, e);

            w.ReportProgress(getProgress(), "DONE!");

            return null;
        }

        public void Organise(string pathRelativeToSource, BackgroundWorker w, DoWorkEventArgs e)
        {
            string srcDir = mSourceFolder + pathRelativeToSource;
            foreach (string dir in Directory.GetDirectories(srcDir))
            {
                Organise(pathRelativeToSource + Path.GetFileName(dir) + Path.DirectorySeparatorChar,w,e);
            }
            foreach (string file in Directory.GetFiles(srcDir))
            {
                string Extension = Path.GetExtension(file).ToLower();
                if (SupportedExtenstions.Contains(Extension))
                {
                    mFileCount++;
                    string targetPath = GetPathFromInfo(file);
                    if (targetPath == null) continue;
                    if (!CreateFolder(targetPath)) throw new IOException("Could not create: '" + targetPath + "'");
                    if (ReplaceFile(targetPath))
                    {
                        w.ReportProgress(getProgress(), "Copying file '" + targetPath + "'");
                        System.IO.File.Copy(file, targetPath);
                    }
                    else
                    {
                        w.ReportProgress(getProgress(), "Ignoring file '" + targetPath + "'");
                    }
                }
            }
        }

        public bool ReplaceFile(string fullPath)
        {
            if(System.IO.File.Exists(fullPath))
            {
                Console.WriteLine("**************What to do with duplicate files?????**************");
                return false;
            }
            if (PreferredFileExtenstion != null)
            {                
                // Need to check if other files with different extensions exist, and keep preferred one.
                /*
                foreach (string supported in SupportedExtenstions.Split(new char[] { '|' }))
                {
                    string possiblePath = Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar+ Path.GetFileNameWithoutExtension(fullPath) + supported;
                    Console.WriteLine("possible file: '" + possiblePath + "'");
                    if (System.IO.File.Exists(possiblePath) && !supported.Equals(PreferredFileExtenstion))
                    {
                        Console.WriteLine("deleting file: '" + possiblePath + "'");
                    }
                }
                 */
                return true;
            }
            return true;
        }

        public string GetPathFromInfo(string fullPath)
        {
            Tag tag;
            try { tag = TagLib.File.Create(fullPath).Tag; }
            catch { return null; }
            string Extension = Path.GetExtension(fullPath).ToLower();
            string Album = tag.Album;
            if (Album == null || Album.Length == 0) Album = "Unknown Album";
            string Artist = tag.FirstPerformer;
            if (Artist == null || Artist.Length == 0) Artist = tag.FirstAlbumArtist;
            if (Artist == null || Artist.Length == 0) Artist = "Unknown Artist";
            string Title = tag.Title;
            if (Title == null || Title.Length == 0) Title = "Unknown";
            int Track = (int)tag.Track;
            if (Track < 1 || Track > 999) Track = 0;

            string possiblePath = mDestFolder + Path.DirectorySeparatorChar + string.Format(FormatString, Track, Artist, Album, Title, Extension);
            string newPath = GetValidPath(possiblePath);
            if (newPath == null) throw new IOException("Invalid Path: '" + possiblePath + "'");
            return newPath;
        }

        public bool CreateFolder(string path)
        {
            string folder = Path.GetDirectoryName(path);
            try
            {
                DirectoryInfo d = Directory.CreateDirectory(folder);
                if (d.Exists) return true;
            }
            catch (IOException) { return false; }
            return false;
        }

        public string GetValidPath(string path)
        {
            string p = path;
            // Other possibility is that path elements begin or end with ' '
            string[] parts = p.Split(new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar});
            if(parts.Length < 1) return null;
            string drive = GetValidDrive(parts[0]);
            if (drive == null) return null;
            string finalP = drive;

            List<char> invalidChars = new List<char>(Path.GetInvalidPathChars());
            if (!invalidChars.Contains(':')) invalidChars.Add(':');
            if (!invalidChars.Contains('?')) invalidChars.Add('?');
            if (!invalidChars.Contains('*')) invalidChars.Add('*');

            for (int i = 1; i < parts.Length; i++)
            {

                // Remove invalid characters.
                foreach (char invalid in invalidChars)
                {
                    if (parts[i].Contains(invalid))
                    {
                        if (AutomaticallyCorrectFilenames) parts[i] = parts[i].Replace(invalid, ' ');
                        else return null;
                    }
                }
                // Drive char ':' is also no good for this part of the path.
                if (parts[i].Contains(':'))
                {
                    if (AutomaticallyCorrectFilenames) parts[i] = parts[i].Replace(':', ' ');
                    else return null;
                }

                string newP = "";
                foreach (string s in parts[i].Split(new char[] { ' ' }))
                {
                    if (s.Length == 0 && !AutomaticallyCorrectFilenames) return null;
                    else if(s.Length > 0) newP += s + " ";
                }

                // Also check if it is on a logical drive
                if (newP.Length == 0 && !AutomaticallyCorrectFilenames) return null;
                else if(newP.Length>0) finalP += newP.Substring(0, newP.Length - 1)+Path.DirectorySeparatorChar;
            }
            finalP = finalP.Substring(0, finalP.Length - 1);
            return finalP;
        }

        public string GetValidDrive(string drive)
        {
            string d = drive;
            if (d.Length > 2) return null;
            if (d.Length == 0) return "\\"; // network drive, no attempt to check it!
            d += Path.DirectorySeparatorChar;
            foreach (string availableDrives in Directory.GetLogicalDrives())
            {
                if (availableDrives.Equals(d,StringComparison.InvariantCultureIgnoreCase)) return d;
            }
            return null;
        }

        public int countFiles(string pathRelativeToSource)
        {
            int total = 0;
            string srcDir = mSourceFolder + pathRelativeToSource;
            foreach (string dir in Directory.GetDirectories(srcDir))
            {
                total += countFiles(pathRelativeToSource + Path.GetFileName(dir) + Path.DirectorySeparatorChar);
            }
            foreach (string file in Directory.GetFiles(srcDir))
            {
                string Extension = Path.GetExtension(file).ToLower();
                if (SupportedExtenstions.Contains(Extension))
                    total++;
            }
            return total;
        }

    }
}
