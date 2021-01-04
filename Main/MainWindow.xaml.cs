using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Leaf.xNet; 


namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Song> listSong;

        public ObservableCollection<Song> ListSong { get => listSong; set { listSong = value; lsbTopSong.ItemsSource = ListSong; } }

        
        public MainWindow()
        {
            InitializeComponent();
            ucSongInfor.BackToMain += UcSongInfor_BackToMain;

            this.DataContext = this;
            
            ListSong = new ObservableCollection<Song>(); 

           
        }

        private void CrawlData(string searching)
        {
            HttpRequest http = new HttpRequest();
            string searchUrl = @"https://nhac.vn/search?q=" + searching;  
            string htmlSearch = http.Get(searchUrl).ToString();
            string dataPattern = @"<div class=""box_content"">(.*?)<div class=""video_widget video_list_large box""";
            var dsData = Regex.Matches(htmlSearch, dataPattern, RegexOptions.Singleline);

            //Lay cai bang dau tien thoi 
            try
            {
                string dsBaiHat = dsData[0].ToString();
                dataPattern = @"<li class=""song-list-new-item""(.*?)</li>";
                var listSongHTML = Regex.Matches(dsBaiHat, dataPattern, RegexOptions.Singleline);

                ListSong.Clear(); 

                foreach (var item in listSongHTML)
                {
                    dataPattern = @"<a\s\S*title=""(.*?)""";

                    //Lay ra ten bai hat va ca si
                    var songandsinger = Regex.Matches(item.ToString(), dataPattern, RegexOptions.Singleline);

                    // Loc ra ten bai hat
                    string songString = songandsinger[0].ToString();
                    int indexSong = songString.IndexOf("title=\"");
                    string songName = songString.Substring(indexSong, songString.Length - indexSong - 1).Replace("title=\"", "");

                    //Loc ra ten ca si
                    string singerString = songandsinger[1].ToString();
                    int indexSinger = singerString.IndexOf("title=\"");
                    string singerName = singerString.Substring(indexSinger, singerString.Length - indexSinger - 1).Replace("title=\"", "");

                    //Loc ra link hinh anh
                    var imageSource = Regex.Matches(item.ToString(), @"<img src=""(.*?)""", RegexOptions.Singleline);
                    string imageString = imageSource[0].ToString();
                    int indexImage = imageString.IndexOf("<img src=");
                    string imageLink = imageString.Substring(indexImage, imageString.Length - indexImage - 1).Replace("<img src=\"", "");


                    // Loc ra link nhac 
                    var linkMusic = Regex.Matches(item.ToString(), @"href=""(.*?)""", RegexOptions.Singleline);
                    string stringMusic = linkMusic[0].ToString();
                    int indexMusic = stringMusic.IndexOf("href=");
                    string musicLinkfinal = stringMusic.Substring(indexMusic, stringMusic.Length - indexMusic - 1).Replace("href=\"", ""); 

                    ListSong.Add(new Song(songName, singerName, imageLink, musicLinkfinal));
                }
               
            }
            catch (Exception)
            {

            }
           
        }

        private void UcSongInfor_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            ucSongInfor.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridTop10.Visibility = Visibility.Hidden;
            ucSongInfor.Visibility = Visibility.Visible;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchString = searchContent.Text;
            searchString = searchString.Replace(" ", "+");
           
            CrawlData(searchString);
        }
    }
}
