using RunningWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.Providers.Auth
{
    public interface IAuthProvider
    {
        /// <summary>
        /// Returns true if a current user is logged in.
        /// </summary>
        /// <returns></returns>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Returns the current signed in user.
        /// </summary>
        /// <returns></returns>
        User GetCurrentUser();

        /// <summary>
        /// Signs a user in.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if the user signed in.</returns>
        bool SignIn(string emailAddress, string password);

        /// <summary>
        /// Logs the user off from the system.
        /// </summary>
        void LogOff();

        /// <summary>
        /// Changes the logged in user's existing password.
        /// </summary>
        /// <param name="existingPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool ChangePassword(string existingPassword, string newPassword);

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        void Register(string fname, string lname, string email, string password, string role);

        /// <summary>
        /// Checks to see if a user has a given role.
        /// </summary>
        /// <param name="roles">One of the roles that the user can belong to.</param>
        /// <returns></returns>
        bool UserHasRole(string[] roles);
    }
}
