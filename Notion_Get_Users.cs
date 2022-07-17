using RestSharp;
using RestSharp.Authenticators;
using System.Threading;
using System.Windows;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace WpfApp3
{
    public class Person
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class Notion_API
    {
        public static Dictionary<string, string> GetUsers()
        {
            var options = new RestClientOptions($"https://api.notion.com/v1/users")
            {
                ThrowOnAnyError = true,
                Timeout = 100000
            };
            var client = new RestClient(options);

            var request = new RestRequest();


            request.AddHeader("Notion-Version", "2022-02-22");
            //request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer secret_fUOlP7ITMCklnmcgUsiif2y8hgEMzfClguJpFLVJBZk");

            RestResponse? response = client.Get(request);

            string jsonstring = JsonConvert.SerializeObject(response.Content);

            JObject Jobj = JObject.Parse(response.Content);

            JArray JArr = JArray.Parse(Jobj["results"].ToString());

            //MessageBox.Show(string.Join(" : ", JArr.Values("name")));

            //MessageBox.Show(JArr.Count.ToString());

            //MessageBox.Show(Jobj.ContainsKey("results").ToString());

            string[] UserNames = JArr.Values("name").Select(x => x.ToString()).ToArray<string>();
            MessageBox.Show("oldi hadi: " + string.Join(":", UserNames));
            //MessageBox.Show(jsonstring);

            var dictionary = JsonConvert.DeserializeObject<IEnumerable<Person>>(Jobj["results"].ToString()).
                             Select(p => (Name: p.name, Record: p.id)).
                             Where(x => x.Name != "API_Test").
                             ToDictionary(t => t.Name, t => t.Record);


            foreach (var person in dictionary)
            {
                MessageBox.Show("Key: " + person.Key + " value: " + person.Value);
            }
            return dictionary;
        }
    }
}