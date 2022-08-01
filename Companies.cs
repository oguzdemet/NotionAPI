using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace NotionAPI
{
    public class Companies
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static Dictionary<string, string> Companies_Dictionary = new() { { "Arcelik", "ARCK" }, { "Coskunoz", "COSK" }, { "Osmanli", "OSMN" } };
    }
}
