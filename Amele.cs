using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json.Linq;
namespace WpfApp3
{
    public class Amele
    {
        public static Person Current_Person = new();
        public static string TimeFormat = "HH:mm:ss";      
        
        //API Request Standard variables
        public static string Notion_Token = $"Bearer secret_fUOlP7ITMCklnmcgUsiif2y8hgEMzfClguJpFLVJBZk";
        public static string Notion_Version = "2022-02-22";
        public static string Notion_Projects_DataBase_ID = $"3f0c889474e441ffab5de0711decb44f";
        public static string Notion_Query_DataBase_EndPoint = @"https://api.notion.com/v1/databases/{id}/query";
        public static string Notion_GetUsers_EndPoint = $"https://api.notion.com/v1/users";
        public static string API_Username = "API_Test";
        
        public static Dictionary<string, string> Names_Dict = Notion_API.GetUsers(API_Username, Notion_GetUsers_EndPoint, Notion_Token, Notion_Version);
        public static Dictionary<string, string> Project_Dict = new Dictionary<string, string>();
        public static string[] Types_Array = new string[] { "Gelistirme", "ARCK-01", "CIMT-01", "Rukneddin" };

        public static string Replacer(string a)
        {
            return a.ToLower().Trim().Replace("ı", "i").Replace("ş", "s").Replace("ü", "u").Replace("ç", "c").Replace("ö", "o").Replace("ğ", "g");
        }
    }
}
