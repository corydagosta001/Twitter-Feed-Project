using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;


namespace twitter
{
    class HashtagProcess
    {
        Dictionary<string, int> HashDictionary { get; set; }
        public HashtagProcess()
        {
            HashDictionary = new Dictionary<string, int>();
        }


        public void HashProcess(string text)
        {
            string[] textSplit = text.Split(' ');
            foreach (string a in textSplit)
            {
                if (a.Substring(0, 1) == "#")
                {
                    addToDictionary(a);
                }
            }
        }


        public string HashProcess(string text, Boolean returnString)
        {
            string[] textSplit = text.Split(' ');
            foreach(string a in textSplit)
            {
                if (a != "")
                {
                    if (a.Substring(0, 1) == "#")
                    {
                        addToDictionary(a);
                    }
                }
            }
            return top3Hashtags();
        }

        private void addToDictionary(string hashTag)
        {
            int val = 0;
            if (HashDictionary.Count == 0)
            {
                try
                {
                    HashDictionary.Add(hashTag, 0);
                }
                catch (Exception){}
            }
            else
            {
                if (HashDictionary.ContainsKey(hashTag))
                {
                    val = HashDictionary[hashTag];
                    val++;
                    HashDictionary[hashTag] = val;
                }
                else
                {
                    try
                    {
                        HashDictionary.Add(hashTag, 0);
                    }
                    catch(Exception){}
                }
            }
        }

        private string top3Hashtags()
        {
            string top3 = "\r\n";
            Stack<string> th = new Stack<string>();
            try
            {
                foreach (KeyValuePair<string, int> e in HashDictionary.OrderBy(x => x.Value))
                {
                    th.Push(e.Key);
                }


                for (int a = 0; a < 3; a++)
                {
                    if (HashDictionary.Count < 3)
                    {
                        a = 0;
                        foreach (var u in HashDictionary)
                        {
                            top3 += "    " + u.Key + "\r\n";
                            a++;
                        }
                        a = 4;
                    }
                    else
                    {
                        top3 += "    " + th.Pop() + "\r\n";
                    }
                }
            }
            catch (Exception)
            {

            }
            if (top3.Length < 4)
            {
                return "";
            }
            else
            {
                return top3.Substring(0, top3.Length - 4);
            }
        }
    }
}
