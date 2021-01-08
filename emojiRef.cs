using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace twitter
{
    interface Iemoji
    {
        Dictionary<string, string> emojiName {get; set;}
        Dictionary<string, int> emojiTally { get; set; }
        void initJ();
    }

    class emojiRef : Iemoji
    {
        public Dictionary<string, string> emojiName { get; set; }
        public Dictionary<string, int> emojiTally { get; set; }
        private string path;
        
        public emojiRef()
        {
            path = Directory.GetCurrentDirectory();
            if (File.Exists(path + "\\emoji.json"))
            {
                initJ();
            }
            else
            {
                MessageBox.Show("emoji.json must be present in the current Directory\r\n" + path +
                    "\r\nClick 'ok' to close the program.");
                Application.Current.Shutdown();
            }
        }
                    public void initJ()
                    {
                        List<string> l = new List<string>();
                        //File.WriteAllText(Directory.GetCurrentDirectory() + "\\PackageOutput.txt", lContent);


                        emojiName = new Dictionary<string, string>();
                        emojiTally = new Dictionary<string, int>();
                        int intStart0, intStart1, intStart2, intStart3;
                        Boolean once = false;
                        Boolean isThere;
                        List<string> compare = new List<string>();
                        string temp = "";
                        string temp1 = "";
                        string temp2 = "";
                        string nameCond = "{\"name\":\"";
                        string cond = "\"unified\":\"";
                        string strContent = File.ReadAllText(path + "\\emoji.json");
                        intStart1 = 0;
                        while (intStart1 > -1)
                        {
                            intStart1 = strContent.IndexOf(nameCond, intStart1);
                            if (intStart1 > -1)
                            {
                                intStart1 += nameCond.Length;
                                intStart2 = strContent.IndexOf("\"", intStart1);
                                try
                                {
                                    temp = strContent.Substring(intStart1, intStart2 - intStart1);
                                    intStart3 = strContent.IndexOf(nameCond, intStart1);
                                    intStart3 = intStart3 == -1 ? intStart3 = strContent.Length : intStart3;
                                    temp2 = strContent.Substring(intStart1, intStart3 - intStart1);
                                    intStart0 = 0;
                                    while(intStart0 > -1)
                                    {
                                        intStart0 = temp2.IndexOf(cond, intStart0);
                                        if(intStart0 > -1)
                                        {
                                            intStart0 += cond.Length;
                                            intStart2 = temp2.IndexOf("\"", intStart0);
                                            try
                                            {
                                                temp1 = temp2.Substring(intStart0, intStart2 - intStart0).Replace("-", "");
                                                emojiName.Add(temp1,temp);
                                                emojiTally.Add(temp1, 0);
                                            }
                                            catch(Exception)
                                            {

                                                MessageBox.Show("Something is wrong in the sub text format of the json file. Click 'ok' to close the program.\r\n" + temp1);
                                                intStart0 = -1;
                                                intStart1 = -1;
                                                Application.Current.Shutdown();

                                            }
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    MessageBox.Show("Something is wrong in the format of the json file. Click 'ok' to close the program.");
                                    intStart1 = -1;
                                    Application.Current.Shutdown();
                                }
                            }
                        }
                    }
        }
}
