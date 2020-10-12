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
        public static string ComputeHash(string input)
        {
            // Create a byte array from input
            var inputInByte = ASCIIEncoding.ASCII.GetBytes(input);

            // Compute hash based on source data.
            var hashValue = new MD5CryptoServiceProvider().ComputeHash(inputInByte);

            return ByteArrayToString(hashValue);
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
