using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace WebContentReader
{
    public partial class MainWindow : Form
    {
        private readonly WebClient _wc = new WebClient();
        private ContentParser _parser;
        private string theEbook;
        public MainWindow()
        {
            InitializeComponent();
            this.Text = "Main Window";
            txtBook.ScrollBars = ScrollBars.Vertical;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(urlInput.Text))
            {
                urlInput.Text = "http://www.cnn.com";
            }

            _wc.DownloadProgressChanged += DownloadProgressChanged;
            _wc.DownloadStringCompleted += StringDownloaded;
            _wc.DownloadStringAsync(new Uri(urlInput.Text.ToString()));
        }

        private void StringDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            theEbook = e.Result;

            _parser = new ContentParser(theEbook);


            var xpath = "//*[self::h1 or self::h2 or self::h3 or self::h4]";
            var tmp = _parser.GetContents(xpath);

            foreach (var htmlNode in tmp)
            {
                txtBook.Text += htmlNode.InnerText + "\r\n";
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());

            double percentage = bytesIn/totalBytes*100;

            progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
        }
        
        private async void btnGetStats_Click(object sender, EventArgs e)
        {
            //get the words from the eBook
            string[] words = theEbook.Split(new char[] {' ', '\u000A', ',', '.', ';', ':', '-', '?', '/'}, 
                StringSplitOptions.RemoveEmptyEntries);

            // now find the 10 most common words
            string[] tenMostCommon = await FindTenMostCommon(words);

            //get the longest word
            string longestWord = await FindLongestWord(words);

            StringBuilder bookStats = new StringBuilder();
            foreach (var s in tenMostCommon)
            {
                bookStats.AppendLine(s);
            }

            bookStats.AppendFormat("\r\nLongest word is: {0}", longestWord);
            bookStats.AppendLine();
            
            MessageBox.Show(bookStats.ToString(), "Ten most common words");
        }

        private async Task<string> FindLongestWord(string[] words)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(2000);
                return (from w in words orderby w.Length descending select w).FirstOrDefault();
            });

        }

        private async Task<string[]> FindTenMostCommon(string[] words)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(2000);

                var frequencyOrder = from word in words
                                     where word.Length > 6
                                     group word by word
                                     into g
                                     orderby g.Count() descending
                                     select g.Key;

                string[] commonWords = (frequencyOrder.Take(10).ToArray());

                return commonWords;
            });
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            urlInput.Text = "";
            txtBook.Text = "";
        }
    }
}
