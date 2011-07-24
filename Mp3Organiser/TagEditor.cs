using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mp3Organiser
{
    class TagEditor
    {

        public byte[] TAGID = new byte[3];      //  3
        public byte[] Title = new byte[30];     //  30
        public byte[] Artist = new byte[30];    //  30 
        public byte[] Album = new byte[30];     //  30 
        public byte[] Year = new byte[4];       //  4 
        public byte[] Comment = new byte[30];   //  30 
        public byte[] Genre = new byte[1];      //  1

        private string mFilePath;

        public TagEditor(string filePath) { mFilePath = filePath;}

        public void ReadTag()
        {
            //string filePath = @"C:\Documents and Settings\All Users\Documents\My Music\Sample Music\041105.mp3";

            using (FileStream fs = File.OpenRead(mFilePath))
            {
                if (fs.Length >= 128)
                {
                    fs.Seek(-128, SeekOrigin.End);
                    fs.Read(TAGID, 0, TAGID.Length);
                    fs.Read(Title, 0, Title.Length);
                    fs.Read(Artist, 0, Artist.Length);
                    fs.Read(Album, 0, Album.Length);
                    fs.Read(Year, 0, Year.Length);
                    fs.Read(Comment, 0, Comment.Length);
                    fs.Read(Genre, 0, Genre.Length);
                    string theTAGID = Encoding.Default.GetString(TAGID);

                    if (theTAGID.Equals("TAG"))
                    {
                        string title = Encoding.Default.GetString(Title);
                        string artist = Encoding.Default.GetString(Artist);
                        string album = Encoding.Default.GetString(Album);
                        string year = Encoding.Default.GetString(Year);
                        string comment = Encoding.Default.GetString(Comment);
                        string genre = Encoding.Default.GetString(Genre);

                        Console.WriteLine("Title: "+title);
                        Console.WriteLine("Artist: " + artist);
                        Console.WriteLine("Album: " + album);
                        Console.WriteLine("Year: " + year);
                        Console.WriteLine("Comment: " + comment);
                        Console.WriteLine("Genre: " + genre);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
