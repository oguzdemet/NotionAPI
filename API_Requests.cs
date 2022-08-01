using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static NotionAPI.INIT;

namespace NotionAPI
{
    public class API_Requests
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static RestRequest request = new();
        public static RestClient restClient = new();
        public static RestClientOptions clientOptions = new();
        public static RestResponse? Rest_Response { get; set; }

        public static bool Notion_Api_Base(string Request_Type, string End_Point)
        {
            logger.Info("Notion_API_Base void started. {Request_Type} {End_Point}", Request_Type, End_Point);
            request = new RestRequest();
            clientOptions = new RestClientOptions(End_Point)
            {
                ThrowOnAnyError = true,
                MaxTimeout = 100000
            };
            restClient = new RestClient(clientOptions);

            try
            {
                logger.Info("Adjusting request headers and body.");
                switch (Request_Type)
                {
                    case "GetUsers":
                        request.AddHeader("Notion-Version", Notion_Version);
                        request.AddHeader("Authorization", Notion_Token);
                        break;
                    case "GetPage":
                        break;
                    case "GetDatabase":
                        break;
                    case "QueryDatabase":
                        logger.Info("{Notion_Database_ID}", Notion_Database_ID);

                        request.AddUrlSegment("id", Notion_Database_ID);
                        request.AddHeader("Notion-Version", Notion_Version);
                        request.AddHeader("Authorization", Notion_Token);
                        if (!string.IsNullOrEmpty(Notion_Query_DataBase_Filter_Body))
                        {
                            request.AddStringBody(Notion_Query_DataBase_Filter_Body, RestSharp.DataFormat.Json);
                        }

                        logger.Info("Request adjusted to query database.{Notion_Database_ID} {Notion_Query_DataBase_Filter_Body} {End_Point}", Notion_Database_ID, Notion_Query_DataBase_Filter_Body, End_Point);
                        break;
                    case "CreatePage":
                        break;
                    case "Search":
                        break;
                    default:
                        throw new ArgumentException("Argument exception on Request_Type: " + Request_Type + " End_Point: " + End_Point);
                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
            finally
            {
                MessageBoxResult MsgResult = new();
                while (!InternetAvailability.IsInternetAvailable() && MsgResult != MessageBoxResult.Cancel)
                {
                    logger.Warn("Internet is unavailable. Popup is going to be shown.");

                    MsgResult = MessageBox.Show("Unable to retrieve user info. Check your internet connection and then click ok.", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Question, MsgResult, MessageBoxOptions.DefaultDesktopOnly);

                    logger.Warn("User clicked {result}.", MsgResult);
                    if (MsgResult == MessageBoxResult.Cancel)
                    {
                        MessageBox.Show("The application cannot reach Notion API. Please connect to the internet and then reopen the application.");
                        break;
                    }
                }
            }
        }

        public static Dictionary<string, string> Notion_Parse_Response(string Request_Type, RestResponse Response)
        {
            Dictionary<string, string> Result = new();
            try
            {
                if (!Response.IsSuccessful)
                {
                    throw new Exception("Request unsuccesfull!");
                }
                else if (string.IsNullOrEmpty(Response.Content))
                {
                    throw new Exception("Response body is null!");
                }

                logger.Info("Response: " + "\n" + Response.Content);

                JObject Jobj = JObject.Parse(Response.Content);

                switch (Request_Type)
                {
                    case "GetUsers":
                        Result = JsonConvert.DeserializeObject<IEnumerable<Person>>(Jobj["results"].ToString()).
                                 Select(p => (Name: p.name, Record: p.id)).
                                 Where(x => x.Name != API_Username).
                                 ToDictionary(t => t.Name, t => t.Record);
                        break;

                    case "GetPage":
                        break;

                    case "GetDatabase":
                        break;

                    case "QueryDatabase":
                        JArray JArr = JArray.Parse(Jobj["results"].ToString());
                        Project_Names = JArr.Values("content").Select(x => x.ToString()).ToArray();

                        foreach (JObject keyValuePairs in JArr)
                        {
                            //Result.Add(keyValuePairs["properties"]["Name"]["title"][0]["text"]["content"].ToString(), keyValuePairs["id"].ToString());
                            //MessageBox.Show("key adding: " + keyValuePairs.SelectToken("$['properties'].*.['title'].[0].['text'].['content']").ToString());
                            Result.Add(keyValuePairs.SelectToken("$['properties'].*.['title'].[0].['text'].['content']").ToString(), keyValuePairs["id"].ToString());
                        }
                        break;

                    case "CreatePage":
                        break;

                    case "Search":
                        break;

                    default:
                        throw new ArgumentException("Argument exception on Request_Type: " + Request_Type);
                }

                return Result;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return new Dictionary<string, string>();
            }
        }

        public static void Notion_Query_DataBase()
        {
            logger.Info("Notion_Query_Database void started.");
            bool ok = Notion_Api_Base("QueryDatabase", Notion_Query_DataBase_EndPoint);
            if (ok)
            {
                logger.Info("Making API request...");
                Rest_Response = restClient.Post(request);
                Notion_Query_DataBase_Dictionary = Notion_Parse_Response("QueryDatabase", Rest_Response);
                logger.Info("Projects dictionary has been set. {Notion_Query_DataBase_Dictionary}", string.Join(", ", Notion_Query_DataBase_Dictionary.Keys));
            }
            else
            {
                logger.Warn("Projects dictionary cannot set due to request is not ok.");
            }
        }

        public static void Notion_Get_Users()
        {
            logger.Info("Notion_Get_Users void started.");
            bool ok = Notion_Api_Base("GetUsers", Notion_GetUsers_EndPoint);
            if (ok)
            {
                logger.Info("Making API request...");
                Rest_Response = restClient.Get(request);
                Notion_Users_Dictionary = Notion_Parse_Response("GetUsers", Rest_Response);
                logger.Info("Users dictionary has been set.");
            }
            else
            {
                logger.Warn("Users dictionary cannot set due to request is not ok.");
                MessageBox.Show("Cannot obtain users. Please restart the application.");
            }
        }


        public static Dictionary<string, string> GetProjects(string API_Username, string Notion_Query_DataBase_EndPoint, string Notion_Token, string Notion_Projects_DataBase_ID, string Notion_Version, JObject Notion_Query_DataBase_Filter_Body)
        {            
            logger.Info("GetProjects Started {API_Username} {Notion_GetUsers_EndPoint} {Notion_Token} {Notion_Version} {Notion_Query_DataBase_Filter_Body}", API_Username, Notion_Query_DataBase_EndPoint, Notion_Token, Notion_Version, Notion_Query_DataBase_Filter_Body);
            try
            {
                var options = new RestClientOptions(Notion_Query_DataBase_EndPoint)
                {
                    ThrowOnAnyError = true,
                    MaxTimeout = 100000
                };
                var client = new RestClient(options);

                var request = new RestRequest();
                request.AddUrlSegment("id", Notion_Projects_DataBase_ID);
                request.AddHeader("Notion-Version", Notion_Version);
                request.AddHeader("Authorization", Notion_Token);
                request.AddStringBody(JsonConvert.SerializeObject(Notion_Query_DataBase_Filter_Body), RestSharp.DataFormat.Json);

                RestResponse? response = client.Post(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception("Request unsuccesfull!");
                }
                else if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception("Response body is null!");
                }
                string jsonstring = JsonConvert.SerializeObject(response.Content);

                JObject Jobj = JObject.Parse(response.Content);

                JArray JArr = JArray.Parse(Jobj["results"].ToString());

                //MessageBox.Show(string.Join(" : ", JArr.Values("name")));

                //MessageBox.Show(JArr.Count.ToString());

                //MessageBox.Show(Jobj.ContainsKey("results").ToString());

                //string[] UserNames = JArr.Values("name").Select(x => x.ToString()).ToArray<string>();
                //MessageBox.Show("oldi hadi: " + string.Join(":", UserNames));
                //MessageBox.Show(jsonstring);

                //var dictionary = JsonConvert.DeserializeObject<IEnumerable<Person>>(Jobj["results"].ToString()).
                //                 Select(p => (Name: p.name, Record: p.id)).
                //                 Where(x => x.Name != API_Username).
                //                 ToDictionary(t => t.Name, t => t.Record);

                var dictionary = new Dictionary<string, string>();

                string[] ProjectNames = JArr.Values("content").Select(x => x.ToString()).ToArray();

                //MessageBox.Show("Proje isimleri: " + string.Join(" : ", ProjectNames));
                
                foreach (JObject keyValuePairs in JArr)
                {
                    dictionary.Add(keyValuePairs["properties"]["Name"]["title"][0]["text"]["content"].ToString(), keyValuePairs["id"].ToString());
                }
                return dictionary;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return new Dictionary<string, string>();
            }
        }
    }
}
