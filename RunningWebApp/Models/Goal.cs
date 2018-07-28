using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RunningWebApp.Models
{
    public class Goal
    {
		[Required(ErrorMessage = "Please select a distance in order to gauge how close you are to your goal")]
		public double Distance { get; set; }

		public int UHours { get; set; }

		public int UMinutes { get; set; }

		public int USeconds { get; set; }

		public int GHours { get; set; }

		public int GMinutes { get; set; }

		public int GSeconds { get; set; }

		public static List<SelectListItem> RaceDistance = new List<SelectListItem>()
		{
			new SelectListItem {Text = "Please Choose Your Distance", Value = "" },
			new SelectListItem {Text = "5k", Value = "3.1" },
			new SelectListItem {Text = "5 miler", Value = "5.0" },
			new SelectListItem {Text = "10k", Value = "6.2" },
			new SelectListItem {Text = "10 miler", Value = "10.0" },
			new SelectListItem {Text = "Half marathon", Value = "13.1" },
			new SelectListItem {Text = "Marathon", Value = "26.2" },
		};

		public int secondsPerMinute = 60;

		public int secondsPerHour = 60 * 60;

		public int UTotalSeconds
		{
			get
			{
				return UHours * secondsPerHour + UMinutes * secondsPerMinute + USeconds;
			}
		}

		public int UAverageSeconds
		{
			get
			{
				return (int)Math.Floor(UTotalSeconds / Distance);
			}
		}

		public int GTotalSeconds
		{
			get
			{
				return GHours * secondsPerHour + GMinutes * secondsPerMinute + GSeconds;
			}
		}

		public int GAverageSeconds
		{
			get
			{
				return (int)Math.Floor(GTotalSeconds / Distance);
			}
		}
		
		public int[] UPerMilePace
		{
			get
			{
				return PerMilePace(UAverageSeconds);
			}
		}

		public int[] GPerMilePace
		{
			get
			{
				return PerMilePace(GAverageSeconds);
			}
		}

		public int[] PerMilePace(int averageSeconds)
		{
			int minutesPerMile = averageSeconds / secondsPerMinute;
			int extraSecondsPerMile = averageSeconds % secondsPerMinute;
			int[] AverageMileTime = new int[]
				{ minutesPerMile, extraSecondsPerMile };
			return AverageMileTime;
		}
	}
}
