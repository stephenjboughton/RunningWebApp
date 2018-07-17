using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.Models
{
	public class PastRun
	{
		public string fname { get; set; }

		public string lname { get; set; }

		public double Distance { get; set; }

		public int Hours { get; set; }

		public int Minutes { get; set; }

		public int Seconds { get; set; }

		public int AverageMinutes { get; set; }

		public int AverageSeconds { get; set; }

		public DateTime PostDate { get; set; }

		public int secondsPerMinute = 60;

		public int secondsPerHour = 60 * 60;

		public int[] ConvertTotalSeconds(int seconds)
		{
			int runHours = (seconds / secondsPerHour);
			int runMinutes = ((seconds % secondsPerHour) / secondsPerMinute);
			int runSeconds = (seconds % secondsPerHour) % secondsPerMinute;
			int[] ConvertedValues = new int[]
				{ runHours, runMinutes, runSeconds };

			return ConvertedValues;
		}

		public int[] ConvertAverageSeconds(int seconds)
		{
			int avgMinutes = (seconds / secondsPerMinute);
			int avgSeconds = seconds % secondsPerMinute;
			int[] ConvertedValues = new int[]
				{ avgMinutes, avgSeconds };

			return ConvertedValues;
		}
	}
}
