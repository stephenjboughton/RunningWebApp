using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RunningWebApp.Models;

namespace RunningWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration config;
        public HomeController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            var configValue = config["AllowRunTrackingOnDesktop"];
            bool AllowRunTrackingOnDesktop = bool.Parse(config["AllowRunTrackingOnDesktop"]);
            ViewBag.AllowRunTrackingOnDesktop = AllowRunTrackingOnDesktop ? true : false;

            return View();
        }
    }
}
