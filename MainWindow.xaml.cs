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
using static NotionAPI.Config;
using System.Windows.Navigation;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Schema;

namespace NotionAPI
{

    public partial class MainWindow : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public MainWindow()
        {
            logger.Info("App Started");
            splashScreen.Show(false);

            try
            {
                // Get default values and store
                Default_Config = JObjectify_Config_Variables();

                // Check config file
                Check_Resource_Folder();

                //Change current culture
                //CultureInfo culture;
                //culture = CultureInfo.CreateSpecificCulture("tr-TR");
                //Thread.CurrentThread.CurrentCulture = culture;
                //Thread.CurrentThread.CurrentUICulture = culture;
                //logger.Info("Culture info changed {culture}", culture);

                InitializeComponent();
                logger.Info("Component initialized");

                // Get Users disabled. Because if there are guests, you cannot get them.
                // In order to get all user list;
                // Created a database where all rows are a user (guest or not).
                // And I am querying this DB.
                // Remaining is the same.
                //API_Requests.Notion_Get_Users();


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
        /*
        private void Onay_Click(object sender, RoutedEventArgs e)
        {
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
        */

        
        private void Main_Page_Click(object sender, RoutedEventArgs e)
        {
            if (Notion_Users_Dictionary.Keys.Count != 0)
            {
                logger.Warn("Navigating to config page. Due to users cannot be initialized.");
                MessageBox.Show("Navigating to config page. Due to users cannot be initialized.");
                MainPageButton.IsEnabled = true;
                ConfigPageButton.IsEnabled = false;
                Main.NavigationService.Navigate(new ConfigView());
            }
            else
            {
                Main.NavigationService.Navigate(new InputPage());
                //Main.NavigationService.Content = new InputPage().Content;
                MainPageButton.IsEnabled = false;
                ConfigPageButton.IsEnabled = true;
            }
        }
        public void Config_Page_Click(object sender, RoutedEventArgs e)
        {
            MainPageButton.IsEnabled = true;
            ConfigPageButton.IsEnabled = false;
            Main.NavigationService.Navigate(new ConfigView());
        }
        public static void Check_Resource_Folder()
        {
            if (!Directory.Exists(LocalAppPath))
            {
                Directory.CreateDirectory(LocalAppPath);
                logger.Info("App directory has been created to: " + LocalAppPath);

                File.Create(ConfigPath);
                logger.Info("Config file has been created.");

                Write_Config_File(JObjectify_Config_Variables());
                logger.Info("Config file has been adjusted");

                //File.Create(CachePath);
                //logger.Info("Cache file has been created.");
            }
            else
            {
                try
                {
                    //Validate Config file
                    string json = File.ReadAllText(ConfigPath);
                    dynamic jsonObj = JsonConvert.DeserializeObject(json);
                    Read_Token = jsonObj["Read_Token"];
                    Read_Write_Token = jsonObj["Read_Write_Token"];
                    Notion_Version = jsonObj["Notion_Version"];
                    Query_DB_Endpoint = jsonObj["Query_DB_Endpoint"];
                    GetUsers_Endpoint = jsonObj["GetUsers_Endpoint"];
                    Get_DB_Endpoint = jsonObj["Get_DB_Endpoint"];
                    Get_Page_Endpoint = jsonObj["Get_Page_Endpoint"];
                    Create_Page_Endpoint = jsonObj["Create_Page_Endpoint"];
                    Update_Page_Endpoint = jsonObj["Update_Page_Endpoint"];
                    Projects_DB_ID = jsonObj["Projects_DB_ID"];
                    Tasks_DB_ID = jsonObj["Tasks_DB_ID"];
                    Dailynotes_DB_ID = jsonObj["Dailynotes_DB_ID"];
                    Users_DB_ID = jsonObj["Users_DB_ID"];
                    Get_DB_ID_From_URL_Regex = jsonObj["Get_DB_ID_From_URL_Regex"];
                }
                catch (Exception ex)
                {
                    logger.Error("Config parse error! " + ex.Message + Environment.NewLine + ex.Source);
                    MessageBox.Show("Config json is not valid. Please check the config page." + Environment.NewLine + "If config file is unrecoverable, you may delete the file.", "WARN", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        public static void Write_Config_File(JObject in_JObject)
        {           
            //write string to file
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(in_JObject));
        }
    }
}