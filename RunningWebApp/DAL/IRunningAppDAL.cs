using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RunningWebApp.Models;

namespace RunningWebApp.DAL
{
    public interface IRunningAppDAL
    {		
		RunData AddToHistory(int runnerId, RunData rundata, int runId = 0);

		IList<PastRun> ShowHistory(int runnerId);

        bool InsertWayPoints(DataTable dt);

        int GetUserID(string fname, string lname, string emailAddress);

        User GetUser(string fname, string lname, string emailAddress);
    }
}
