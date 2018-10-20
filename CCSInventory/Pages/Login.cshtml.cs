using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics; // for Stopwatch()
using Microsoft.Extensions.Logging;

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
                
                // Login is successful here.  Add Authentication Cookie now.
                //TODO: Finish login authentication
                // From TopsyTurvyCakes project:
                // var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                //     new Claim(ClaimTypes.Name, EmailAddress)
                // }, scheme));
                // return SignIn(user, scheme);

                return RedirectToPage("Index");
            }
        }

        public IActionResult OnGetLogout(){
            //TODO: Finish logout
            return Redirect("~/");
        }
    }
}
