using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace WpfApp3
{
    public class Amele
    {
        public static string Person = string.Empty;
        public static string TimeFormat = "HH:mm:ss";
        public static string[] Person_Array = Notion_API.GetUsers().Keys.ToArray<string>();


        public static string Replacer(string a)
        {
            return a.ToLower().Trim().Replace("ı", "i").Replace("ş", "s").Replace("ü", "u").Replace("ç", "c").Replace("ö", "o").Replace("ğ", "g");
        }
    }
}
