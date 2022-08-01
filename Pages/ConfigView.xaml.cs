using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using static NotionAPI.INIT;
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

            /*
            TB_APIKey.Text = Notion_Token;
            TB_NotionAPIVersion.Text = Notion_Version;
            TB_ProjectsDBURL.Text = $"https://www.notion.so/" + Notion_Projects_DataBase_ID;
            TB_DailyNotesDBURL.Text = $"https://www.notion.so/" + Notion_DailyNotes_Database_ID;
            
            
            */
            LBL_SuccessfullSave.Visibility = Visibility.Hidden;
            //DG_Config.ItemsSource = DailyNotesLines();
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
                Notion_Token = TB_APIKey.Text;
                Notion_Version = TB_NotionAPIVersion.Text;
                
                RegexMatch = Regex.Match(TB_ProjectsDBURL.Text.Trim().ToLowerInvariant(), Notion_Get_ID_Regex_Pattern);
                if (!string.IsNullOrEmpty(RegexMatch.Value))
                {
                    Notion_Projects_DataBase_ID = RegexMatch.Groups["ID"].Value;
                }
                RegexMatch = Regex.Match(TB_DailyNotesDBURL.Text.Trim().ToLowerInvariant(), Notion_Get_ID_Regex_Pattern);
                if (!string.IsNullOrEmpty(RegexMatch.Value))
                {
                    Notion_DailyNotes_Database_ID = RegexMatch.Groups["ID"].Value;
                }
            }
            LBL_SuccessfullSave.Visibility = Visibility.Visible;
        }
        
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
        
    }
}
