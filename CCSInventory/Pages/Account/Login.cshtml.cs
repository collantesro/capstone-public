using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics; // for Stopwatch()
using System.Security.Claims;
using System.Threading.Tasks;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels;

// Microsoft API Reference:
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.1&tabs=aspnetcore2x

namespace CCSInventory.Pages {
    public class LoginModel : PageModel {
        [BindProperty]
        public UserLogin Login { get; set; }

        // For redirects back to the page they were viewing
        // [BindProperty]
        // public string ReturnUri { get; set; }

        private CCSDbContext _context;
        private ILogger<LoginModel> _log;

        public LoginModel(CCSDbContext context, ILogger<LoginModel> logger){
            _context = context;
            _log = logger;
        }

        public IActionResult OnGet(){
            // If they're already logged in, just redirect to the index.
            if(User.Identity.IsAuthenticated){
                return Redirect("~/");
            } else {
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(){
            if(!ModelState.IsValid){
                return Page();
            } else {
                // For security, prevent instant feedback for failed login
                var watch = new Stopwatch();
                watch.Start();
                // Pull user from DB
                User user = await _context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.UserName.ToLower() == Login.UserName.ToLower());

                if(user == null || !user.MatchesPassword(Login.Password)){
                    ModelState.AddModelError("", "Login Failed: Invalid username or password");
                    // Log the failed login in the Logger
                    _log.LogWarning($"Failed login attempt for {user?.UserName ?? "unknown user " + Login.UserName} " +
                    $"from IP address {HttpContext.Connection.RemoteIpAddress}");

                    // Delay the response to the user since the login failed.
                    watch.Stop();
                    int toSleepMs = (int)(500/*ms*/ - watch.ElapsedMilliseconds);
                    _log.LogDebug($"Delaying response by {toSleepMs}ms after failed login attempt");
                    if(toSleepMs > 0){
                        await Task.Delay(toSleepMs);
                    }

                    return Page();
                }

                // User and Password matches, but check they're not a disabled user:
                if(user.Role == UserRole.DISABLED){
                    return RedirectToPage("DisabledUser");
                }
                
                // Login is successful here.  Add Authentication Cookie now.
                var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserRole", user.Role.ToString()),
                    new Claim("LastModified", user.Modified.ToString()),
                }, scheme));

                // The UserRole claim is used to discriminate app features based on UserRole.
                //
                // The LastModified claim is used in the Validation logic to revoke the cookie
                // if the user was modified in the database since their last login.
                // For example, say a logged in user leaves the company. When their Role is
                // changed from STANDARD to DISABLED, they shouldn't have access anymore.
                // We can compare the LastModified field with the DB in the Validator.

                await HttpContext.SignInAsync(scheme, claimsPrincipal);
                return Redirect("~/");
            }
        }

        public async Task<IActionResult> OnGetLogout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }
    }
}
