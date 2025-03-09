using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.Models
{
	public class RunData
	{
		public int Id { get; set; }

		public User User { get; set; }

		public double Distance { get; set; }

		public int Hours { get; set; }

		public int Minutes { get; set; }

		public int Seconds { get; set; }

        public RouteData RouteData { get; private set; }

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

        public RunData(User user)
        {
            this.User = user;
            this.Distance = 0;
            this.Hours = 0;
            this.Minutes = 0;
            this.Seconds = 0;
            this.RouteData = new RouteData();
        }

		public RunData()
        {

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
