using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using CCSInventory.Models;

// Microsoft Documentation
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.1&tabs=aspnetcore2x#react-to-back-end-changes
// tl;dr:
// The user's record in the database may change at any time after they have logged in.
// For example, a user's permissions (role) or password may have changed.  Their old login cookies should not be reusable.
// This class is used by CookieAuthenticationEvents to validate the login cookie. 
// The reference to this class is in Startup.ConfigureServices,
// services.AddAuthentication(...).AddCookie(...)

namespace CCSInventory.Utilities
{
    /// <summary>
    /// Custom class extended from CookieAuthenticationEvents.  Used to validate the user's
    /// login cookie at each request.
    /// </summary>
    public class LoginValidator : CookieAuthenticationEvents
    {
        private CCSDbContext dbContext;

        public LoginValidator(CCSDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// This method validates a user's login cookie to ensure their record in the database has not changed since they logged in
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            // I'm not sure exactly why these are named ClaimsPrincipals, but it appears that ClaimsPrincipals are
            // authorization assertions applied to the user.  These specific claims referenced here were
            // set in the PageModel for the Login RazorPage.  Refer to Pages/Login.cshtml.cs method OnPostAsync()
            var userPrincipal = context.Principal;

            // This is the username
            string claimsUser = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
            var claimsLastModified = userPrincipal.FindFirst("LastModified")?.Value;

            if (String.IsNullOrEmpty(claimsUser) || String.IsNullOrEmpty(claimsLastModified))
            {
                // Don't even hit the database.  Log them out if the username or the LastModified is empty:
                context.RejectPrincipal();
                await context.HttpContext
                    .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                // Check with the DbContext.
                // Since this method will be called with each and every request made by the
                // end-user, there's a potential bottleneck here with the constant database queries.
                // According to CCS, the number of simultaneous users is usually around 5,
                // which shouldn't be too stressful.  If this becomes a bottleneck, consider adding
                // some kind of caching layer, either around the DbContext or at the server level.
                DateTime lastModified = await dbContext.Users.AsNoTracking()
                                                .Where(u => u.Username == claimsUser)
                                                .Select(u => u.ModifiedDate)
                                                .FirstOrDefaultAsync();
                if (lastModified.ToString() != claimsLastModified)
                {
                    context.RejectPrincipal();
                    await context.HttpContext
                        .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
                // According to the example code in the above link, there is nothing to be done
                // on a valid cookie.  The code simply returns.
            }
        }
    }
}
