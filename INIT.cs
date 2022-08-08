using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows;
using Microsoft.Extensions.Logging;
using System.Runtime;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;
using System.IO;
using System.Reflection;

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
        public static string LocalAppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetCallingAssembly().GetName().Name);
        public static string ConfigPath = Path.Combine(LocalAppPath, "Config.json");
        public static string CachePath = Path.Combine(LocalAppPath, "Cache.json");
        public static SplashScreen splashScreen = new SplashScreen("Items/Loading.jpeg");

        //API Request variables
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
                notifyClass.NotifyPropertyChanged(nameof(Notion_Query_DataBase_Filter_Body));
            }
        }

        private static string _notion_Create_Page_Body = string.Empty;
        public static string Notion_Create_Page_Body
        {
            get
            {
                return _notion_Create_Page_Body;
            }
            set
            {
                _notion_Create_Page_Body = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Create_Page_Body));
            }
        }

        private static string _notion_Update_Page_Body = string.Empty;
        public static string Notion_Update_Page_Body
        {
            get
            {
                return _notion_Update_Page_Body;
            }
            set
            {
                _notion_Update_Page_Body = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Update_Page_Body));
            }
        }

        private static string _notion_Database_ID = string.Empty;
        public static string Notion_Database_ID
        {
            get { return _notion_Database_ID; }
            set
            {
                _notion_Database_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Database_ID));
            }
        }

        private static string _notion_Update_Page_ID = string.Empty;
        public static string Notion_Update_Page_ID
        {
            get { return _notion_Update_Page_ID; }
            set
            {
                _notion_Update_Page_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Update_Page_ID));
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
                notifyClass.NotifyPropertyChanged(nameof(Notion_Users_Dictionary));
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
                notifyClass.NotifyPropertyChanged(nameof(Notion_Query_DataBase_Dictionary));
            }
        }

        private static Dictionary<string, string> notion_Database_Dictionary = new();
        public static Dictionary<string, string> Notion_Database_Dictionary
        {
            get
            {
                return notion_Database_Dictionary;
            }
            set
            {
                notion_Database_Dictionary = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Database_Dictionary));
            }
        }

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
                notifyClass.NotifyPropertyChanged(nameof(Notion_Projects_Dictionary));
            }
        }

        private static Dictionary<string, string> _notion_Tasks_Dictionary = new();
        public static Dictionary<string, string> Notion_Tasks_Dictionary
        {
            get
            {
                return _notion_Tasks_Dictionary;
            }
            set
            {
                _notion_Tasks_Dictionary = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Tasks_Dictionary));
            }
        }

        private static Dictionary<string, string> _notion_Properties_Dictionary = new();
        public static Dictionary<string, string> Notion_Properties_Dictionary
        {
            get
            {
                return _notion_Properties_Dictionary;
            }
            set
            {
                _notion_Properties_Dictionary = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Properties_Dictionary));
            }
        }

        private static Dictionary<string, string> _notion_Locations_Dictionary = new();
        public static Dictionary<string, string> Notion_Locations_Dictionary
        {
            get
            {
                return _notion_Locations_Dictionary;
            }
            set
            {
                _notion_Locations_Dictionary = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Locations_Dictionary));
            }
        }

        private static Dictionary<string, string> _notion_Created_Pages_Dictionary = new();
        public static Dictionary<string, string> Notion_Created_Pages_Dictionary
        {
            get
            {
                return _notion_Created_Pages_Dictionary;
            }
            set
            {
                _notion_Created_Pages_Dictionary = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Created_Pages_Dictionary));
            }
        }

        private static int _generated_Create_Page_ID = 0;
        public static int Generated_Create_Page_ID
        {
            get { return _generated_Create_Page_ID; }
            set
            {
                _generated_Create_Page_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(_generated_Create_Page_ID));
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

        public static JObject Notion_Create_Page_Json(string DatabaseID, string Title, string Aciklama, string[] Gerceklestiren, DateTime DateStart, string Property, string Location, bool Billable, string Task)
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
                    // Aciklama
                    [Config.Notion_DailyNotes_Aciklama_ID] = new JObject
                    {
                        ["rich_text"] = new JArray(
                            new JObject
                            {
                                ["text"] = new JObject
                                {
                                    ["content"] = Aciklama
                                }
                            })
                    },
                    // Gerceklestiren
                    [Config.Notion_DailyNotes_Gerceklestiren_ID] = new JObject
                    {
                        ["people"] = PeopleJsonArray
                    },
                    // Date
                    [Config.Notion_DailyNotes_Date_ID] = new JObject
                    {
                        ["date"] = new JObject
                        {
                            ["start"] = DateStart,
                            ["end"] = DateStart
                        }
                    },
                    // Property
                    [Config.Notion_DailyNotes_Property_ID] = new JObject
                    {
                        ["select"] = new JObject
                        {
                            ["name"] = Property
                        }
                    },
                    // Location
                    [Config.Notion_DailyNotes_Location_ID] = new JObject
                    {
                        ["select"] = new JObject
                        {
                            ["name"] = Location
                        }
                    },
                    // Billable
                    [Config.Notion_DailyNotes_Billable_ID] = new JObject
                    {
                        ["checkbox"] = Billable
                    },
                    // Tasks
                    [Config.Notion_DailyNotes_Tasks_ID] = new JObject
                    {
                        ["relation"] = new JArray(new JObject { ["id"] = Task})
                    },
                    // Name
                    ["title"] = new JObject
                    {
                        ["title"] = new JArray(
                            new JObject
                            {
                                ["text"] = new JObject
                                {
                                    ["content"] = Title
                                }
                            })
                    }
                }
            };
            logger.Info("Notion_Create_Page_Json: " + Environment.NewLine + outJson);
            return outJson;
        }

        public static JObject Notion_Update_Page_Json(DateTime DateEnd)
        {
            JObject start = (JObject)JsonConvert.DeserializeObject(Notion_Create_Page_Body);
            JObject outJson = new()
            {
                ["properties"] = new JObject
                {
                    [Config.Notion_DailyNotes_Date_ID] = new JObject
                    {
                        ["date"] = new JObject
                        {
                            ["start"] = start["properties"][Config.Notion_DailyNotes_Date_ID]["date"]["start"],
                            ["end"] = DateEnd
                        }
                    }
                }
            };
            logger.Info("Notion_Update_Page_Json: " + Environment.NewLine + outJson);
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

    public class Config
    {
        public static NotifyClass notifyClass = new NotifyClass();

        // API Request Standard variables
        private static string _read_Token = "Bearer secret_fUOlP7ITMCklnmcgUsiif2y8hgEMzfClguJpFLVJBZk";
        public static string Read_Token
        {
            get
            {
                return _read_Token;
            }
            set
            {
                _read_Token = value;
                notifyClass.NotifyPropertyChanged(nameof(Read_Token));
            }
        }
        private static string _read_Write_Token = "Bearer secret_fUOlP7ITMCklnmcgUsiif2y8hgEMzfClguJpFLVJBZk";
        public static string Read_Write_Token
        {
            get
            {
                return _read_Write_Token;
            }
            set
            {
                _read_Write_Token = value;
                notifyClass.NotifyPropertyChanged("Read_Write_Token");
            }
        }
        private static string _notion_Version = "2022-02-22";
        public static string Notion_Version
        {
            get
            {
                return _notion_Version;
            }
            set
            {
                _notion_Version = value;
                notifyClass.NotifyPropertyChanged("Notion_Version");
            }
        }
        private static string _query_DB_Endpoint = @"https://api.notion.com/v1/databases/{id}/query";
        public static string Query_DB_Endpoint
        {
            get
            {
                return _query_DB_Endpoint;
            }
            set
            {
                _query_DB_Endpoint = value;
                notifyClass.NotifyPropertyChanged("Query_DB_Endpoint");
            }
        }
        private static string _getUsers_Endpoint = @"https://api.notion.com/v1/users";
        public static string GetUsers_Endpoint
        {
            get
            {
                return _getUsers_Endpoint;
            }
            set
            {
                _getUsers_Endpoint = value;
                notifyClass.NotifyPropertyChanged("GetUsers_Endpoint");
            }
        }
        private static string _get_DB_Endpoint = @"https://api.notion.com/v1/databases/{id}";
        public static string Get_DB_Endpoint
        {
            get
            {
                return _get_DB_Endpoint;
            }
            set
            {
                _get_DB_Endpoint = value;
                notifyClass.NotifyPropertyChanged("Get_DB_Endpoint");
            }
        }
        private static string _get_Page_Endpoint = @"https://api.notion.com/v1/pages/{id}";
        public static string Get_Page_Endpoint
        {
            get
            {
                return _get_Page_Endpoint;
            }
            set
            {
                _get_Page_Endpoint = value;
                notifyClass.NotifyPropertyChanged("Get_Page_Endpoint");
            }
        }
        private static string _create_Page_Endpoint = @"https://api.notion.com/v1/pages";
        public static string Create_Page_Endpoint
        {
            get
            {
                return _create_Page_Endpoint;
            }
            set
            {
                _create_Page_Endpoint = value;
                notifyClass.NotifyPropertyChanged("Create_Page_Endpoint");
            }
        }
        private static string _update_Page_Endpoint = @"https://api.notion.com/v1/pages/{id}";
        public static string Update_Page_Endpoint
        {
            get
            {
                return _update_Page_Endpoint;
            }
            set
            {
                _update_Page_Endpoint = value;
                notifyClass.NotifyPropertyChanged("Update_Page_Endpoint");
            }
        }
        private static string _projects_DB_ID = "3f0c889474e441ffab5de0711decb44f";
        public static string Projects_DB_ID
        {
            get
            {
                return _projects_DB_ID;
            }
            set
            {
                _projects_DB_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Projects_DB_ID));
            }
        }
        private static string _tasks_DB_ID = "82dbeb69d0e94f81acb2be5b581a29a1";
        public static string Tasks_DB_ID
        {
            get
            {
                return _tasks_DB_ID;
            }
            set
            {
                _tasks_DB_ID = value;
                notifyClass.NotifyPropertyChanged("Tasks_DB_ID");
            }
        }
        private static string _dailynotes_DB_ID = "f95c4d839b0045d0aed7d113f77da93b";
        public static string Dailynotes_DB_ID
        {
            get
            {
                return _dailynotes_DB_ID;
            }
            set
            {
                _dailynotes_DB_ID = value;
                notifyClass.NotifyPropertyChanged("Dailynotes_DB_ID");
            }
        }
        private static string _users_DB_ID = "f75294e49e0c46d38eaaaeb5d231c83d";
        public static string Users_DB_ID
        {
            get
            {
                return _users_DB_ID;
            }
            set
            {
                _users_DB_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Users_DB_ID));
            }
        }
        private static string _get_DB_ID_From_URL_Regex = @"(notion[.]so[/])?(?<ID>[\w]{32})([?]v[=])?$";
        public static string Get_DB_ID_From_URL_Regex
        {
            get
            {
                return _get_DB_ID_From_URL_Regex;
            }
            set
            {
                _get_DB_ID_From_URL_Regex = value;
                notifyClass.NotifyPropertyChanged("Get_DB_ID_From_URL_Regex");
            }
        }

        private static string _notion_DailyNotes_Aciklama_ID = "Lh%7Bs";
        public static string Notion_DailyNotes_Aciklama_ID
        {
            get
            {
                return _notion_DailyNotes_Aciklama_ID;
            }
            set
            {
                _notion_DailyNotes_Aciklama_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Aciklama_ID));
            }
        }

        private static string _notion_DailyNotes_Gerceklestiren_ID = "%3BOG%3D";
        public static string Notion_DailyNotes_Gerceklestiren_ID
        {
            get
            {
                return _notion_DailyNotes_Gerceklestiren_ID;
            }
            set
            {
                _notion_DailyNotes_Gerceklestiren_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Gerceklestiren_ID));
            }
        }

        private static string _notion_DailyNotes_Date_ID = "vPY%5E";
        public static string Notion_DailyNotes_Date_ID
        {
            get
            {
                return _notion_DailyNotes_Date_ID;
            }
            set
            {
                _notion_DailyNotes_Date_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Date_ID));
            }
        }

        private static string _notion_DailyNotes_Property_ID = "QTaj";
        public static string Notion_DailyNotes_Property_ID
        {
            get
            {
                return _notion_DailyNotes_Property_ID;
            }
            set
            {
                _notion_DailyNotes_Property_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Property_ID));
            }
        }

        private static string _notion_DailyNotes_Location_ID = "y%7Dbx";
        public static string Notion_DailyNotes_Location_ID
        {
            get
            {
                return _notion_DailyNotes_Location_ID;
            }
            set
            {
                _notion_DailyNotes_Location_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Location_ID));
            }
        }

        private static string _notion_DailyNotes_Billable_ID = "wxTI";
        public static string Notion_DailyNotes_Billable_ID
        {
            get
            {
                return _notion_DailyNotes_Billable_ID;
            }
            set
            {
                _notion_DailyNotes_Billable_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Billable_ID));
            }
        }

        private static string _notion_DailyNotes_Tasks_ID = "%3BeGX";
        public static string Notion_DailyNotes_Tasks_ID
        {
            get
            {
                return _notion_DailyNotes_Tasks_ID;
            }
            set
            {
                _notion_DailyNotes_Tasks_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Tasks_ID));
            }
        }

        private static string _notion_DailyNotes_Projects_ID = "gX_c";
        public static string Notion_DailyNotes_Projects_ID
        {
            get
            {
                return _notion_DailyNotes_Projects_ID;
            }
            set
            {
                _notion_DailyNotes_Projects_ID = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_DailyNotes_Projects_ID));
            }
        }

        private static string _notion_Get_Properties_JsonPath = "$['properties']..[?(@.id=='" + Notion_DailyNotes_Property_ID + "')].['select'].['options'].[*]";
        public static string Notion_Get_Properties_JsonPath
        {
            get
            {
                return _notion_Get_Properties_JsonPath;
            }
            set
            {
                _notion_Get_Properties_JsonPath = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Get_Properties_JsonPath));
            }
        }

        private static string _notion_Get_Locations_JsonPath = "$['properties']..[?(@.id=='" + Notion_DailyNotes_Location_ID + "')].['select'].['options'].[*]";
        public static string Notion_Get_Locations_JsonPath
        {
            get
            {
                return _notion_Get_Locations_JsonPath;
            }
            set
            {
                _notion_Get_Locations_JsonPath = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_Get_Properties_JsonPath));
            }
        }

        private static string _notion_GetDataBase_JsonPath = string.Empty;
        public static string Notion_GetDataBase_JsonPath
        {
            get
            {
                return _notion_GetDataBase_JsonPath;
            }
            set
            {
                _notion_GetDataBase_JsonPath = value;
                notifyClass.NotifyPropertyChanged(nameof(Notion_GetDataBase_JsonPath));
            }
        }

        public static JObject Default_Config;

        public static JObject JObjectify_Config_Variables()
        {
            JObject WriteJ = new()
            {
                [nameof(Read_Token)] = Read_Token,
                [nameof(Read_Write_Token)] = Read_Write_Token,
                [nameof(Notion_Version)] = Notion_Version,
                [nameof(Query_DB_Endpoint)] = Query_DB_Endpoint,
                [nameof(GetUsers_Endpoint)] = GetUsers_Endpoint,
                [nameof(Get_DB_Endpoint)] = Get_DB_Endpoint,
                [nameof(Get_Page_Endpoint)] = Get_Page_Endpoint,
                [nameof(Create_Page_Endpoint)] = Create_Page_Endpoint,
                [nameof(Update_Page_Endpoint)] = Update_Page_Endpoint,
                [nameof(Projects_DB_ID)] = Projects_DB_ID,
                [nameof(Tasks_DB_ID)] = Tasks_DB_ID,
                [nameof(Dailynotes_DB_ID)] = Dailynotes_DB_ID,
                [nameof(Users_DB_ID)] = Users_DB_ID,
                [nameof(Get_DB_ID_From_URL_Regex)] = Get_DB_ID_From_URL_Regex,
                [nameof(Notion_Get_Properties_JsonPath)] = Notion_Get_Properties_JsonPath,
                [nameof(Notion_Get_Locations_JsonPath)] = Notion_Get_Locations_JsonPath,
                [nameof(Notion_GetDataBase_JsonPath)] = Notion_GetDataBase_JsonPath
            };
            MessageBox.Show(WriteJ.ToString());
            return WriteJ;
        }
    }
}
