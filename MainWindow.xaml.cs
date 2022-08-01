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
using System.Windows.Navigation;
//using static NotionAPI.Items.Loading;

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
                //Change current culture
                //CultureInfo culture;
                //culture = CultureInfo.CreateSpecificCulture("tr-TR");

                //Thread.CurrentThread.CurrentCulture = culture;
                //Thread.CurrentThread.CurrentUICulture = culture;

                //logger.Info("Culture info changed {culture}", culture);
                
                InitializeComponent();

                logger.Info("Component initialized");

                //Get names from the API and show on the combobox
                //CB_Names.ItemsSource = Names_Dict.Keys;


                //CB_Types.ItemsSource = Types_Array;

                API_Requests.Notion_Get_Users();
                

                Notion_Database_ID = Notion_Projects_DataBase_ID;
                API_Requests.Notion_Query_DataBase();
                Notion_Projects_Dictionary = Notion_Query_DataBase_Dictionary;

                

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
            //Main.NavigationService.Navigate(new InputPage());
            Main.NavigationService.Content = new InputPage().Content;
            if (Notion_Users_Dictionary.Keys.Count == 0)
            {
                Close();
                logger.Warn("Application has been closed due to users not being initialized.");
            }
        }
        private void Config_Page_Click(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new ConfigView());
        }
    }
}