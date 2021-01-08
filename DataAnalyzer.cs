using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
namespace twitter
{


    class DataAnalyzer
    {
        public List<string> urlList = new List<string>();
        public string TextStrip(string text)
        {
            int intStart1;
            string StrippedText = text;
            intStart1 = StrippedText.IndexOf("\"text\":\"") + 8;
            try
            {
                StrippedText = StrippedText.Substring(intStart1, text.Length - intStart1).Replace("\"}}", "");
            }
            catch(Exception)
            {
                //pass through;
            }
            StrippedText = StrippedText.Replace("\\n", " ");
            return StrippedText;
        }

        public int SearchForPicUrl(string text)
        {
            if(text.IndexOf("entities[") > -1 || text.IndexOf("extended_entities[") > -1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int mentionedWord(string text, string searchWord)
        {
            int fCount = 0;
            string t = text;
            t = t.ToLower();
            searchWord = searchWord.ToLower();
            int intStart1 = 0;

            while (intStart1 > -1)
            {
                 intStart1 = t.IndexOf(searchWord, intStart1);
                 if (intStart1 > -1)
                 {
                     fCount++;
                     intStart1 += 4;
                 }
            }
            return fCount;
        }

        public string UniCodeConvert(string text)
        {
            //byte[] byteText = Encoding.Unicode.GetBytes(text);
            //var hexString = BitConverter.ToString(byteText);
            char[] t = text.ToCharArray();
            string hexOutput = "";
            foreach (char letter in t)
            {
                int value1 = Convert.ToInt32(letter);
                hexOutput += String.Format("{0:X}", value1);
            }
            return hexOutput.Replace("20", " ").Replace("-", "");
        }


        public Tuple<Dictionary<string,int>, string,int> tallyEmoji(string text, Dictionary<string, int> ed,
            Dictionary<string,string> en, int totalTweets)
        {
            int emojiThere = 0;
            text = UniCodeConvert(text);
            int val;
            string output = "";
            try
            {
                foreach (var d in ed)
                {
                    if (text.IndexOf(d.Key) > -1)
                    {
                        emojiThere++;
                        val = ed[d.Key];
                        val++;
                        ed[d.Key] = val;
                    }
                }
            }
            catch (Exception) { }
            return new Tuple<Dictionary<string, int>, string,int>(ed,output,emojiThere);
        }


        public Tuple<Dictionary<string, int>, string, int> tallyEmoji_V(string text, Dictionary<string, int> ed,
            Dictionary<string, string> en, int totalTweets)
        {
            int emojiThere = 0;
            text = UniCodeConvert(text);
            string[] tArr = text.Split(' ');
            int val;
            string output = "";
            foreach (string a in tArr)
            {
                if (ed.ContainsKey(a))
                {
                    emojiThere++;
                    val = ed.FirstOrDefault(x => ed.ContainsKey(a)).Value;
                    val = ed[a];
                    val++;
                    ed[a] = val;
                    output += en[a] + " ::: " + a + " ::: " + ed[a] + "\r\n";
                }
            }
            return new Tuple<Dictionary<string, int>, string, int>(ed, output, emojiThere);
        }

        public string[] topEmojis(Dictionary<string,string> emojiName, Dictionary<string,int> emojiTally)
        {
            string[] tEmojis = new string[3];
            Stack<string> te = new Stack<string>();
            string record = "";
            foreach (KeyValuePair<string, int> e in emojiTally.OrderBy(x => x.Value))
            {
                te.Push(e.Key);
            }
            for (int a=0;a<tEmojis.Length;a++)
            {
                string b = te.Pop();
                tEmojis[a] = emojiName[b];
            }
            return tEmojis;
        }

    }
}
