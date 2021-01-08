using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace twitter
{
    class tweetAvgCalc
    {
        int sixty = 60;
        int threeK = 3600;
        int runningMinutes = 0;
        List<float> secList = new List<float>();
        List<float> minList = new List<float>();
        List<float> hourList = new List<float>();
        float oneSec = 0;
        float oneMin = 0;
        float oneHour = 0;
        int loopSeconds;
        public Tuple<float,float,int,float> avgTweetsPerSecond(float elapsedTime, int tweetAmount, DataModel dm, TwitterFeedDriver tfd)
        {
            loopSeconds = tfd.StreamTime / 1000;
            float tweet = 0;
            dm.totalTweet += tweetAmount;
            tweet = tweetAmount / loopSeconds;
            oneSec = 0;
            for(int a=0;a<loopSeconds;a++)
            {
                secList.Add(tweet);
            }
            tweet = 0;
            foreach(float b in secList)
            {
                tweet += b;
            }
            if(secList.Count == sixty)
            {
                oneMin = calcMinHour("minute");
                sixty += 60;
            }
            if (secList.Count == threeK)
            {
                oneHour = calcMinHour("hour");
                threeK += 3600;
            }
            oneSec = float.Parse(Math.Round((tweet / secList.Count), 2).ToString());
            return new Tuple<float,float,int,float>(oneSec,oneMin,runningMinutes,oneHour);
        }

        private float calcMinHour(string timeFactor)
        {
            float oneTM = 0;
            int timeClick = 1;
            runningMinutes++;
            List<float> t;
            t = timeFactor == "minute" ? minList : hourList;
            int timeCond = timeFactor == "minute" ? 60 : 3600;
            foreach (float b in secList)
            {
                oneTM += b;
                timeClick++;
                if (timeClick == timeCond)
                {
                    timeClick = 1;
                    t.Add(oneTM);
                    oneTM = 0;
                }
            }
            oneTM = 0;
            foreach (float a in t)
            {
                oneTM += a;
            }
            return oneTM = float.Parse(Math.Round((oneTM / t.Count), 2).ToString());
        }
    }
}
