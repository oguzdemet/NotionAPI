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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static NotionAPI.INIT;
//using static NotionAPI.Items.Loading;

namespace NotionAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            splashScreen.Show(true);
            
            MessageBox.Show("sa");

            //MessageBox.Show(string.Join(" : ", Notion_API.GetUsers().Keys));


            //Change current culture
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("tr-TR");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();

            //Get names from the API and show on the combobox
            CB_Names.ItemsSource = Names_Dict.Keys;
            
            CB_Types.ItemsSource = Types_Array;

            splashScreen.Close(TimeSpan.FromSeconds(1));

            
        }

        

        //Input name handling
        private void Onay_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBName.Text.Trim()) || TBName.Text.Trim() == "Please input your name and click the button.")
            {
                MessageBox.Show("Please input your name and click the button.");
            }
            else if (Names_Dict.Keys.Where(x => Replacer(x) == Replacer(TBName.Text)).Count() == 0)
            {
                MessageBox.Show("The name you have written does not exist in the list. Please enter one of the names below:" + Environment.NewLine + string.Join(Environment.NewLine, Names_Dict.Keys));
            }
            else
            {
                //Amele.Person = Person_Array.Where(x => Replacer(x) == Replacer(TBName.Text)).ToArray()[0];
                
                //MessageBox.Show("Person: " + Amele.Person + " welcome aboard. Please do not click the buttons unless necessary.");
            }

        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            CB_ProjectNames.IsEnabled = false;
            CB_Types.IsEnabled = false;
            Notion_One_Line_Json = JObject.FromObject(Notion_Json_Template("", "", ""));
            MessageBox.Show(JsonConvert.SerializeObject(Notion_One_Line_Json));
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(JsonConvert.SerializeObject(Notion_Json_End(Notion_One_Line_Json)));
            CB_ProjectNames.IsEnabled = true;
            CB_Types.IsEnabled = true;
        }

        private void CBNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                splashScreen.Show(true);
                CB_Names.IsEnabled = false;
                
                Current_Person.name = e.AddedItems[0].ToString();
                MessageBox.Show(Current_Person.name + " Hoşgelmişsiniz");
                Current_Person.id = Names_Dict[Current_Person.name];
                
                JObject Notion_Query_DataBase_Filter_Body = new JObject();
                JObject j2 = new JObject();
                JObject j3 = new JObject();

                j2["property"] = "Project Manager";
                j2["people"] = new JObject
                {
                    ["contains"] = Current_Person.id
                };

                j3["people"] = new JObject
                {
                    ["contains"] = Current_Person.id
                };
                j3["property"] = "Team";

                Notion_Query_DataBase_Filter_Body["filter"] = new JObject
                {
                    ["or"] = new JArray() {j2, j3}
                };

                Project_Dict = Projects.GetProjects(API_Username, Notion_Query_DataBase_EndPoint, Notion_Token, Notion_Projects_DataBase_ID, Notion_Version, Notion_Query_DataBase_Filter_Body);
                CB_ProjectNames.ItemsSource = Project_Dict.Keys;
                splashScreen.Close(TimeSpan.FromSeconds(1));
            }
        }
        private void CBProjectsNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}