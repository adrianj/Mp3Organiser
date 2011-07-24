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
        public bool DeleteSupportedFilesNotInSource { get; set; }
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
                if (Directory.Exists(value)) mDestFolder = value;
                else throw new ArgumentException("Destination Folder '" + value + "' does not exist.");
            }
        }

        private bool mCurrentTagIsCompilation = false;
        private Dictionary<string, string> mFileList = new Dictionary<string, string>();
        private int mCopiedFileCount = 0;
        private BackgroundWorker mWorker = null;

        public Mp3Organiser() {
        // Default FormatString
            FormatString = "{1}\\{2}\\{0:00} {3}{4}";
            FormatStringCompilation = "{2}\\{0:00} {1} - {3}{4}";
        }

        private int getProgress()
        {
            if (mFileList.Count < 1) return 0;
            return (int)((float)mCopiedFileCount * 100 / (float)mFileList.Count);
        }

        public object Organise(object p, BackgroundWorker w, DoWorkEventArgs e)
        {
            ReplaceSameFileDialogResult = DialogResult.Abort;
            if (mSourceFolder == null)// || mDestFolder == null)  // add second half later.
            {
                w.ReportProgress(0, "Source or Destination invalid");
                return null;
            }
            mWorker = w;
            mFileList.Clear();
            //mTotalFiles = 0;
            countFiles("\\");
            if(PreferredFileExtenstion != null)
                RemoveDuplicateFilesWithDifferentExtensions();
            if (mFileList.Count < 1)
            {
                w.ReportProgress(100, "No supported file types found in Source folder.");
                return null;
            }
            w.ReportProgress(getProgress(), "Found "+mFileList.Count+" files.");

            //List<string> targetFiles = new List<string>();

            foreach (KeyValuePair<string,string> pair in mFileList)
            {
                string file = pair.Key;
                string targetPath = pair.Value;
                string Extension = Path.GetExtension(file).ToLower();
                if (SupportedExtenstions.Contains(Extension))
                {
                    mCopiedFileCount++;
                    if (targetPath == null) continue;
                    if (!CreateFolder(targetPath)) throw new IOException("Could not create: '" + targetPath + "'");
                     if (ReplaceFile(targetPath))
                        {
                            w.ReportProgress(getProgress(), "Copying file '" + targetPath + "'");
                            CopyOrMove(file, targetPath);
                        }
                        else
                        {
                            w.ReportProgress(getProgress(), "Ignoring file '" + targetPath + "'");
                        }
                }
            }
            // Delete files that AREN'T in the list!
            if(DeleteSupportedFilesNotInSource)
                DeleteFilesNotInList(mDestFolder);
            w.ReportProgress(getProgress(), "DONE!");

            return null;
        }

        private void CopyOrMove(string sourceFile, string targetFile)
        {
            string sourceDir = Path.GetDirectoryName(sourceFile);
            string targetDir = Path.GetDirectoryName(targetFile);
            if (sourceDir.Equals(targetFile, StringComparison.InvariantCultureIgnoreCase))
                System.IO.File.Move(sourceFile, targetFile);
            else
                System.IO.File.Copy(sourceFile, targetFile);
        }

        private void RemoveDuplicateFilesWithDifferentExtensions()
        {
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string,string> pair in mFileList)
            {
                Console.WriteLine("Check: " + pair.Value);
                string targetPath = pair.Value;
                List<string> duplicates = GetKeysWithSameTargetPath(targetPath);
                if (duplicates.Count > 0)
                {
                    RemoveNonPrefferredFilenames(duplicates);
                    keysToRemove.AddRange(duplicates);
                }
            }
            foreach (string key in keysToRemove)
            {
                Console.WriteLine("Removing from list: " + mFileList[key]);
                mFileList.Remove(key);
            }
        }

        private void RemoveNonPrefferredFilenames(List<string> filenames)
        {
            int index = -1;
            for (int i = 0; i < filenames.Count; i++)
            {
                string ext = Path.GetExtension(filenames[i]);
                if (ext.Equals(PreferredFileExtenstion))
                    index = i;
            }
            if(index >= 0)
                filenames.RemoveAt(index);
        }

        private List<string> GetKeysWithSameTargetPath(string targetPath)
        {
            List<string> retList = new List<string>();
            string name = Path.GetFileNameWithoutExtension(targetPath);
            foreach (KeyValuePair<string, string> pair in mFileList)
            {
                string otherName = Path.GetFileNameWithoutExtension(pair.Value);
                if (otherName.Equals(name) && !targetPath.Equals(pair.Value))
                    retList.Add(pair.Key);
            }
            return retList;
        }


        private void DeleteFilesNotInList(string directory)
        {
            foreach (string subDir in Directory.GetDirectories(directory))
            {
                DeleteFilesNotInList(subDir);
            }
            foreach (string file in Directory.GetFiles(directory))
            {
                if (!mFileList.Values.Contains(file) && IsSupportedFileType(file))
                    DeleteFile(file);
            }
            int files = Directory.GetFiles(directory).Length;
            int dirs = Directory.GetDirectories(directory).Length;
            if ((files + dirs) < 1)
                Directory.Delete(directory);
        }

        public void DeleteFile(string filePath)
        {
            Console.WriteLine("Deleting: " + filePath);
            System.IO.File.Delete(filePath);
        }

        public bool ReplaceFile(string fullPath)
        {
            if(System.IO.File.Exists(fullPath))
            {
                Console.WriteLine("***Found Duplicate - Ignoring***");
                return false;
            }
            if (PreferredFileExtenstion != null)
            {                
                // Need to check if other files with different extensions exist, and keep preferred one.
                foreach (string supported in SupportedExtenstions.Split(new char[] { '|' }))
                {
                    string possiblePath = Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar+ Path.GetFileNameWithoutExtension(fullPath) + supported;
                    Console.WriteLine("possible file: '" + possiblePath + "'");
                    if (System.IO.File.Exists(possiblePath) && !supported.Equals(PreferredFileExtenstion))
                    {
                        Console.WriteLine("deleting file: '" + possiblePath + "'");
                    }
                }
                 
                return true;
            }
            return true;
        }

        /// <summary>
        /// Gets the tag information from an audio file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public Tag OpenTag(string fullPath)
        {
            Tag tag = null;
            try
            {
                TagLib.File file = TagLib.File.Create(fullPath);
                
                mCurrentTagIsCompilation = false;
                if ((file.TagTypes & TagTypes.Id3v2) > 0)
                {
                    TagLib.Id3v2.Tag iTag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
                    if (iTag.IsCompilation) mCurrentTagIsCompilation = true;
                    return iTag;
                }
                
                if ((file.TagTypes & TagTypes.Apple) > 0)
                {
                    TagLib.Mpeg4.AppleTag iTag = (TagLib.Mpeg4.AppleTag)file.GetTag(TagTypes.Apple);
                    if (iTag.IsCompilation) mCurrentTagIsCompilation = true;
                    return iTag;
                }
                 
                // If not any of above tags, then try the default create.
                tag = file.Tag;
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
            foreach (string sourcePath in Directory.GetFiles(srcDir))
            {
                if (IsSupportedFileType(sourcePath))
                {
                    string targetPath = GetPathFromInfo(sourcePath);
                    if (IsValidToAdd(targetPath))
                    {
                        total++;
                        mFileList.Add(sourcePath, targetPath);
                    }
                }
            }
            if (mWorker != null)
                mWorker.ReportProgress(0, "Counting files...\n"+pathRelativeToSource);
            return total;
        }

        private bool IsValidToAdd(string targetPath)
        {
            if (!mFileList.Values.Contains(targetPath))
                return true;
            return false;
        }

        private bool IsSupportedFileType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (SupportedExtenstions.Contains(extension))
                return true;
            else 
                return false;
        }

    }
}
