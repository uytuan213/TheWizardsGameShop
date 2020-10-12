using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
