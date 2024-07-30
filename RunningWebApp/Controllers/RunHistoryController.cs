using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RunningWebApp.DAL;
using RunningWebApp.Extensions;
using RunningWebApp.Models;

namespace RunningWebApp.Controllers
{
	public class RunHistoryController : Controller
	{
		private IRunningAppDAL dal;
		private IConfiguration config;
		public RunHistoryController(IRunningAppDAL dal, IConfiguration config)
		{
			this.dal = dal;
			this.config = config;
		}

		//TODO add session feature for id - maybe call this action from somewhere different - upfront?
		public IActionResult FindRunner()
		{
			return View();
		}

		private const string RunnerKey = "Runner";
		private const string RunKey = "Run";

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public IActionResult AddToHistory(RunData runData)
		{
			/*AddToHistory method in RunHistoryDAL passes in runData instance and uses fname and lname to 
			 * query the DB and return the runner id for that fname and lname, then sends an executenonquery 
			 * to insert the instance of runData into the personal history associated with that runner id. 
			 * If user already has a session, we can use the runnerId stored in session to grab their fname
			 * and lname and avoid having to have them fill out that form*/

			//get runnerId from session
			User runnerInSession = HttpContext.Session.Get<User>(RunnerKey);
			int userId = runnerInSession.Id;

			//if user didn't have a session already, sessionId will be 0, so take fname and lname from form submit
			//and use it to get runnerId
			if (userId == 0)
			{
				userId = dal.GetUserID(runData.User.FName, runData.User.LName, runData.User.EmailAddress);
			}

			int runId = runData.Id;
			// TODONE? - we should be able to have a runId here - do we need to put it in session earlier (after we create it)?
			// or is there something we can do to make sure model comes in with it?
			if (runData.Id == 0)
            {
				runId = HttpContext.Session.Get<int>(RunKey);
            }

			// TODO - decide whether to insert a new run or update an existing based on whether we have an id in session here?


			//add the rundata to runner's history using their runnerId
			dal.AddToHistory(userId, runData, runId);

			////create a new variable that will hold a runner_id and set it
			//int sessionId = HttpContext.Session.Get<int>(RunnerKey);
			////set the sessionID with the runnerId fromm the runData the user inputs
			//sessionId = userId;
			////save the sessionID back into session
			//HttpContext.Session.Set(RunnerKey, sessionId);

			//redirect to showHistory action
			return RedirectToAction("ShowHistory", "RunHistory");
		}

		public IActionResult ShowHistory(string fName, string lName, string emailAddress)
		{
			//int sessionId = HttpContext.Session.Get<int>(RunnerKey);
			var User = HttpContext.Session.Get<User>(RunnerKey);
			int runnerId = User.Id;

			if ((fName != null && lName != null) || emailAddress != null)
			{
				runnerId = dal.GetUserID(fName, lName, emailAddress);
			}

			if (runnerId == 0)
			{
				return RedirectToAction("FindRunner", "RunHistory");
			}

			else
			{
				IList<PastRun> runs = dal.ShowHistory(runnerId);
				HttpContext.Session.Set(RunnerKey, runnerId);
				return View(runs);
			}
		}

		public object GetHistory(string fName, string lName, string emailAddress)
		{
			int sessionId = HttpContext.Session.Get<int>(RunnerKey);

			if ((fName != null && lName != null) || emailAddress != null)
			{
				sessionId = dal.GetUserID(fName, lName, emailAddress);
			}

			if (sessionId == 0)
			{
				return RedirectToAction("FindRunner", "RunHistory");
			}

			else
			{
				IList<PastRun> runs = dal.ShowHistory(sessionId);
				HttpContext.Session.Set(RunnerKey, sessionId);
				return runs;
			}
		}

		[HttpPost]
		public ActionResult CaptureRouteData([FromBody] RouteData payload)
        {
			//var configValue = config["UseWaypointLogging"];
			var useWaypointLogging = bool.Parse(config["UseWaypointLogging"]);
			if (!useWaypointLogging) return Ok();

			DataTable dt = new DataTable();
			//Add columns  
			dt.Columns.Add(new DataColumn("RunId", typeof(int)));
			dt.Columns.Add(new DataColumn("Lattitude", typeof(double)));
			dt.Columns.Add(new DataColumn("Longitude", typeof(double)));
			dt.Columns.Add(new DataColumn("Time", typeof(DateTime)));

			List<RunCoords> convertedWayPoints = new List<RunCoords>();
			for (int i = 0; i < payload.WayPoints.Count; i++)
            {
				RunCoords nextCoords = new RunCoords();
				dt.Rows.Add(payload.RunId, payload.WayPoints[i].lat, payload.WayPoints[i].lon, payload.WayPoints[i].time);
            }


			dal.InsertWayPoints(dt);
	


			Guid guid = Guid.NewGuid();
			string relativepath = System.IO.Path.Combine("RunDataLogs", DateTime.Now.ToString("yyyy-MM-dd T HHmmss") + ".json");
			string workingDirectory = Environment.CurrentDirectory;

			// Could be root of app if we are working from a bin?  Unsure what we will end up with when running Azure
			//string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

			// Should be root of app?
			string projectDirectory = Directory.GetParent(workingDirectory).FullName;
			string fullpath = System.IO.Path.Combine(projectDirectory, relativepath);
            string dir = System.IO.Path.GetDirectoryName(fullpath);

            // TODO = perhaps wrap several things below in try/catch and run them all to see where something will "stick" with Azures
            if (!System.IO.Directory.Exists(dir))
            {
				try
				{
					System.IO.Directory.CreateDirectory(dir);
				}
				catch (Exception ex)
				{
					return Content($"route logging failed: {ex.Message}", "text/plain");
					// do something here to log an error?
				}
			}

            string jsonStringified = JsonConvert.SerializeObject(payload, Formatting.Indented);

			try
			{
				System.IO.File.WriteAllText(fullpath, jsonStringified);
			}
			catch(Exception ex)
			{
				return Content($"route logging failed: {ex.Message}", "text/plain");
				// do something here to log an error?
			}

			return Content($"success - route data written to {fullpath}", "text/plain");
		}

		[HttpPost]
		public User GetUser([FromBody] User payload)
        {
			int sessionId = HttpContext.Session.Get<int>(RunnerKey);

			//if ((payload.FName != null && LName != null) || EmailAddress != null)
			//{
			//	sessionId = dal.GetUserID(fName, lName, emailAddress);
			//}

			User User = new User();
			User = dal.GetUser(payload.FName, payload.LName, payload.EmailAddress);
			return User;
		}
	}
}