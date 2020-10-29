using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiscordWebhookControlUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private static EmbedConfigurationPage embedConfigurationPage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_ReqNavigation(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private async void SendWebhook(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Are you sure send this message via webhook?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (res != MessageBoxResult.Yes) return;
            Dictionary<string, string> values;
            if (Webhook_CustomNameOption.IsChecked.Value) {
                values = new Dictionary<string, string>
                {
                    { "username", Webhook_CustomName.Text },
                    { "content", WebhookContent.Text }
                };
            } else {
                values = new Dictionary<string, string>
                {
                    { "content", WebhookContent.Text }
                };
            }

            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = await client.PostAsync(WebhookURL.Text, content);
                WebhookStatus.Text = "Sent " + (response.IsSuccessStatusCode ? "successful!" : "with error!");
            } catch (Exception)
            {
                MessageBox.Show("An unexpected error occured!\n" + e.ToString(), "Oops!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void PerformSaving(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WebhookSettings settings = new WebhookSettings();
            settings.url = WebhookURL.Text;
            settings.content = WebhookContent.Text;
            settings.custom_name_enabled = Webhook_CustomNameOption.IsChecked.Value;
            settings.custom_name = Webhook_CustomName.Text;
            System.IO.File.WriteAllText(Environment.GetEnvironmentVariable("APPDATA")+@"\DiscordWebhookControl.config.json", JsonConvert.SerializeObject(settings));
        }

        private void PerformLoad(object sender, EventArgs e)
        {
            try
            {
                var data = System.IO.File.ReadAllText(Environment.GetEnvironmentVariable("APPDATA") + @"\DiscordWebhookControl.config.json");
                WebhookSettings settings = JsonConvert.DeserializeObject<WebhookSettings>(data);
                WebhookURL.Text = settings.url;
                WebhookContent.Text = settings.content;
                Webhook_CustomNameOption.IsChecked = settings.custom_name_enabled;
                Webhook_CustomName.Text = settings.custom_name;
            } catch (Exception) { }
        }

        private void ShowEmbedsDialog(object sender, RoutedEventArgs e)
        {
            embedConfigurationPage = new EmbedConfigurationPage();
            embedConfigurationPage.Owner = this;
            embedConfigurationPage.ShowDialog();
        }
    }
}
