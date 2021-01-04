using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Song
    {
        private string songName;
        private string singerName;
        private string imageSource;
        private string linkMusic;
        private string linkDownload; 

        public Song(string songname, string singer, string image, string musicLink, string downloadLink)
        {
            SongName = songname;
            SingerName = singer;
            ImageSource = image;
            LinkMusic = musicLink;
            LinkDownload = downloadLink; 
        }

        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string ImageSource { get => imageSource; set => imageSource = value; }
        public string LinkMusic { get => linkMusic; set => linkMusic = value; }
        public string LinkDownload { get => linkDownload; set => linkDownload = value; }
    }
}
