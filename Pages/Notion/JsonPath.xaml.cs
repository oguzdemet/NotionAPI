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
    /// Interaction logic for JsonPath.xaml
    /// </summary>
    public partial class JsonPath : Page
    {
        public JsonPath()
        {
            InitializeComponent();
            LBL_SuccessfullSave.Visibility = Visibility.Hidden;
        }
        private Match RegexMatch;
        public void Config_Apply(object sender, RoutedEventArgs e)
        {
            Notion_Get_Properties_JsonPath = TB_Notion_Get_Properties_JsonPath.Text;
            Notion_GetDataBase_JsonPath = TB_Notion_GetDataBase_JsonPath.Text;

            MainWindow.Write_Config_File(JObjectify_Config_Variables());
            MainWindow.Check_Resource_Folder();
            LBL_SuccessfullSave.Visibility = Visibility.Visible;
        }
    }
}
