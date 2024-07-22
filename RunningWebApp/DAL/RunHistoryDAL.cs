using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using RunningWebApp.Models;

namespace RunningWebApp.DAL
{
	public class RunHistoryDAL : IRunningAppDAL

	{
		private string connectionString;

		public RunHistoryDAL(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public RunData AddToHistory(int runnerId, RunData rundata, int id = 0) // is there any state where we wouldn't have access to runnerId already on rundata object?  if not, let's elmininat the parameter
		{
			int runId;
			if (id > 0)
            {
				// TODO - make this an update instead of an insert
            }
            try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					//string sqlS = $"SELECT id FROM runner WHERE fname = @fname AND lname = @lname;";

					//SqlCommand cmdS = new SqlCommand(sqlS, conn);
					//cmdS.Parameters.AddWithValue("@fname", rundata.FName);
					//cmdS.Parameters.AddWithValue("@lname", rundata.LName);

					//runnerId = Convert.ToInt32(cmdS.ExecuteScalar());

					string sqlI = $"INSERT INTO rundata (runner_id, distance, total_seconds, average_seconds) " +
							$"VALUES (@runner_id, @distance, @total_seconds, @average_seconds); SELECT CAST(scope_identity() AS int)";
					SqlCommand cmdI = new SqlCommand(sqlI, conn);
					cmdI.Parameters.AddWithValue("@runner_id", runnerId);
					cmdI.Parameters.AddWithValue("@distance", rundata.Distance);
					cmdI.Parameters.AddWithValue("@total_seconds", rundata.TotalSeconds);
					cmdI.Parameters.AddWithValue("@average_seconds", rundata.AverageSeconds);

					rundata.Id = (int)cmdI.ExecuteScalar();
				}
			}
			catch (SqlException ex)
			{
				throw;
			}
			return rundata;
		}

		public IList<PastRun> ShowHistory(int runnerId)
		{
			List<PastRun> runs = new List<PastRun>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sql = $"SELECT * FROM rundata WHERE runner_id = @runner_id;";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@runner_id", runnerId);

					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						PastRun run = new PastRun();
						run.Distance = Convert.ToDouble(reader["distance"]);
						run.Hours = run.ConvertTotalSeconds(Convert.ToInt32(reader["total_seconds"]))[0];
						run.Minutes = run.ConvertTotalSeconds(Convert.ToInt32(reader["total_seconds"]))[1];
						run.Seconds = run.ConvertTotalSeconds(Convert.ToInt32(reader["total_seconds"]))[2];
						run.AverageMinutes = run.ConvertAverageSeconds(Convert.ToInt32(reader["average_seconds"]))[0];
						run.AverageSeconds = run.ConvertAverageSeconds(Convert.ToInt32(reader["average_seconds"]))[1];
						run.PostDate = Convert.ToDateTime(reader["post_date"]);

						runs.Add(run);
					}
				}
			}
			catch (SqlException ex)
			{
				throw;
			}

			return runs;
		}

		public bool InsertWayPoints(DataTable dt)
        {
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					SqlCommand cmd = new SqlCommand("WayPoints__Insert", conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@tableWayPoints", dt);
					//cmd.Parameters.Add(prmReturn);
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				throw;
			}

			return true;
		}

		public int GetUserID(string fname, string lname, string emailAddress)
		{
			int runnerId;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string firstName = "%" + fname + "%";
					string lastName = "%" + lname + "%";

					string sqlS = $"SELECT id FROM runner WHERE ((@fname is null and @lname is null) or (fname like @fname AND lname like @lname)) and ((@emailAddress is null) or (emailAddress = @emailAddress));";
					SqlCommand cmdS = new SqlCommand(sqlS, conn);
					cmdS.Parameters.AddWithValue("@fname", firstName ?? (object)DBNull.Value);
					cmdS.Parameters.AddWithValue("@lname", lastName ?? (object)DBNull.Value);
					cmdS.Parameters.AddWithValue("@emailAddress", emailAddress ?? (object)DBNull.Value);

					runnerId = Convert.ToInt32(cmdS.ExecuteScalar());
				}
			}
			catch (SqlException ex)
			{
				throw;
			}
			return runnerId;
		}

		public User GetUser(string fname, string lname, string emailAddress)
		{
			User runner = new User();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string firstName = "%" + fname + "%";
					string lastName = "%" + lname + "%";

					string sql = $"SELECT * FROM runner WHERE ((@fname is null and @lname is null) or (fname like @fname AND lname like @lname)) and ((@emailAddress is null) or (emailAddress = @emailAddress));";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@fname", firstName ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@lname", lastName ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@emailAddress", emailAddress ?? (object)DBNull.Value);

					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						//runner.ID = covert.ToString(reader["id"]);
						runner.FName = Convert.ToString(reader["fName"]);
						runner.FName = Convert.ToString(reader["lName"]);
						runner.EmailAddress = Convert.ToString(reader["emailAddress"]);
					}
				}
			}
			catch (SqlException ex)
			{
				throw;
			}
			return runner;
		}
	}
}
