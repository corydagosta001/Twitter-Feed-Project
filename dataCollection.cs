using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace twitter
{
    class dataCollection
    {
        Boolean go = false;
        float percentTweet = 0;
        string StrippedText = "";
        float percentUrl = 0;
        string[] te = new string[3];
        string[] tu = new string[3];
        //var backFeed = await passList(FeedList);

        //FeedList.Clear();

        private async Task<List<string>> passList(List<string> l)
        {
            List<string> newList = new List<string>();
            await Task.Run(() =>
            {
                foreach (string t in l)
                {
                    newList.Add(t);
                }
            });
            return newList;
        }
        //dm.outputFeed += outputText; 
        public async void processData(List<string> FeedList, DataModel dm, DataAnalyzer da, emojiRef er, urlProcess up)
        {
            var backFeed = await passList(FeedList);
            string outputText = "";
            await Task.Run(() =>
            {
                foreach (string ele in backFeed)
                {
                    StrippedText = da.TextStrip(ele) + " ";
                    dm.totalMedia += da.SearchForPicUrl(StrippedText);
                    dm.fCount += da.mentionedWord(StrippedText, "fuck");
                    dm.trumpCount += da.mentionedWord(StrippedText, "trump");
                    dm.bidenCount += da.mentionedWord(StrippedText, "biden");
                    //pngCount += da.mentionedWord(StrippedText, ".png");
                    //jpgCount += da.mentionedWord(StrippedText, ".jpg");
                    percentUrl = up.urlTracker(StrippedText, dm.totalTweet);
                    er.emojiTally = da.tallyEmoji(StrippedText, er.emojiTally, er.emojiName, dm.totalTweet).Item1;
                    //h += da.tallyEmoji(StrippedText, er.emojiTally, er.emojiName, dm.totalTweet).Item2;
                    dm.totalEmoji += da.tallyEmoji(StrippedText, er.emojiTally, er.emojiName, dm.totalTweet).Item3;
                    tu = up.topUrl();
                    te = da.topEmojis(er.emojiName, er.emojiTally);
                   outputText += StrippedText + "\r\n" + "-----------------------------------------------------------\r\n";
                }
            });

            percentTweet = ((100 / float.Parse(dm.totalTweet.ToString()))) * dm.totalEmoji;
            percentTweet = float.Parse(Math.Round(percentTweet, 2).ToString());
            dm.percentTweet = percentTweet;
            dm.backfeedCount = backFeed.Count;
            dm.totalTweet += backFeed.Count;
            foreach (string s in te)
            {
                dm.topE += "    " + s + "\r\n";
            }

            foreach (string s in tu)
            {
                dm.top3Url += "    " + s + "\r\n";
            }
            //configure shutoff for the following
            dm.outputFeed += outputText;
            backFeed.Clear();
        }
    }
}
