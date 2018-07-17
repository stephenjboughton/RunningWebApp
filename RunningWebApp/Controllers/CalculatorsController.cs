using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunningWebApp.Models;

namespace RunningWebApp.Controllers
{
    public class CalculatorsController : Controller
    {
        public IActionResult PaceCalculator()
        {
            return View();
        }

		public IActionResult PaceCalculatorResult(RunData model)
		{
			return View(model);
		}
	}
}