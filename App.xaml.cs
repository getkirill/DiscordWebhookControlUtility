using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace DiscordWebhookControlUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public App()
        //{
            //MessageBox.Show(Environment.GetEnvironmentVariable("APPDATA"));
            //string text = System.IO.File.ReadAllText();
        //}
    }
    public class WebhookSettings
    {
        public string url;
        public bool custom_name_enabled;
        public string custom_name;
        public string content;
    }
}
