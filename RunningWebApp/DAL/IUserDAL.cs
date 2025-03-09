using RunningWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RunningWebApp.DAL
{
    public interface IUserDAL
    {
        /// <summary>
        /// Retrieves a user from the system by username.
        /// </summary>
        /// <param name="username">The username of the user to get.</param>
        /// <returns></returns>
        RunningWebApp.Models.User GetUser(string username);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user object to create in the database</param>
        void CreateUser(User user);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user">The user object that represents the </param>
        void UpdateUser(User user);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="user"></param>
        void DeleteUser(User user);
    }
}
