using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MvcCore.WebApp.Options.Code;
using MvcCore.WebApp.Options.Models;

namespace MvcCore.WebApp.Options.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<OptionRoot> root;
        private IOptions<SubOption> suboption;
        private IOptions<OptionRoot2> root2;
        private IOptions<SubOption2> suboption2;
        public HomeController(IOptions<OptionRoot> root, IOptions<SubOption> suboption, IOptions<OptionRoot2> root2, IOptions<SubOption2> suboption2)
        {
            this.root = root;
            this.suboption = suboption;
            this.root2 = root2;
            this.suboption2 = suboption2;
        }
        public IActionResult Index()
        {
            var x = this.root.Value.Option1;
            var y = this.suboption.Value.Suboption2;
            var z = this.root2.Value.Option4;
            var a = this.suboption2.Value.Suboption3;
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
