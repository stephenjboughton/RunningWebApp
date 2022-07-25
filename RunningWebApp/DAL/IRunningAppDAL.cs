using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunningWebApp.Models;

namespace RunningWebApp.DAL
{
    public interface IRunningAppDAL
    {		
		int AddToHistory(int runnerId, RunData rundata);

		IList<PastRun> ShowHistory(int runnerId);

		int GetUserID(string fname, string lname, string emailAddress);
    }
}
