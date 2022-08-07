using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static NotionAPI.INIT;
using static NotionAPI.Config;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace NotionAPI.Pages.API
{
    /// <summary>
    /// Interaction logic for General_Defaults.xaml
    /// </summary>
    public partial class General_Defaults : Page
    {
        public General_Defaults()
        {
            
            InitializeComponent();
            LBL_SuccessfullSave.Visibility = Visibility.Hidden;
        }

        private Match RegexMatch;
        public void Config_Apply(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("config apply: " + TB_Read_Token.Text);
            Read_Token = TB_Read_Token.Text;
            Notion_Version = TB_Notion_Version.Text;
            Query_DB_Endpoint = TB_Query_DB_Endpoint.Text;
            GetUsers_Endpoint = TB_GetUsers_Endpoint.Text;
            Get_DB_Endpoint = TB_Get_DB_Endpoint.Text;
            Get_Page_Endpoint = TB_Get_Page_Endpoint.Text;
            Create_Page_Endpoint = TB_Create_Page_Endpoint.Text;
            Update_Page_Endpoint = TB_Update_Page_Endpoint.Text;
            MainWindow.Write_Config_File(JObjectify_Config_Variables());
            MainWindow.Check_Resource_Folder();
            LBL_SuccessfullSave.Visibility = Visibility.Visible;
        }
    }
}
