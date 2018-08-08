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
    public class CalculatorsController : Controller
    {
		private IRunningAppDAL dal;
		public CalculatorsController(IRunningAppDAL dal)
		{
			this.dal = dal;
		}

		private const string SessionKey = "Runner";

		public IActionResult PaceCalculator()
        {
            return View();
        }

		public IActionResult PaceCalculatorResult(RunData model)
		{
			//get runnerId from session
			int sessionId = HttpContext.Session.Get<int>(SessionKey);

			//if user wasn't in session, return view witht KnownUser set to false, which prompts if statement
			//in view to include fname lname form, which will return runnerId
			if (sessionId == 0)
			{
				ViewBag.KnownUser = false;
				return View(model);
			}

			//if user was in session KnownUser will be true so we will not even display fname and lname form on
			//view, just give them addToHistory button which takes their runnerId from session
			else
			{
				ViewBag.KnownUser = true;
				return View(model);
			}
		}
	}
}