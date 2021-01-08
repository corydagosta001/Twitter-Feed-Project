using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Timers;
using System.Net.Sockets;
using System.IO;
using System.Net.Http.Headers;


namespace twitter
{
    public partial class TwitterFeedDriver : Window
    {
        DataAnalyzer da;
        urlProcess up;
        DataModel dm;
        io_class ic;
        emojiRef er;
        dataCollection dc;
        DisplayData dd;
        tweetAvgCalc tac;
        Thread thr;
        HashtagProcess hp;
        List<string> FeedList = new List<string>();
        public int StreamTime = 3000;
        readonly string token;
        readonly string twitterPath;
        public Boolean go = true;
        CancellationTokenSource cts = new CancellationTokenSource();
        System.Timers.Timer timer;
        int pngCount = 0;
        int jpgCount = 0;
        int jimmyPageCount = 0;
        string outputText = "";
        public TwitterFeedDriver()
        {
            thr = Thread.CurrentThread;
            da = new DataAnalyzer();
            up = new urlProcess();
            dm = new DataModel();
            ic = new io_class();
            er = new emojiRef();
            dc = new dataCollection();
            dd = new DisplayData();
            hp = new HashtagProcess();
            tac = new tweetAvgCalc();
            token = "AAAAAAAAAAAAAAAAAAAAAJADKgEAAAAAD4w2DLjwp9rtzAPQFnnlqUaF32c%3DUO6irrSGMTKi4LYutW1K0EZpGbajPMotfdlJlqen26pn7iokyR";
            twitterPath = "https://api.twitter.com/2/tweets/sample/stream";
            dm.seconds = 0;
            StartStreaming();
        }



        private async Task<List<string>> passList(List<string> l)
        {
            List<string> newList = new List<string>();
            await Task.Run(() =>
            {
                try
                {
                    foreach (string t in l)
                    {
                        newList.Add(t);
                    }
                }
                catch (Exception) 
                {
                 //   passList(l);            
                }
            });
            return newList;
        }

        private async void ProcessFeed()
        {
            float percentTweet = 0;
            string StrippedText = "";
            string[] te = new string[3];
            string[] tu = new string[3];
            var backFeed = await passList(FeedList);
            string rn = "\r\n";
            FeedList.Clear();
            if (backFeed.Count > 0)
            {
                dm.avgTweetPerSec = tac.avgTweetsPerSecond(dm.seconds, backFeed.Count, dm,this).Item1;
                dm.avgTweetPerMin = tac.avgTweetsPerSecond(dm.seconds, backFeed.Count, dm,this).Item2;
                dm.runningMinutes = tac.avgTweetsPerSecond(dm.seconds, backFeed.Count, dm,this).Item3;
                dm.avgTweetPerHour = tac.avgTweetsPerSecond(dm.seconds, backFeed.Count, dm,this).Item4;
                try
                {
                    await Task.Run(() =>
                    {
                        foreach (string ele in backFeed)
                        {
                            StrippedText = da.TextStrip(ele) + " ";
                            dm.totalMedia += da.SearchForPicUrl(StrippedText);
                            dm.fCount += da.mentionedWord(StrippedText, "fuck");
                            dm.trumpCount += da.mentionedWord(StrippedText, "trump");
                            dm.bidenCount += da.mentionedWord(StrippedText, "biden");
                            pngCount += da.mentionedWord(StrippedText, ".png");
                            jpgCount += da.mentionedWord(StrippedText, ".jpg");
                            dm.percentUrl = up.urlTracker(StrippedText, dm.totalTweet);
                            er.emojiTally = da.tallyEmoji(StrippedText, er.emojiTally, er.emojiName, dm.totalTweet).Item1;
                            dm.totalEmoji += da.tallyEmoji(StrippedText, er.emojiTally, er.emojiName, dm.totalTweet).Item3;
                            dm.topHashtags = hp.HashProcess(StrippedText, true);
                            tu = up.topUrl();
                            te = da.topEmojis(er.emojiName, er.emojiTally);
                            outputText += StrippedText + rn + "----------------------------------------------------------" + rn;
                        }
                    });
                }
                catch (Exception)
                {
                    ProcessFeed();
                }
                string topE = rn;
                string top3Url = "";
                foreach (string s in te)
                {
                    topE += "    " + s + rn;
                }

                foreach (string s in tu)
                {
                    top3Url += "    " + s + rn;
                }
                percentTweet = ((100 / float.Parse(dm.totalTweet.ToString()))) * dm.totalEmoji;
                percentTweet = float.Parse(Math.Round(percentTweet, 2).ToString());
                dm.jimmyPageCount = jimmyPageCount;
                dm.totalUrl = 0;
                dm.top3Url = top3Url;
                dm.topE = topE;
                dm.TwitterFeed = "";
                dm.percentTweet = percentTweet;
                TB.Text = outputText;
                outputText = "";
                statHold.Text = dd.GenerateDisplay(dm);
            }
            backFeed.Clear();
            backFeed = null;
        }


        async Task GetFromApi(string path)
        {
            HttpClient client = new HttpClient();
            go = true;
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, path);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            using (var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
            {
                var body = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(body);
                await Task.Run(() =>
                {
                    while (!reader.EndOfStream && go == true)
                    {
                        FeedList.Add(reader.ReadLine());
                        if (go == false)
                        {
                            go = true;
                        }
                    }
                });
            }
        }

        public async void StartStreaming()
        {
            go = true;
            timer = new System.Timers.Timer();
            timer.Interval = StreamTime;
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            await GetFromApi(twitterPath);
        }


        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            dm.seconds += StreamTime / 1000;
            try
            {
                Dispatcher.Invoke(() =>
                {
                    ProcessFeed();
                });
            }
            catch (Exception ex9)
            {
                MessageBox.Show(ex9.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            go = false;
            StopStream.IsEnabled = false;
            StreamData.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartStreaming();
            StopStream.IsEnabled = true;
            StreamData.IsEnabled = false;
        }

    }
}
