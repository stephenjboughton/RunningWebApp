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

		public IActionResult FindRunner()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddToHistory(RunData runData)
		{
			int runnerId = dal.AddToHistory(runData);
			return RedirectToAction("ShowHistory", "RunHistory", new { ID = runnerId });
		}

		public IActionResult ShowHistory(int ID, string fname, string lname)
		{
			if (ID == 0)
			{
				ID = dal.GetUserID(fname, lname);
			}

			IList<PastRun> runs = dal.ShowHistory(ID);
			return View(runs);
		}
	}
}