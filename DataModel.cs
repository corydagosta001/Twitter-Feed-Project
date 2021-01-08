using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace twitter
{

    class DataModel
    {
        public int totalTweet { get; set; }
        public int totalMedia { get; set; }
        public int fCount { get; set; }
        public int trumpCount { get; set; }
        public int bidenCount { get; set; }
        public int jimmyPageCount { get; set; }
        public float percentUrl { get; set; }
        public int totalEmoji { get; set; }
        public string topEmogis { get; set; }
        public int totalUrl { get; set; }
        public string topUrl { get; set; }
        public float percentTweet { get; set; }
        public string TwitterFeed { get; set; }
        public string topE { get; set; }
        public string top3Url { get; set; }
        public int backfeedCount { get; set; }
        public string outputFeed { get; set; }
        public string topHashtags { get; set; }
        public float seconds { get; set; }
        public float avgTweetPerSec { get; set; }
        public float avgTweetPerMin { get; set; }
        public float avgTweetPerHour { get; set; }
        public int runningMinutes { get; set; }
    }
}
