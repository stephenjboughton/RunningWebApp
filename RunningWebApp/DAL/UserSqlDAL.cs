using RunningWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.DAL
{
    public class UserSqlDAL : IUserDAL
    {
        /// <summary>
        /// The connection string used to access the database.
        /// </summary>
        private readonly string connectionString;

        public UserSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Saves the user to the database.
        /// </summary>
        /// <param name="user">The user object that represents the user to be added to the database.</param>
        public void CreateUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Th SQL statement that adds a new user to the database.
                    SqlCommand command = new SqlCommand("INSERT INTO runner VALUES (@fname, @lname, @emailaddress, @password, @salt, @role);", connection);
                    command.Parameters.AddWithValue("@fname", user.FName);
                    command.Parameters.AddWithValue("@lname", user.LName);
                    command.Parameters.AddWithValue("@emailaddress", user.EmailAddress);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@salt", user.Salt);
                    command.Parameters.AddWithValue("@role", user.Role);

                    command.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        /// <param name="user">The user object that that represents the user to be removed from the database.</param>
        public void DeleteUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the user from the database.
        /// </summary>
        /// <param name="username">The username of the user to get info for.</param>
        /// <returns>User object that mirrors the username in the database.</returns>
        public User GetUser(string emailAddress)
        {
            User user = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM runner WHERE emailAddress = @emailAddress;", conn);
                    // $"SELECT * FROM runner WHERE emailAddress = @emailAddress;";
                    cmd.Parameters.AddWithValue("@emailAddress", emailAddress);

                    int runnerId = Convert.ToInt32(cmd.ExecuteScalar());
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.Read())
                    {
                       
                        user = MapRowToUser(reader);
                    }
                }

                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the user in the database.
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET password = @password, salt = @salt, role = @role WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Maps the information in a database row to a user object.
        /// </summary>
        /// <param name="reader">The reader that is being used to read through the database.</param>
        /// <returns>A user object that corresponds to the row the reader was on.</returns>
        private User MapRowToUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = Convert.ToInt32(reader["id"]),
                FName = Convert.ToString(reader["fname"]),
                LName = Convert.ToString(reader["lname"]),
                EmailAddress = Convert.ToString(reader["emailAddress"]),
                Password = Convert.ToString(reader["password"]),
                Salt = Convert.ToString(reader["salt"]),
                Role = Convert.ToString(reader["role"]),
            };
        }
    }
}
