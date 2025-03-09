using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.Providers.Auth
{
    public class AuthorizationFilterAttribute : Attribute, IActionFilter
    {
        private string[] roles;

        public AuthorizationFilterAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authProvider = context.HttpContext.RequestServices.GetService<IAuthProvider>();

            if (!authProvider.IsLoggedIn)
            {
                // Force the user to login first
                context.Result = new RedirectToRouteResult(new
                {
                    controller = "account",
                    action = "login",                    
                });
                return;
            }

            if (roles.Length > 0 && !authProvider.UserHasRole(roles))
            {
                // User shouldn't have access
                context.Result = new StatusCodeResult(403);
                return;
            }
        }
    }
}
