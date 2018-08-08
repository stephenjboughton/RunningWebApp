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
	public class RunHistoryController : Controller
	{
		private IRunningAppDAL dal;
		public RunHistoryController(IRunningAppDAL dal)
		{
			this.dal = dal;
		}

		//TODO add session feature for id - maybe call this action from somewhere different - upfront?
		public IActionResult FindRunner()
		{
			return View();
		}

		private const string SessionKey = "Runner";

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddToHistory(RunData runData)
		{
			/*AddToHistory method in RunHistoryDAL passes in runData instance and uses fname and lname to 
			 * query the DB and return the runner id for that fname and lname, then sends an executenonquery 
			 * to insert the instance of runData into the personal history associated with that runner id. 
			 * If user already has a session, we can use the runnerId stored in session to grab their fname
			 * and lname and avoid having to have them fill out that form*/

			//get runnerId from session
			int runnerId = HttpContext.Session.Get<int>(SessionKey);

			//if user didn't have a session already, sessionId will be 0, so take fname and lname from form submit
			//and use it to get runnerId
			if (runnerId == 0)
			{
				runnerId = dal.GetUserID(runData.FName, runData.LName);
			}
			//add the rundata to runner's history using their runnerId
			dal.AddToHistory(runnerId, runData);
			//create a new variable that will hold a runner_id and set it
			int sessionId = HttpContext.Session.Get<int>(SessionKey);
			//set the sessionID with the runnerId fromm the runData the user inputs
			sessionId = runnerId;
			//save the sessionID back into session
			HttpContext.Session.Set(SessionKey, sessionId);
			//redirect to showHistory action
			return RedirectToAction("ShowHistory", "RunHistory");
		}

		public IActionResult ShowHistory(string fName, string lName)
		{
			int sessionId = HttpContext.Session.Get<int>(SessionKey);

			if (fName != null && lName != null)
			{
				sessionId = dal.GetUserID(fName, lName);
			}

			if (sessionId == 0)
			{
				return RedirectToAction("FindRunner", "RunHistory");
			}

			else
			{
				IList<PastRun> runs = dal.ShowHistory(sessionId);
				HttpContext.Session.Set(SessionKey, sessionId);
				return View(runs);
			}
		}
	}
}