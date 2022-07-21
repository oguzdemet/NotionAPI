using RestSharp;
using System.Windows;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using NLog;

namespace NotionAPI
{

    public class Person
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class Notion_API
    {
        public static Dictionary<string, string> GetUsers(string API_Username, string Notion_GetUsers_EndPoint, string Notion_Token, string Notion_Version)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("GetUsers Started {API_Username} {Notion_GetUsers_EndPoint} {Notion_Token} {Notion_Version}", API_Username, Notion_GetUsers_EndPoint, Notion_Token, Notion_Version);
            try
            {
                MessageBoxResult MsgResult = new();
                while (!InternetAvailability.IsInternetAvailable() && MsgResult != MessageBoxResult.Cancel)
                {
                    logger.Warn("Internet is unavailable. Popup is going to be shown.");

                    MsgResult = MessageBox.Show("Unable to retrieve user info. Check your internet connection and then click ok.", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Question, MsgResult, MessageBoxOptions.DefaultDesktopOnly);

                    logger.Warn("User clicked {result}.", MsgResult);
                    if (MsgResult == MessageBoxResult.Cancel)
                    {
                        MessageBox.Show("The application cannot reach users. Please connect to the internet and then reopen the application.");
                        break;
                    }
                }
                var options = new RestClientOptions(Notion_GetUsers_EndPoint)
                {
                    ThrowOnAnyError = true,
                    MaxTimeout = 100000
                };
                var client = new RestClient(options);

                var request = new RestRequest();

                request.AddHeader("Notion-Version", Notion_Version);
                request.AddHeader("Authorization", Notion_Token);

                RestResponse? response = client.Get(request);

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

                if (!Jobj.ContainsKey("results"))
                {
                    throw new Exception("Response body does not contain results key!");
                }
                JArray JArr = JArray.Parse(Jobj["results"].ToString());

                //MessageBox.Show(string.Join(" : ", JArr.Values("name")));

                //MessageBox.Show(JArr.Count.ToString());

                //MessageBox.Show(Jobj.ContainsKey("results").ToString());

                string[] UserNames = JArr.Values("name").Select(x => x.ToString()).ToArray();
                //MessageBox.Show("oldi hadi: " + string.Join(":", UserNames));
                //MessageBox.Show(jsonstring);

                var dictionary = JsonConvert.DeserializeObject<IEnumerable<Person>>(Jobj["results"].ToString()).
                                 Select(p => (Name: p.name, Record: p.id)).
                                 Where(x => x.Name != API_Username).
                                 ToDictionary(t => t.Name, t => t.Record);

                foreach (var person in dictionary)
                {
                    //MessageBox.Show("Key: " + person.Key + " value: " + person.Value);
                }
                return dictionary;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                logger.Error(ex);
                MessageBox.Show("Connection interrupted due to unstable/broken internet connection. Please check your internet connection. And then restart the program.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
                return new Dictionary<string, string>();
            }
            catch (Exception e)
            {
                logger.Error(e);
                return new Dictionary<string, string>();
            }
        }
    }
}