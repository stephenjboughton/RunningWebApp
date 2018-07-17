using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunningWebApp.DAL;
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

		public IActionResult AddToHistory(RunData model)
		{
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddToHistory(string fname, string lname, RunData rundata)
		{
			int runnerId = dal.AddToHistory(fname, lname, rundata);
			return RedirectToAction("ShowHistory", "RunHistory", new { ID = runnerId });
		}

		public IActionResult ShowHistory(int ID)
		{
			IList<PastRun> runs = dal.ShowHistory(ID);
			return View(runs);
		}
	}
}