using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NotionAPI
{
    public class Projects
    {
        public static Dictionary<string, string> GetProjects(string API_Username, string Notion_Query_DataBase_EndPoint, string Notion_Token, string Notion_Projects_DataBase_ID, string Notion_Version, JObject Notion_Query_DataBase_Filter_Body)
        {
            var logger = LogManager.GetCurrentClassLogger();
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
                    JArray JArr2 = JArray.Parse(keyValuePairs["properties"]["Name"]["title"].ToString());
                    JObject Jobj2 = JObject.Parse(JArr2[0].ToString());

                    dictionary.Add(Jobj2["text"]["content"].ToString(), keyValuePairs["id"].ToString());
                    //MessageBox.Show("Key: " + Jobj2["text"]["content"].ToString() + " value: " + keyValuePairs["id"]);
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
