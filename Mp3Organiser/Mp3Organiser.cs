﻿using System;
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
        /// String to define filename formatting. See String.Format for how it works.
        /// Assumes parameters are:
        /// {0} = Track number
        /// {1} = First Album Artist
        /// {2} = Album
        /// {3} = Title
        /// {4} = File extension (typically ".mp3")
        /// </summary>
        public string FormatString { get; set; }
        /// <summary>
        /// String to define filename formatting for tracks noted as part of a Compilation. See String.Format for how it works.
        /// </summary>
        public string FormatStringCompilation { get; set; }
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
                        mSourceFolder = value;
                else throw new ArgumentException("Source Folder '"+value+"' does not exist.");
            }
        }
        public string DestinationFolder
        {
            get { return mDestFolder; }
            set
            {
                if(value == null) return;
                if (Directory.Exists(value)) mDestFolder = value;
                else throw new ArgumentException("Destination Folder '" + value + "' does not exist.");
            }
        }

        public Mp3Organiser() {
        // Default FormatString
            FormatString = "{1}\\{2}\\{0:00} - {3}{4}";
            FormatStringCompilation = "{2}\\{0:00} - {1} - {3}{4}";
        }

        private List<string> mFileList = new List<string>();
        //private int mTotalFiles = 1;
        private int mFileCount = 0;
        private int getProgress()
        {
            if (mFileList.Count < 1) return 0;
            return (int)((float)mFileCount * 100 / (float)mFileList.Count);
        }
        private bool mCurrentTagIsCompilation = false;

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
            mFileList.Clear();
            //mTotalFiles = 0;
            //mTotalFiles = countFiles("\\");
            if (mFileList.Count < 1)
            {
                w.ReportProgress(100, "No supported file types found in Source folder.");
                return null;
            }
            w.ReportProgress(getProgress(), "Found "+mFileList.Count+" files.");

            foreach (string file in mFileList)
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

            w.ReportProgress(getProgress(), "DONE!");

            return null;
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

        /// <summary>
        /// Gets the tag information from an audio file.
        /// It uses the "Comment" field
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public Tag OpenTag(string fullPath)
        {
            Tag tag = null;
            // finally, let TagLib decide, but CompilationInfo will default as false.
            try
            {
                TagLib.File file = TagLib.File.Create(fullPath);
                Console.WriteLine(Path.GetFileNameWithoutExtension(fullPath)+"\nTAGS: " + file.TagTypes);
                
                mCurrentTagIsCompilation = false;
                if ((file.TagTypes & TagTypes.Id3v2) > 0)
                {
                    TagLib.Id3v2.Tag iTag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
                    Console.WriteLine("iTag: " + iTag + "," + iTag.IsCompilation);
                    if (iTag.IsCompilation) mCurrentTagIsCompilation = true;
                    return iTag;
                }
                
                if ((file.TagTypes & TagTypes.Apple) > 0)
                {
                    TagLib.Mpeg4.AppleTag iTag = (TagLib.Mpeg4.AppleTag)file.GetTag(TagTypes.Apple);
                    Console.WriteLine("iTag: " + iTag + "," + iTag.IsCompilation);
                    if (iTag.IsCompilation) mCurrentTagIsCompilation = true;
                    return iTag;
                }
                 
                // If not any of above tags, then try the default create.
                tag = file.Tag;
                Console.WriteLine("..." + (file.TagTypes & TagTypes.Id3v2) + " COMPILATION: " + mCurrentTagIsCompilation);
            }
            catch
            {
                return null;
            }
                mCurrentTagIsCompilation = false;
            return tag;
        }

        public string GetPathFromInfo(string fullPath)
        {
            Tag tag = OpenTag(fullPath);
            if (tag == null) return null;

            string Extension = Path.GetExtension(fullPath).ToLower();
            string Album = tag.Album;
            if (Album == null || Album.Length == 0) Album = "Unknown Album";
            string Artist = tag.FirstPerformer;
            if (Artist == null || Artist.Length == 0) Artist = tag.FirstAlbumArtist;
            if (Artist == null || Artist.Length == 0) Artist = "Unknown Artist";
            string Title = tag.Title;
            if (Title == null || Title.Length == 0) Title = "Unknown Title";
            int Track = (int)tag.Track;
            if (Track < 1 || Track > 999) Track = 0;
                

            string formattedPath = string.Format(FormatString, Track, Artist, Album, Title, Extension);
            if (mCurrentTagIsCompilation) formattedPath = string.Format(FormatStringCompilation, Track, Artist, Album, Title, Extension);
            string possiblePath = mDestFolder + Path.DirectorySeparatorChar + formattedPath;
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
                {
                    total++;
                    mFileList.Add(file);
                }
            }
            return total;
        }

    }
}