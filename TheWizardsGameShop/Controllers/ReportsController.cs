using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FastReport.Data;
using FastReport.Web;
using FastReport.Utils;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace TheWizardsGameShop.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index(string reportName)
        {
            if (string.IsNullOrEmpty(reportName))
            {
                reportName = "GameReport";
            }
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            WebReport webReport = new WebReport();
            MsSqlDataConnection sqlConnection = new MsSqlDataConnection();
            sqlConnection.ConnectionString = @"Data Source=prog3050.daehwa.ca;Initial Catalog=TheWizardsGameShop;Persist Security Info=True;User ID=sa;Password=TheWizards!";
            sqlConnection.CreateAllTables();

            webReport.Report.Dictionary.Connections.Add(sqlConnection);
            webReport.Report.Load($".//Reports//{reportName}.frx");
            webReport.ShowPreparedReport = false;
            webReport.ShowExports = false;
            webReport.ShowZoomButton = false;

            ViewBag.report = webReport;
            return View();
        }
    }
}
