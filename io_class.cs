using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace twitter
{
    interface Iio
    {
        string prolog { get; set;}
        void PackupFile(DataModel dm);
        void UnpackFile();
    }
    class io_class : Iio
    {
        public string prolog { get; set; }
        private string PackageContent;
        private string rn;
        public emojiRef er;
        public io_class()
        {
            prolog = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>";
            PackageContent = "";
            rn = "\r\n";

        }

        public emojiRef initER()
        {
            er = new emojiRef();
            return er;
        }
        public void PackupFile(DataModel dm)
        {
            PackageContent = prolog + rn;
            foreach(var pi in dm.GetType().GetProperties())
            {
                PackageContent += "<" + pi.Name + ">" + dm.GetType().GetProperty(pi.Name).GetValue(dm,null) + "</" + pi.Name + ">" + rn;
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\PackageOutput.txt", PackageContent);
            PackageContent = "";
        }

        public void UnpackFile()
        {

        }
    }
}
