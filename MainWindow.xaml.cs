using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using System.Threading;
using System.Globalization;
using BenchmarkDotNet.Running;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //MessageBox.Show(string.Join(" : ", Notion_API.GetUsers().Keys));


            //Change current culture
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("tr-TR");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();

            //Get names from the API and show on the combobox
            CB_Names.ItemsSource = Names_Dict.Keys;


        }



        public Dictionary<string, string> Names_Dict = Notion_API.GetUsers();


        //Input name handling
        private void Onay_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBName.Text.Trim()) || TBName.Text.Trim() == "Please input your name and click the button.")
            {
                MessageBox.Show("Please input your name and click the button.");
            }
            else if (Amele.Person_Array.Where(x => Amele.Replacer(x) == Amele.Replacer(TBName.Text)).Count() == 0)
            {
                MessageBox.Show("The name you have written does not exist in the list. Please enter one of the names below:" + Environment.NewLine + string.Join(Environment.NewLine, Amele.Person_Array));
            }
            else
            {
                Amele.Person = Amele.Person_Array.Where(x => Amele.Replacer(x) == Amele.Replacer(TBName.Text)).ToArray()[0];
                MessageBox.Show("Person: " + Amele.Person + " welcome aboard. Please do not click the buttons unless necessary.");
            }
        }

        private void CBNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (e.AddedItems.Count > 0)
            {
                CB_Names.IsEnabled = false;
            }
        }
    }
}