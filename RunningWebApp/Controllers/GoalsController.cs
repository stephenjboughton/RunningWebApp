using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunningWebApp.Models;

namespace RunningWebApp.Controllers
{
    public class GoalsController : Controller
    {
        public IActionResult GoalEntry()
        {
            return View();
        }

		public IActionResult GoalResult(Goal model)
		{
			return View(model);
		}
    }
}