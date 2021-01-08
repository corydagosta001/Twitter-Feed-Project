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

//using Tweetinvi.Models;

using System.Diagnostics;

using System.Net.Http.Headers;


using System.Configuration;

namespace twitter
{
    
    public partial class MainWindow : Window
    {

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TwitterFeedDriver tfd = new TwitterFeedDriver();
            tfd.go = false;
            tfd.StopStream.IsEnabled = false;
            tfd.StreamData.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TwitterFeedDriver tfd = new TwitterFeedDriver();
            tfd.StartStreaming();
            tfd.StopStream.IsEnabled = true;
            tfd.StreamData.IsEnabled = false;
        }

    }


}
