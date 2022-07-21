using RestSharp;
using RestSharp.Authenticators;
using System.Threading;
using System.Windows;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

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
            MessageBox.Show("oldi hadi: " + string.Join(":", UserNames));
            //MessageBox.Show(jsonstring);

            var dictionary = JsonConvert.DeserializeObject<IEnumerable<Person>>(Jobj["results"].ToString()).
                             Select(p => (Name: p.name, Record: p.id)).
                             Where(x => x.Name != API_Username).
                             ToDictionary(t => t.Name, t => t.Record);

            foreach (var person in dictionary)
            {
                MessageBox.Show("Key: " + person.Key + " value: " + person.Value);
            }
            return dictionary;
        }
    }
}