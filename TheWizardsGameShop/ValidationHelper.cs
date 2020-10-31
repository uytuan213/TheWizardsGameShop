using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheWizardsGameShop
{
    public static class ValidationHelper
    {
        public static Boolean PostalCodeValidation(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            Regex pattern = new Regex
                (@"^([ABCEGHJKLMNPRSTVXY]|[abceghjklmnprstvxy])\d([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]) ?\d([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz])\d$");
            return pattern.IsMatch(str.Trim());
        }

        public static Boolean EmailValidation(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            Regex pattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return pattern.IsMatch(str.Trim());
        }

        /// <summary>
        /// Check if password has upper case, number
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean PasswordValidation(string str)
        {
            if (string.IsNullOrEmpty(str) || !str.Any(char.IsLower) || !str.Any(char.IsUpper) || !str.Any(char.IsNumber))
            {
                return false;
            }
            return true;
            
        }

        public static Boolean PhoneValidation(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            Regex pattern = new Regex(@"\D*([2-9]\d{2})(\D*)([2-9]\d{2})(\D*)(\d{4})\D*");
            return pattern.IsMatch(str.Trim());
        }

        public static Boolean IsNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            Regex pattern = new Regex(@"^[0-9]*$");
            return pattern.IsMatch(str.Trim());
        }
    }

    
}
