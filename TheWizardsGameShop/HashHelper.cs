using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TheWizardsGameShop
{
    /*
     * More about hash https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/compute-hash-values 
     */
    public static class HashHelper
    {
        public static byte[] ComputeHash(string input)
        {
            // Create a byte array from input
            var inputInByte = ASCIIEncoding.ASCII.GetBytes(input);

            // Compute hash based on source data.
            var hashValue = new MD5CryptoServiceProvider().ComputeHash(inputInByte);

            return hashValue;
        }
    }
}
