using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop
{
    public static class Base64Helper
    {
        public static string encode(string data)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string decode(string encodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
