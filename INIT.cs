using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Windows;
using Microsoft.Extensions.Logging;
using System.Runtime;
using System.Runtime.InteropServices;

namespace NotionAPI
{
    public class INIT
    {
# region initializeParameters
        public static Person Current_Person = new();
        public static string TimeFormat = "HH:mm:ss";

        public static SplashScreen splashScreen = new SplashScreen("Items/Loading.jpeg");

        //API Request Standard variables
        public static string Notion_Token = $"Bearer secret_fUOlP7ITMCklnmcgUsiif2y8hgEMzfClguJpFLVJBZk";
        public static string Notion_Version = "2022-02-22";
        public static string Notion_Projects_DataBase_ID = $"3f0c889474e441ffab5de0711decb44f";
        public static string Notion_Query_DataBase_EndPoint = @"https://api.notion.com/v1/databases/{id}/query";
        public static string Notion_GetUsers_EndPoint = $"https://api.notion.com/v1/users";
        public static string API_Username = "API_Test";

        public static Dictionary<string, string> Names_Dict = Notion_API.GetUsers(API_Username, Notion_GetUsers_EndPoint, Notion_Token, Notion_Version);
        public static Dictionary<string, string> Project_Dict = new();
        public static string[] Types_Array = new string[] { "Gelistirme", "ARCK-01", "CIMT-01", "Rukneddin" };

        public static JObject Notion_One_Line_Json = new();

# endregion

        public static string Replacer(string a)
        {
            return a.ToLower().Trim().Replace("ı", "i").Replace("ş", "s").Replace("ü", "u").Replace("ç", "c").Replace("ö", "o").Replace("ğ", "g");
        }

        public static JObject Notion_Json_Template(string Person_ID, string Project_ID, string Type)
        {
            JObject outJson = new()
            {
                ["person_id"] = Current_Person.id,
                ["project_id"] = Current_Person.name,
                ["start"] = DateTime.Now.ToString(TimeFormat)
            };

            return outJson;
        }
        public static JObject Notion_Json_End(JObject in_Json)
        {
            JObject outJson = JObject.FromObject(in_Json);

            outJson["end"] = DateTime.Now.ToString(TimeFormat);

            return outJson;
        }
    }
    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogDebug(20, "Doing hard work! {Action}", name);
        }
    }
    public class InternetAvailability
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
    }

}
