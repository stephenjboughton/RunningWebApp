using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunningWebApp.DAL;
using RunningWebApp.Extensions;
using RunningWebApp.Models;

namespace RunningWebApp.Controllers
{
    public class RecorderController : Controller
    {
        private const string RunnerKey = "Runner";
        private const string RunKey = "Run";

        private IRunningAppDAL dal;
        public RecorderController(IRunningAppDAL dal)
        {
            this.dal = dal;
        }

        public IActionResult RunRecorder()
        {
            User runner = HttpContext.Session.Get<User>(RunnerKey);

            if (runner == null)
            {
                return RedirectToAction("Login", "User");
            }

            RunData rd = new RunData(runner);
            dal.AddToHistory(rd.User.Id, rd);

            HttpContext.Session.Set(RunKey, rd.Id);
            //HttpContext.Session.Set<int>(RunKey, rd.Id);
            // what if we call the DAL here, create a new run record so that we have an id, then return a runData object with an id to the view
            // that would allow us to use the ajax call from calculate and take the run id to create all routedata records tied to that parent run

            return View("RunRecorder", rd);
        }
    }
}