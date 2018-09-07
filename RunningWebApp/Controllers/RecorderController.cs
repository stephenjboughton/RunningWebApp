using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RunningWebApp.Controllers
{
    public class RecorderController : Controller
    {
        public IActionResult RunRecorder()
        {
            return View();
        }
    }
}