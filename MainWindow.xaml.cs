using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using static NotionAPI.INIT;
//using static NotionAPI.Items.Loading;

namespace NotionAPI
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("App Started");
            splashScreen.Show(false);



            try
            {



                //Change current culture
                CultureInfo culture;
                culture = CultureInfo.CreateSpecificCulture("tr-TR");

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                logger.Info("Culture info changed {culture}", culture);





                if (Names_Dict.Keys.Count == 0)
                {
                    this.Close();
                    logger.Warn("Application has been closed due to users not being initialized.");
                }

                InitializeComponent();

                logger.Info("Component initialized");

                //Get names from the API and show on the combobox
                CB_Names.ItemsSource = Names_Dict.Keys;


                CB_Types.ItemsSource = Types_Array;



            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                splashScreen.Close(TimeSpan.FromSeconds(1));
            }
        }

        //Input name handling
        private void Onay_Click(object sender, RoutedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Onay_Click");
            try
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
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Start_Click");
            try
            {
                CB_ProjectNames.IsEnabled = false;
                CB_Types.IsEnabled = false;
                Notion_One_Line_Json = JObject.FromObject(Notion_Json_Template("", "", ""));
                MessageBox.Show(JsonConvert.SerializeObject(Notion_One_Line_Json));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Submit_Click");
            try
            {
                MessageBox.Show(JsonConvert.SerializeObject(Notion_Json_End(Notion_One_Line_Json)));
                CB_ProjectNames.IsEnabled = true;
                CB_Types.IsEnabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        private void CBNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("CBNamesSelectionChanged");
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    splashScreen.Show(false);
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
                        ["or"] = new JArray() { j2, j3 }
                    };

                    Project_Dict = Projects.GetProjects(API_Username, Notion_Query_DataBase_EndPoint, Notion_Token, Notion_Projects_DataBase_ID, Notion_Version, Notion_Query_DataBase_Filter_Body);
                    CB_ProjectNames.ItemsSource = Project_Dict.Keys;
                    splashScreen.Close(TimeSpan.FromSeconds(1));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        private void CBProjectsNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("CBNamesSelectionChanged");
            try
            {

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void Page_Click(object sender, RoutedEventArgs e)
        {
            var config = new ConfigView();
            config.BeginInit();
            config.InitializeComponent();
        }
    }
}