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
using static NotionAPI.Config;

namespace NotionAPI.Pages.Notion
{
    /// <summary>
    /// Interaction logic for WorkspaceURLs.xaml
    /// </summary>
    public partial class WorkspaceURLs : Page
    {
        public WorkspaceURLs()
        {
            InitializeComponent();
            LBL_SuccessfullSave.Visibility = Visibility.Hidden;
        }
        private Match RegexMatch;
        public void Config_Apply(object sender, RoutedEventArgs e)
        {
            RegexMatch = Regex.Match(TB_Projects_DB_ID.Text.Trim().ToLowerInvariant(), Get_DB_ID_From_URL_Regex);
            if (!string.IsNullOrEmpty(RegexMatch.Value))
            {
                Projects_DB_ID = RegexMatch.Groups["ID"].Value;
            }
            RegexMatch = Regex.Match(TB_Tasks_DB_ID.Text.Trim().ToLowerInvariant(), Get_DB_ID_From_URL_Regex);
            if (!string.IsNullOrEmpty(RegexMatch.Value))
            {
                Tasks_DB_ID = RegexMatch.Groups["ID"].Value;
            }
            RegexMatch = Regex.Match(TB_Dailynotes_DB_ID.Text.Trim().ToLowerInvariant(), Get_DB_ID_From_URL_Regex);
            if (!string.IsNullOrEmpty(RegexMatch.Value))
            {
                Dailynotes_DB_ID = RegexMatch.Groups["ID"].Value;
            }
            RegexMatch = Regex.Match(TB_Users_DB_ID.Text.Trim().ToLowerInvariant(), Get_DB_ID_From_URL_Regex);
            if (!string.IsNullOrEmpty(RegexMatch.Value))
            {
                Users_DB_ID = RegexMatch.Groups["ID"].Value;
            }
            MainWindow.Write_Config_File(JObjectify_Config_Variables());
            MainWindow.Check_Resource_Folder();
            LBL_SuccessfullSave.Visibility = Visibility.Visible;
        }
    }
}
