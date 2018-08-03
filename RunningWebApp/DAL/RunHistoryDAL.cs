using System;
using System.Collections.Generic;
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

		public int AddToHistory(RunData rundata)
		{
			int runnerId;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sqlS = $"SELECT id FROM runner WHERE fname = @fname AND lname = @lname;";
					SqlCommand cmdS = new SqlCommand(sqlS, conn);
					cmdS.Parameters.AddWithValue("@fname", rundata.FName);
					cmdS.Parameters.AddWithValue("@lname", rundata.LName);

					runnerId = Convert.ToInt32(cmdS.ExecuteScalar());

					string sqlI = $"INSERT INTO rundata (runner_id, distance, total_seconds, average_seconds) " +
							$"VALUES (@runner_id, @distance, @total_seconds, @average_seconds);";
					SqlCommand cmdI = new SqlCommand(sqlI, conn);
					cmdI.Parameters.AddWithValue("@runner_id", runnerId);
					cmdI.Parameters.AddWithValue("@distance", rundata.Distance);
					cmdI.Parameters.AddWithValue("@total_seconds", rundata.TotalSeconds);
					cmdI.Parameters.AddWithValue("@average_seconds", rundata.AverageSeconds);

					cmdI.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				throw;
			}
			return runnerId;
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

		public int GetUserID(string fname, string lname)
		{
			int runnerId;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sqlS = $"SELECT id FROM runner WHERE fname = @fname AND lname = @lname;";
					SqlCommand cmdS = new SqlCommand(sqlS, conn);
					cmdS.Parameters.AddWithValue("@fname", fname);
					cmdS.Parameters.AddWithValue("@lname", lname);

					runnerId = Convert.ToInt32(cmdS.ExecuteScalar());
				}
			}
			catch (SqlException ex)
			{
				throw;
			}
			return runnerId;
		}
	}
}
