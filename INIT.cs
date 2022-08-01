using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Windows;
using Microsoft.Extensions.Logging;
using System.Runtime;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;

namespace NotionAPI
{
    public class NotifyClass : INotifyPropertyChanged
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class INIT
    {
        #region initializeParameters
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static NotifyClass notifyClass = new NotifyClass();
        
        public static string TimeFormat = "HH:mm:ss";

        public static SplashScreen splashScreen = new SplashScreen("Items/Loading.jpeg");

        //API Request Standard variables
        private static string _notionToken = $"Bearer secret_fUOlP7ITMCklnmcgUsiif2y8hgEMzfClguJpFLVJBZk";
        public static string Notion_Token
        {
            get
            {
                return _notionToken;
            }
            set
            {
                _notionToken = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notionVersion = "2022-02-22";
        public static string Notion_Version
        {
            get
            {
                return _notionVersion;
            }
            set
            {
                _notionVersion = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notion_Projects_Database_ID = $"3f0c889474e441ffab5de0711decb44f";
        public static string Notion_Projects_DataBase_ID
        {
            get
            {
                return _notion_Projects_Database_ID;
            }
            set
            {
                _notion_Projects_Database_ID = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notion_Tasks_DataBase_ID = $"82dbeb69d0e94f81acb2be5b581a29a1";
        public static string Notion_Tasks_DataBase_ID
        {
            get
            {
                return _notion_Tasks_DataBase_ID;
            }
            set
            {
                _notion_Tasks_DataBase_ID = value;
                notifyClass.NotifyPropertyChanged();
            }
        }
        

        private static string _notion_Query_DataBase_EndPoint = @"https://api.notion.com/v1/databases/{id}/query";
        public static string Notion_Query_DataBase_EndPoint
        {
            get
            {
                return _notion_Query_DataBase_EndPoint;
            }
            set
            {
                _notion_Query_DataBase_EndPoint = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notion_Query_DataBase_Filter_Body = string.Empty;
        public static string Notion_Query_DataBase_Filter_Body
        {
            get
            {
                return _notion_Query_DataBase_Filter_Body;
            }
            set
            {
                _notion_Query_DataBase_Filter_Body = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notion_GetUsers_EndPoint = $"https://api.notion.com/v1/users";
        public static string Notion_GetUsers_EndPoint
        {
            get
            {
                return _notion_GetUsers_EndPoint;
            }
            set
            {
                _notion_GetUsers_EndPoint = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _api_Username = "API_Test";
        public static string API_Username
        {
            get
            {
                return _api_Username;
            }
            set
            {
                _api_Username = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notion_DailyNotes_Database_ID = "";
        public static string Notion_DailyNotes_Database_ID
        {
            get
            {
                return _notion_DailyNotes_Database_ID;
            }
            set
            {
                _notion_DailyNotes_Database_ID = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string[] _project_Names = Array.Empty<string>();
        public static string[] Project_Names
        {
            get
            {
                return _project_Names;
            }
            set
            {
                _project_Names = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static string _notion_Database_ID = string.Empty;
        public static string Notion_Database_ID
        {
            get { return _notion_Database_ID; }
            set
            {
                _notion_Database_ID = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static Dictionary<string, string> notion_Users_Dictionary = new();
        public static Dictionary<string, string> Notion_Users_Dictionary
        {
            get
            {
                return notion_Users_Dictionary;
            }
            set
            {
                notion_Users_Dictionary = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static Dictionary<string, string> notion_Query_DataBase_Dictionary = new();
        public static Dictionary<string, string> Notion_Query_DataBase_Dictionary
        {
            get
            {
                return notion_Query_DataBase_Dictionary;
            }
            set
            {
                notion_Query_DataBase_Dictionary = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        public static string Notion_Get_ID_Regex_Pattern = @"(notion[.]so[/])?(?<ID>[\w]{32})([?]v[=])?$";

        private static Dictionary<string, string>  notion_Projects_Dictionary = new();
        public static Dictionary<string, string> Notion_Projects_Dictionary
        {
            get
            {
                return notion_Projects_Dictionary;
            }
            set
            {
                notion_Projects_Dictionary = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        private static Dictionary<string, string> _ntion_Tasks_Dictionary = new();
        public static Dictionary<string, string> Notion_Tasks_Dictionary
        {
            get
            {
                return _ntion_Tasks_Dictionary;
            }
            set
            {
                _ntion_Tasks_Dictionary = value;
                notifyClass.NotifyPropertyChanged();
            }
        }

        

        //public static Dictionary<string, string> Names_Dict = Notion_API.GetUsers(API_Username, Notion_GetUsers_EndPoint, Notion_Token, Notion_Version);
        //public static Dictionary<string, string> Project_Dict = new();


        public static JObject Notion_One_Line_Json = new();

# endregion

        public static string Replacer(string inputstring)
        {
            return inputstring.ToLower().Trim().Replace("ı", "i").Replace("ş", "s").Replace("ü", "u").Replace("ç", "c").Replace("ö", "o").Replace("ğ", "g");
        }
        /*
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
        */
        public static JObject Notion_Json_End(JObject in_Json)
        {
            JObject outJson = JObject.FromObject(in_Json);

            outJson["end"] = DateTime.Now.ToString(TimeFormat);

            return outJson;
        }

        public static JObject Notion_Create_Page_Json(string DatabaseID, string Aciklama, string[] Gerceklestiren, DateTime DateStart, string Property, string Location, bool Billable, string Task)
        {
            JArray PeopleJsonArray = new();

            foreach (string item in Gerceklestiren)
            {
                PeopleJsonArray.Add(new JObject { ["id"] = item });
            }
            JObject outJson = new()
            {
                ["object"] = "page",
                ["parent"] = new JObject
                {
                    ["type"] = "database_id",
                    ["database_id"] = DatabaseID
                },
                ["properties"] = new JObject
                {
                    ["Aciklama"] = new JObject
                    {
                        ["rich_text"] = new JObject
                        {
                            ["text"] = new JObject
                            {
                                ["content"] = Aciklama
                            }
                        }
                    },
                    ["Gerceklestiren"] = new JObject
                    {
                        ["people"] = PeopleJsonArray
                    },
                    ["Date"] = new JObject
                    {
                        ["date"] = new JObject
                        {
                            ["start"] = DateStart,
                            ["end"] = DateStart
                        }
                    },
                    ["Property"] = new JObject
                    {
                        ["select"] = new JObject
                        {
                            ["name"] = Property
                        }
                    },
                    ["Location"] = new JObject
                    {
                        ["select"] = new JObject
                        {
                            ["name"] = Location
                        }
                    }
                    ["Billable"] = new JObject
                    {
                        ["checkbox"] = Billable
                    },
                    ["_Tasks"] = new JObject
                    {
                        ["relation"] = new JArray(new JObject { ["id"] = Task})
                    }
                }
            };
            return outJson;
        }

        public static JObject Notion_Update_Page_Json(DateTime DateEnd)
        {
            JObject outJson = new()
            {
                ["Date"] = new JObject
                {
                    ["date"] = new JObject
                    {
                        ["end"] = DateEnd
                    }
                }
            };
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
    public class DailyNotesLine
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Gerceklestiren { get; set; }
        public string Task { get; set; }
        public string Property { get; set; }
        public bool Billable { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class Person
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
