


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Org.BouncyCastle.Crypto.Prng.Drbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWizardsGameShop.Models;

namespace TheWizardsGameShop
{
    public static class UserHelper
    {
        // Declaring Constants
        public const string NOT_LOGGED_IN_MESSAGE = "Please log in to proceed.";
        public const string NOT_EMPLOYEE_MESSAGE = "You don't have permission to access this page";

        /// <summary>
        /// Check if the user is logged in
        /// </summary>
        /// <param name="ctr">Controller of the page</param>
        /// <returns></returns>
        public static bool IsLoggedIn(Controller ctr)
        {
            return ctr.HttpContext.Session.GetInt32("userId") != null;
        }

        /// <summary>
        /// Check if the user has a role as an Employee
        /// </summary>
        /// <param name="ctr">Controller of the page</param>
        /// <returns></returns>
        public static bool IsEmployee(Controller ctr)
        {
            var userRole = ctr.HttpContext.Session.GetString("userRole");
            if (userRole == null || !IsLoggedIn(ctr)) return false;

            return userRole == "Employee";
        }

        /// <summary>
        /// Interupt the requested action and redirect to login page
        /// </summary>
        /// <param name="ctr">Controller of the page</param>
        /// <returns></returns>
        public static RedirectToActionResult RequireLogin(Controller ctr, string message = NOT_LOGGED_IN_MESSAGE)
        {
            string actionName = ctr.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ctr.ControllerContext.RouteData.Values["controller"].ToString();

            // Pass if already logged in
            if (IsLoggedIn(ctr)) return null;

            // Save the requested action
            ctr.TempData["LoginMessage"] = message;
            ctr.TempData["RequestedActionName"] = actionName;
            ctr.TempData["RequestedControllerName"] = controllerName;

            // Redirect to login page
            return ctr.RedirectToAction("Login", "Users");
        }

        /// <summary>
        /// Interupt the requested action to block access to employee only page according to user role
        /// </summary>
        /// <param name="ctr"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static RedirectToActionResult RequireEmployee(Controller ctr, string message = NOT_EMPLOYEE_MESSAGE)
        {
            if (!IsLoggedIn(ctr)) return RequireLogin(ctr);
            ctr.TempData["ErrorPageTitle"] = "Error";
            ctr.TempData["ErrorPageMessage"] = message;
            return ctr.RedirectToAction("Error", "Home");
        }
    }
}
