using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop
{
    public static class UserHelper
    {
        public const string NOT_LOGGED_IN_MESSAGE = "Please log in to proceed.";

        public static bool IsLoggedIn(HttpContext context)
        {
            return context.Session.GetInt32("userId") != null;
        }

        public static bool IsEmployee(HttpContext context)
        {
            if (!UserHelper.IsLoggedIn(context)) return false;
            var userRole = context.Session.GetString("userRole");
            if (userRole == null)
            {
                return false;
            }

            return userRole == "Employee";
        }
    }
}
