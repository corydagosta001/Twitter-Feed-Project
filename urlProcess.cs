using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
namespace twitter
{
    interface Iurl
    { 
        Dictionary<string, int> UrlDictionary { get; set; }
        float urlTracker(string text, int totalTweet);
    }

    class urlProcess : Iurl
    {
        public Dictionary<string, int> UrlDictionary { get; set;}
        private List<string> urlList = new List<string>();

        public urlProcess()
        {
            UrlDictionary = new Dictionary<string, int>();
        }

        public float urlTracker(string text, int totalTweet)
        {
            text = text.Replace("\\n", " ");
            string[] t = text.Split(' ');
            float percentUrl;
            foreach (string i in t)
            {
                if(i.IndexOf("http") == 0)
                {
                    urlList.Add(i);
                    StripUrl(i);
                }
            }
            percentUrl = ((100 / float.Parse(totalTweet.ToString()))) * urlList.Count;
            return percentUrl = float.Parse(Math.Round(percentUrl, 2).ToString());
        }


        public void PrintList()
        {
            string strContent = "";
            foreach (string i in urlList)
            {
                strContent += i + "\r\n";
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\urlList.txt", strContent);
            strContent = "";
            foreach(var h in UrlDictionary)
            {
                strContent += "[" + h.Value + "] " + h.Key + "\r\n";
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\urlDictionary.txt", strContent);
        }

        private void StripUrl(string url)
        {
            string strippedUrl = url;
            int intStart1;
            intStart1 = url.IndexOf("://");
            string temp = "";
            if(intStart1 != -1)
            {
                intStart1 += 3;
                temp = url.Substring(0, intStart1);
                strippedUrl = url.Substring(intStart1, url.Length - intStart1);
                string[] splitUrl = strippedUrl.Split('/');
                strippedUrl = temp + splitUrl[0];
            }
            else
            {
                strippedUrl = url;
            }
            
            try
            {
                BuildUrlDictionary(strippedUrl,url);
            }
            catch(Exception){}
        }

        private void BuildUrlDictionary(string sUrl, string url)
        {
            int val = 0;
            if (UrlDictionary.Count == 0)
            {
                try
                {
                    UrlDictionary.Add(sUrl, 0);
                }
                catch (Exception)
                { }
            }
            else
            {
                if (UrlDictionary.ContainsKey(sUrl))
                {
                    val = UrlDictionary[sUrl];
                    val++;
                    UrlDictionary[sUrl] = val;
                }
                else
                {
                    try
                    {
                        UrlDictionary.Add(sUrl, 0);
                    }
                    catch(Exception)
                    { }
                }
            }
        }

        public string[] topUrl()
        {
            string[] tUrl = new string[3];
            for(int y=0;y<3;y++)
            {
                tUrl[y] = "";
            }
            Stack<string> tu = new Stack<string>();
            foreach (KeyValuePair<string, int> e in UrlDictionary.OrderBy(x => x.Value))
            {
                tu.Push(e.Key);
            }
            for (int a = 0; a < tUrl.Length; a++)
            {
                if (UrlDictionary.Count < 3)
                {
                    a = 0;
                    foreach(var u in UrlDictionary)
                    {
                        tUrl[a] = u.Key;
                            a++;
                    }
                    a = 4;
                }
                else
                {
                    string b = tu.Pop();
                    tUrl[a] = b;
                }
            }
            return tUrl;
        }

    }
}
