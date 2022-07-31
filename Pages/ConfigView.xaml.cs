using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;
using static NotionAPI.INIT;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NotionAPI
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>




    public partial class ConfigView : Page
    {
        public ConfigView()
        {
            
            InitializeComponent();
            
            TB_APIKey.Text = Notion_Token;
            TB_NotionAPIVersion.Text = Notion_Version;
            TB_ProjectsDBURL.Text = $"https://www.notion.so/" + Notion_Projects_DataBase_ID;
            TB_DailyNotesDBURL.Text = $"https://www.notion.so/" + Notion_DailyNotes_Database_ID;
            LBL_SuccessfullSave.Visibility = Visibility.Hidden;
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
    }
}
