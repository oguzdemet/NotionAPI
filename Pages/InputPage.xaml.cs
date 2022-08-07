using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using static NotionAPI.INIT;
using static NotionAPI.Config;
using System.Collections.Generic;

namespace NotionAPI
{
    public partial class InputPage : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public InputPage()
        {
            logger.Info("Initializing Input Page");
            InitializeComponent();

            //Get user list via querying Users DB
            Notion_Database_ID = Users_DB_ID;
            API_Requests.Notion_Query_DataBase("People");
            Notion_Users_Dictionary = Notion_Query_DataBase_Dictionary;
            CB_Person.ItemsSource = Notion_Users_Dictionary.Keys;

            //Get Projects list via querying Projects DB
            Notion_Database_ID = Projects_DB_ID;
            API_Requests.Notion_Query_DataBase("Text");
            Notion_Projects_Dictionary = Notion_Query_DataBase_Dictionary;
            CB_ProjectNames.ItemsSource = Notion_Projects_Dictionary.Keys;

            //Get properties from get database(dailynotes)
            Notion_Database_ID = Dailynotes_DB_ID;
            Notion_GetDataBase_JsonPath = Notion_Get_Properties_JsonPath;
            API_Requests.Notion_Get_Database();
            Notion_Properties_Dictionary = Notion_Database_Dictionary;
            CB_Property.ItemsSource = Notion_Properties_Dictionary.Keys;

            //Get properties from get database(dailynotes)
            Notion_Database_ID = Dailynotes_DB_ID;
            Notion_GetDataBase_JsonPath = Notion_Get_Locations_JsonPath;
            API_Requests.Notion_Get_Database();
            Notion_Locations_Dictionary = Notion_Database_Dictionary;
            CB_Location.ItemsSource = Notion_Locations_Dictionary.Keys;




        }
        private List<DailyNotesLine> DailyNotesLines()
        {
            List<DailyNotesLine> lines = new List<DailyNotesLine>
            {
                new DailyNotesLine()
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
                },
                new DailyNotesLine()
                {
                    ID = 2,
                    Title = "TATD-Gunluk Calisma",
                    Description = "b",
                    Gerceklestiren = new string[] { "Oguz Demet", "Umut Yaman" },
                    Task = "asdf",
                    Property = "Gelistirme",
                    Billable = true,
                    Location = "UCGEN",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMinutes(7)
                }
            };
            return lines;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Start_Click");
            try
            {
                CB_ProjectNames.IsEnabled = false;
                CB_Property.IsEnabled = false;
                CB_Location.IsEnabled = false;
                CB_Person.IsEnabled = false;
                CB_Task.IsEnabled = false;
                Notion_Create_Page_Body = JsonConvert.SerializeObject(Notion_Create_Page_Json(
                 Dailynotes_DB_ID,
                 TB_Title.Text,
                 TB_Notes.Text,
                 new string[] { CB_Person.Text },
                 DateTime.Now,
                 CB_Property.Text,
                 CB_Location.Text,
                 (bool)CHB_Billable.IsChecked,
                 Notion_Tasks_Dictionary[CB_Task.Text])
                );

                /*

                Notion_One_Line_Json = JObject.FromObject(Notion_Json_Template("", "", ""));
                MessageBox.Show(JsonConvert.SerializeObject(Notion_One_Line_Json));
                */
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
                CB_Property.IsEnabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }
        /*
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
        */
        private void CBProjectsNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("CBProjectNamesSelectionChanged");
            try
            {
                // GET TASKS FILTERED
                Notion_Database_ID = Tasks_DB_ID;
                Notion_Query_DataBase_Filter_Body = JsonConvert.SerializeObject(new JObject
                {
                    ["filter"] = new JObject
                    {
                        ["property"] = "PROJECTS",
                        ["relation"] = new JObject
                        {
                            ["contains"] = Notion_Projects_Dictionary[CB_ProjectNames.SelectedValue.ToString()]
                        }
                    }
                });
                API_Requests.Notion_Query_DataBase("Text");
                Notion_Tasks_Dictionary = Notion_Query_DataBase_Dictionary;
                logger.Info("Notion_Tasks_Dictionary has been changed", string.Join(", ", Notion_Tasks_Dictionary.Keys));
                CB_Task.ItemsSource = Notion_Tasks_Dictionary.Keys;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }

}
