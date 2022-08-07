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
using static NotionAPI.Config;

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
                        request.AddHeader("Authorization", Read_Token);
                        logger.Info("GetUsers.");
                        break;
                    case "GetPage":
                        logger.Info("GetPage.");
                        break;
                    case "GetDatabase":
                        request.AddUrlSegment("id", Notion_Database_ID);
                        request.AddHeader("Notion-Version", Notion_Version);
                        request.AddHeader("Authorization", Read_Token);
                        logger.Info("GetDatabase.");
                        break;
                    case "QueryDatabase":
                        logger.Info("{Notion_Database_ID}", Notion_Database_ID);

                        request.AddUrlSegment("id", Notion_Database_ID);
                        request.AddHeader("Notion-Version", Notion_Version);
                        request.AddHeader("Authorization", Read_Token);
                        if (!string.IsNullOrEmpty(Notion_Query_DataBase_Filter_Body))
                        {
                            request.AddStringBody(Notion_Query_DataBase_Filter_Body, RestSharp.DataFormat.Json);
                        }
                        logger.Info("Request adjusted to query database.", Notion_Database_ID, Notion_Query_DataBase_Filter_Body, End_Point);
                        break;
                    case "CreatePage":
                        request.AddHeader("Notion-Version", Notion_Version);
                        request.AddHeader("Authorization", Read_Write_Token);
                        request.AddStringBody(Notion_Create_Page_Body, RestSharp.DataFormat.Json);
                        logger.Info("CreatePage.");
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
                Notion_Query_DataBase_Filter_Body = string.Empty;
                Notion_Create_Page_Body = string.Empty;
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
            logger.Info("Notion_Parse_Response void started.", Request_Type);
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
                logger.Info("", Response.ResponseStatus);

                JObject Jobj = JObject.Parse(Response.Content);
                logger.Info("Response parsed to JObject.");

                switch (Request_Type)
                {
                    case "GetUsers":
                        Result = JsonConvert.DeserializeObject<IEnumerable<Person>>(Jobj["results"].ToString()).
                                 Select(p => (Name: p.name, Record: p.id)).
                                 Where(x => x.Name != string.Empty).
                                 ToDictionary(t => t.Name, t => t.Record);
                        break;

                    case "GetPage":
                        
                        break;

                    case "GetDatabase":
                        foreach (JObject keyValuePairs in Jobj.SelectTokens(Notion_GetDataBase_JsonPath))
                        {
                            Result.Add(keyValuePairs["name"].ToString(), keyValuePairs["id"].ToString());
                        }
                        break;

                    case "QueryDatabase_Text":
                        JArray JArr = JArray.Parse(Jobj["results"].ToString());

                        foreach (JObject keyValuePairs in JArr)
                        {
                            //Result.Add(keyValuePairs["properties"]["Name"]["title"][0]["text"]["content"].ToString(), keyValuePairs["id"].ToString());
                            //MessageBox.Show("key adding: " + keyValuePairs.SelectToken("$['properties'].*.['title'].[0].['text'].['content']").ToString());
                            Result.Add(keyValuePairs.SelectToken("$['properties'].*.['title'].[0].['text'].['content']").ToString(), keyValuePairs["id"].ToString());
                        }
                        break;

                    case "QueryDatabase_Properties":
                        JArray JArr2 = JArray.Parse(Jobj["results"].ToString());
                        //Project_Names = JArr2.Values("content").Select(x => x.ToString()).ToArray();
                        JArray jArray2 = JArray.Parse(JsonConvert.SerializeObject(JArr2.SelectTokens("$['properties']..[?(@.id=='QTaj')].['select'].['options'].[*]")));
                        foreach (JObject keyValuePairs in jArray2)
                        {
                            //Result.Add(keyValuePairs["properties"]["Name"]["title"][0]["text"]["content"].ToString(), keyValuePairs["id"].ToString());
                            //MessageBox.Show("key adding: " + keyValuePairs.SelectToken("$['properties'].*.['title'].[0].['text'].['content']").ToString());
                            Result.Add(keyValuePairs["name"].ToString(), keyValuePairs["id"].ToString());
                        }
                        break;

                    case "QueryDatabase_People":
                        
                        JArray JArr3 = JArray.Parse(JsonConvert.SerializeObject(Jobj.SelectTokens("$['results']..['properties']")));
                        foreach (JObject keyValuePairs in JArr3)
                        {
                            JObject key1 = JObject.Parse(JsonConvert.SerializeObject(keyValuePairs.SelectToken("$..['title'].[*]")));
                            JObject value1 = JObject.Parse(JsonConvert.SerializeObject(keyValuePairs.SelectToken("$..['people'].[*]")));

                            Result.Add(key1["text"]["content"].ToString(), value1["id"].ToString());
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
            finally
            {
                Rest_Response = new RestResponse();
            }
        }

        public static void Notion_Query_DataBase(string type)
        {
            logger.Info("Notion_Query_Database void started.");
            bool ok = Notion_Api_Base("QueryDatabase", Query_DB_Endpoint);
            if (ok)
            {
                logger.Info("Making API request...");
                Rest_Response = restClient.Post(request);
                Notion_Database_ID = string.Empty;
                Notion_Query_DataBase_Dictionary = Notion_Parse_Response("QueryDatabase_" + type, Rest_Response);
                logger.Info("Queried dictionary has been set. {Notion_Query_DataBase_Dictionary}", string.Join(", ", Notion_Query_DataBase_Dictionary.Keys));
            }
            else
            {
                logger.Warn("Queried dictionary cannot set due to request is not ok.");
            }
        }

        public static void Notion_Get_Users()
        {
            logger.Info("Notion_Get_Users void started.");
            bool ok = Notion_Api_Base("GetUsers", GetUsers_Endpoint);
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

        public static void Notion_Get_Database()
        {
            logger.Info("Notion_Get_Database void started.");
            bool ok = Notion_Api_Base("GetDatabase", Get_DB_Endpoint);
            if (ok)
            {
                try
                {
                    logger.Info("Making API request...");
                    Rest_Response = restClient.Get(request);
                    Notion_Database_Dictionary = Notion_Parse_Response("GetDatabase", Rest_Response);
                    logger.Info("Database property dictionary has been set.");
                }
                catch (Exception e)
                {
                    logger.Error(e.Source + Environment.NewLine + e.Source);
                }
            }
            else
            {
                logger.Warn("Databse property dictionary cannot set due to request is not ok.");
                MessageBox.Show("Cannot obtain users. Please restart the application.");
            }
        }

        public static void Notion_Create_Page()
        {
            logger.Info("Notion_Create_Page void started.");
            bool ok = Notion_Api_Base("CreatePage", Create_Page_Endpoint);
            if (ok)
            {
                try
                {
                    logger.Info("Making API request...");
                    Rest_Response = restClient.Get(request);
                    // Gelen response u parse edip Page ID'yi kaydet.
                    Notion_Database_Dictionary = Notion_Parse_Response("GetDatabase", Rest_Response);
                    logger.Info("Database property dictionary has been set.");
                }
                catch (Exception e)
                {
                    logger.Error(e.Source + Environment.NewLine + e.Source);
                }
            }
            else
            {
                logger.Warn("Databse property dictionary cannot set due to request is not ok.");
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
