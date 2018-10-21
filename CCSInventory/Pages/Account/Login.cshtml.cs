using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics; // for Stopwatch()
using System.Security.Claims;
using System.Threading.Tasks;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels;

// Microsoft API Reference:
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.1&tabs=aspnetcore2x

namespace CCSInventory.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserLogin Login { get; set; }

        [BindProperty]
        [DisplayName("Remember me")]
        public bool PersistLogin {get; set;}

        private readonly CCSDbContext _context;
        private readonly ILogger<LoginModel> _log;

        public LoginModel(CCSDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _log = logger;
        }

        public IActionResult OnGet(string returnUrl = "~/")
        {
            // If they're already logged in, just redirect the user without doing anything
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string returnUrl = "~/")
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                // For security, prevent instant feedback for failed login
                var watch = new Stopwatch();
                watch.Start();
                // Pull user from DB
                User user = await _context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.UserName.ToLower() == Login.UserName.ToLower());

                if (user == null || !user.MatchesPassword(Login.Password))
                {
                    ModelState.AddModelError("", "Login Failed: Invalid username or password");
                    // Log the failed login in the Logger
                    _log.LogWarning($"Failed login attempt for {user?.UserName ?? "unknown user " + Login.UserName} " +
                    $"from IP address {HttpContext.Connection.RemoteIpAddress}");

                    // Delay the response to the user since the login failed.
                    watch.Stop();
                    int toSleepMs = (int)(500/*ms*/ - watch.ElapsedMilliseconds);
                    _log.LogDebug($"Delaying response by {toSleepMs}ms after failed login attempt");
                    if (toSleepMs > 0)
                    {
                        await Task.Delay(toSleepMs);
                    }

                    return Page();
                }

                // User and Password matches, but check they're not a disabled user:
                if (user.Role == UserRole.DISABLED)
                {
                    _log.LogInformation($"Login attempted for disabled user {user.UserName} " +
                        $"from IP address {HttpContext.Connection.RemoteIpAddress}");
                    return RedirectToPage("DisabledUser");
                }

                _log.LogInformation($"Successful login by user {user.UserName}");

                // Login is successful here.  Add Authentication Cookie now.
                var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserRole", user.Role.ToString()),
                    new Claim("LastModified", user.Modified.ToString()),
                }, scheme));

                // The UserRole claim is used to discriminate app features based on UserRole.
                //
                // The LastModified claim is used in the Validation logic (LoginValidator) to
                // revoke the cookie if the user was modified in the database since their login.
                // For example, say a logged in user leaves the company. When their Role is
                // changed from STANDARD to DISABLED, they shouldn't have access anymore.
                // We can compare the LastModified field with the DB in the Validator.

                await HttpContext.SignInAsync(scheme, claimsPrincipal,
                    new AuthenticationProperties{
                        IsPersistent = PersistLogin,
                    }
                );
                return Redirect(returnUrl);
            }
        }

        public async Task<IActionResult> OnGetLogout(string returnUrl = "~/")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(returnUrl);
        }
    }
}
