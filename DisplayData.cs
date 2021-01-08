using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace twitter
{
    class DisplayData
    {
        private string outputText;
        public string GenerateDisplay(DataModel dm)
        {
            string[] te = new string[3];
            string[] tu = new string[3];
            string dashes = "---------------------------------------------------------------";
            outputText = dashes + "\r\nNot sure if it's because of my token rights...\r\nThe only url that shows up in tweets is the media " +
                "url\r\nhttp://t.co\r\n" + dashes + "\r\nTotal Running Minutes: $$runningminutes$$\r\nTotal Tweets: " + dm.totalTweet.ToString()
              + "\r\nAvg Tweets per second: $$avgpersec$$\r\nAvg Tweets per min: $$avgpermin$$\r\nAvg Tweets per hour: $$avgperhour$$\r\n"
              + "Total Media: It appers that http://t.co are media links\r\nF-word mentioned: $$fword$$\r\n"
              + "Trump mentioned: $$trump$$\r\nBiden mentioned: $$biden$$\r\nPercent of tweets with URL: %$$url$$\r\n"
              + "Tweets that contain emojis: $$emojitally$$\r\nTop 3 Emojis: $$top3$$Top 3 Hashtags: $$hashtags$$"
              + "\r\nPercent of Tweets that have emojis: %$$percOfEmojis$$\r\nTop 3 Domains:\r\n$$top3Domains$$\r\n";

            outputText = outputText.Replace("$$totalMediaHold$$", dm.totalMedia.ToString()).Replace("$$fword$$", dm.fCount.ToString())
                .Replace("$$trump$$", dm.trumpCount.ToString()).Replace("$$biden$$", dm.bidenCount.ToString()).Replace("$$url$$", dm.percentUrl.ToString())
                .Replace("$$emojitally$$", dm.totalEmoji.ToString())
                .Replace("$$top3$$", dm.topE).Replace("$$percOfEmojis$$", dm.percentTweet.ToString()).Replace("$$top3Domains$$", dm.top3Url)
                .Replace("$$hashtags$$", dm.topHashtags).Replace("$$avgpersec$$", dm.avgTweetPerSec.ToString())
                .Replace("$$avgpermin$$", dm.avgTweetPerMin.ToString()).Replace("$$runningminutes$$", dm.runningMinutes.ToString())
                .Replace("$$avgperhour$$",dm.avgTweetPerHour.ToString());
            return outputText;
        }
        
    }
}
