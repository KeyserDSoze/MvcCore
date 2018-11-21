using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcCore.WebApp.CustomLog.Models;

namespace MvcCore.WebApp.CustomLog.Controllers
{
    public class HomeController : Controller
    {
        private ILogger logger;
        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }
        public IActionResult Index()
        {
            this.logger.Log(LogLevel.Debug, "DEBUG");
            this.logger.Log(LogLevel.Information, "INFORMATION");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
