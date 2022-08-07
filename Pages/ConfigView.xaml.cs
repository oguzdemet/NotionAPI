using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using static NotionAPI.INIT;
using static NotionAPI.Config;
using NLog;
using System;
using System.Runtime.Caching;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;

namespace NotionAPI
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>

    


    public partial class ConfigView : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ConfigView()
        {
            InitializeComponent();

            //LBL_SuccessfullSave.Visibility = Visibility.Hidden;
            TB_SetDefaultSettings.Visibility = Visibility.Hidden;
        }

        private List<DailyNotesLine> DailyNotesLines()
        {
            List<DailyNotesLine> lines = new List<DailyNotesLine>();
            lines.Add(new DailyNotesLine()
            {
                ID = 1,
                Title = "TATD-Gunluk Calisma",
                Description = "a",
                Gerceklestiren = new string[] { "Oguz Demet" },
                Task = "asdf",
                Property = "Gelistirme",
                Billable = true,
                Location = "Ev",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(2)
            });
            lines.Add(new DailyNotesLine()
            {
                ID = 2,
                Title = "TATD-Gunluk Calisma",
                Description = "b",
                Gerceklestiren = new string[] { "Oguz Demet", "Umut Yaman"},
                Task = "asdf",
                Property = "Gelistirme",
                Billable = true,
                Location = "UCGEN",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(7)
            });
            return lines;
        }

        private Match RegexMatch;
        private void Config_Apply_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Config_Apply = MessageBox.Show("Are you sure to change the config settings?", "Config Settings Change", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (Config_Apply == MessageBoxResult.OK)
            {
                /*
                MessageBox.Show("textbocx: " + general_Defaults.TB_Read_Token.Text);
                Read_Token = general_Defaults.TB_Read_Token.Text;
                MessageBox.Show("read: " + Read_Token);
                //Read_Write_Token = general_Defaults.TB_Read_Write_Token.Text;
                Notion_Version = general_Defaults.TB_Notion_Version.Text;
                
                RegexMatch = Regex.Match(general_Defaults.TB_Projects_DB_ID.Text.Trim().ToLowerInvariant(), Get_DB_ID_From_URL_Regex);
                if (!string.IsNullOrEmpty(RegexMatch.Value))
                {
                    Projects_DB_ID = RegexMatch.Groups["ID"].Value;
                }
                RegexMatch = Regex.Match(general_Defaults.TB_Dailynotes_DB_ID.Text.Trim().ToLowerInvariant(), Get_DB_ID_From_URL_Regex);
                if (!string.IsNullOrEmpty(RegexMatch.Value))
                {
                    Dailynotes_DB_ID = RegexMatch.Groups["ID"].Value;
                }
                */
                MainWindow.Write_Config_File(JObjectify_Config_Variables());
                //MainWindow.Check_Resource_Folder();
                //LBL_SuccessfullSave.Visibility = Visibility.Visible;
            }
        }
        private void Set_Default_Settings(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Config_Apply = MessageBox.Show("Are you sure to rollback to default settings?", "Config Settings Change", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (Config_Apply == MessageBoxResult.OK)
            {
                MainWindow.Write_Config_File(Default_Config);
                MainWindow.Check_Resource_Folder();
                ConfigFrame.NavigationService.Refresh();
                TB_SetDefaultSettings.Visibility = Visibility.Visible;
            }
        }

        /*
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ObjectCache cache = MemoryCache.Default;
            string fileContents = cache["filecontents"] as string;
            string asdfg = cache["asdfg"] as string;
            asdfg = "aboleyllo";
            if (fileContents == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.0);

                List<string> filePaths = new List<string>();
                filePaths.Add(@"C:\Oguz_Demet\Notion\WpfApp3\cacheText.txt");

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                // Fetch the file contents.
                fileContents = File.ReadAllText(@"C:\Oguz_Demet\Notion\WpfApp3\cacheText.txt") + "\n" + DateTime.Now.ToString();

                cache.Set("filecontents", fileContents, policy);
                cache.Set("asdfg", asdfg, policy);
            }
            TB_Cache.Text = fileContents;
        }
        */
        private void Show_API_General_Defaults(object sender, RoutedEventArgs e)
        {
            ConfigFrame.NavigationService.Navigate(new Pages.API.General_Defaults());
        }
        private void Show_API_Personal_Defaults(object sender, RoutedEventArgs e)
        {
            ConfigFrame.NavigationService.Navigate(new Pages.API.Personal_Defaults());
        }
        private void Show_Notion_JsonPath(object sender, RoutedEventArgs e)
        {
            ConfigFrame.NavigationService.Navigate(new Pages.Notion.JsonPath());
        }
        private void Show_Notion_Workspace_URL(object sender, RoutedEventArgs e)
        {
            ConfigFrame.NavigationService.Navigate(new Pages.Notion.WorkspaceURLs());
        }
        private void Show_Parsing(object sender, RoutedEventArgs e)
        {
            ConfigFrame.NavigationService.Navigate(new Pages.Parsing.Parsing());
        }
    }
}
