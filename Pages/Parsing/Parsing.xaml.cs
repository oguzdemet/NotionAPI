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
using static NotionAPI.Config;

namespace NotionAPI.Pages.Parsing
{
    /// <summary>
    /// Interaction logic for Parsing.xaml
    /// </summary>
    public partial class Parsing : Page
    {
        public Parsing()
        {
            InitializeComponent();
            LBL_SuccessfullSave.Visibility = Visibility.Hidden;
        }
        public void Config_Apply(object sender, RoutedEventArgs e)
        {
            Get_DB_ID_From_URL_Regex = TB_Get_DB_ID_From_URL_Regex.Text;

            MainWindow.Write_Config_File(JObjectify_Config_Variables());
            MainWindow.Check_Resource_Folder();
            LBL_SuccessfullSave.Visibility = Visibility.Visible;
        }
    }
}
