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
// The user's db record may have changed since they last logged in.
// Their permissions or password may have changed.  Their old login cookies should not be reusable.
// This class is used to validate the login cookie.

namespace CCSInventory
{

    public class LoginValidator : CookieAuthenticationEvents
    {
        private CCSDbContext dbContext;

        public LoginValidator(CCSDbContext context)
        {
            dbContext = context;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;
            string claimsUser = userPrincipal.Claims
                                    .Where(c => c.Type == ClaimTypes.Name)
                                    .Select(c => c.Value).FirstOrDefault();
            var claimsLastModified = userPrincipal.Claims
                                    .Where(c => c.Type == "LastModified")
                                    .Select(c => c.Value).FirstOrDefault();

            if (String.IsNullOrEmpty(claimsUser) || String.IsNullOrEmpty(claimsLastModified))
            {
                // Don't even hit the database.  Log them out if the username or the LastModified is empty:
                context.RejectPrincipal();
                await context.HttpContext
                    .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                // Check the DB. If this runs on every request, it may make sense to index the user entity
                DateTime lastModified = await dbContext.Users.AsNoTracking()
                                                .Where(u => u.UserName == claimsUser)
                                                .Select(u => u.Modified)
                                                .SingleOrDefaultAsync();
                if (lastModified.ToString() != claimsLastModified)
                {
                    context.RejectPrincipal();
                    await context.HttpContext
                        .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
                // Seems we don't do anything else if it's valid.
            }
        }
    }
}
