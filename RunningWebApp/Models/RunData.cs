using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.Models
{
	public class RunData
	{
		public string FName { get; set; }

		public string LName { get; set; }

		public double Distance { get; set; }

		public int Hours { get; set; }

		public int Minutes { get; set; }

		public int Seconds { get; set; }

		public int secondsPerMinute = 60;

		public int secondsPerHour = 60 * 60;

		public int TotalSeconds
		{
			get
			{
				return Hours * secondsPerHour + Minutes * secondsPerMinute + Seconds;
			}
		}

		public int AverageSeconds
		{
			get
			{
				return (int)Math.Floor(TotalSeconds / Distance);
			}
		}

		public int[] PerMilePace()
		{
			
			int minutesPerMile = AverageSeconds / secondsPerMinute;
			int extraSecondsPerMile = AverageSeconds % secondsPerMinute;
			int[] AverageMileTime = new int[]
				{ minutesPerMile, extraSecondsPerMile };
			return AverageMileTime;
		}
	}
}
