using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcCore.WebApp.DependencyInjection.Code;
using MvcCore.WebApp.DependencyInjection.Models;

namespace MvcCore.WebApp.DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        private IInjectionObserver injectionObserver;
        public HomeController(IInjectionObserver injectionObserver)
        {
            //Set a breakpoint here to see the magic, second step
            this.injectionObserver = injectionObserver;
        }
        public IActionResult Index()
        {
            //Set a breakpoint here to see the magic, third step
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
