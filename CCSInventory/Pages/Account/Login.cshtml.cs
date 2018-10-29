using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel; // This is for the [BindProperty], [DisplayName] attributes
using System.Diagnostics; // for Stopwatch()
using System.Security.Claims;
using System.Threading.Tasks;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels;

// Microsoft Documentation reference/tutorial for cookie authentication:
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.1&tabs=aspnetcore2x
// Upon successful login, the user's browser is given an authentication cookie.
// Specific controllers can discriminate access based on a user's role by using
// the [Authorize] attribute.  For example, all of the AdminController is only 
// accessible by users who have AdminUser policy (which is based off their
// UserRole being ADMIN).  That is marked with [Authorize("AdminUser")].
// The AdminUser policy is specified in Startup.ConfigureServices in the line
// services.AddAuthorization()


namespace CCSInventory.Pages
{
    /// <summary>
    /// This PageModel provides the logic behind the Login.cshtml Razor Page.  It's used to
    /// perform a user's login.  The user logs into the system by providing a username and
    /// a password.  Since this is a system internal to the client, there is no functionality
    /// provided to logged-out users to create a new account.  Please ask an administrator
    /// user to create an account through the admin console.
    /// </summary>
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserLogin Login { get; set; }
        // This is a ViewModel that only has two properties: UserName and Password.
        // The domain model of User (Models/User.cs) should not be bound, since the Password

        [BindProperty]
        [DisplayName("Remember me")]
        public bool PersistLogin {get; set;}
        // This boolean is bound to the "Remember me" checkbox in the login form.
        // In discussing it with the team, this may be removed later after considering the
        // security implications of allowing a user's browser to remain logged in
        // even after it has exited.  At least for development, this option exists to reduce
        // the friction of testing.

        // These private fields are set using dependency injection. (In the constructor)
        private readonly CCSDbContext _context;
        private readonly ILogger<LoginModel> _log;

        public LoginModel(CCSDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _log = logger;
        }

        /// <summary>
        /// This action method runs when the user visits the Login page.  If they're already
        /// authenticated/logged in, the user is redirected to the returnUrl URL parameter.
        /// If the returnUrl parameter is unspecified, it defaults to redirecting the user to
        /// the root.  Currently, this is corresponds to the Index razor page.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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

        // This is the primary logic of this page.  This is where we process the login attempt
        /// <summary>
        /// This action method runs when the user submits the form on the Login page.  The
        /// UserLogin property will be filled by the framework before running the method.
        /// This queries for the user from the database and runs its MatchesPassword() method
        /// to check the provided password.
        /// </summary>
        /// <param name="returnUrl">This parameter is used after a successful login.  The user is
        /// redirected to this value on a successful login.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string returnUrl = "~/")
        {
            // Thanks to [Required] data annotations in the UserLogin ViewModel, 
            // the framework can take care of empty username or passwords.
            // If UserName or Password are empty, this is false and the page reloads
            // with error messages.
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                // This stopwatch is used to prevent inadvertent leaking of the usernames in the database.
                // "Timing Attack" almost sounds like the appropriate term for this, but the wikipedia
                // article focuses on vulnerabilities in cryptographic systems.
                // The rationale for this timer is to always take at least 500ms to respond to the user
                // on an invalid login.  Due to password hashing, checking the password of an existing
                // username takes *much* longer than rejecting a login because the username doesn't exist.
                // Without the artificial delay, a hacker can use the time it took to return an answer
                // as confirmation that the user does or doesn't exist.  A minimum delay complicates this.
                var watch = new Stopwatch();
                watch.Start();
                // Pull user from DB in a read-only manner (the AsNoTracking()).
                User user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName.ToLower() == Login.UserName.ToLower());

                if (user == null || !user.MatchesPassword(Login.Password))
                {
                    // Keep this error message ambiguous.  It makes it a little more annoying for the
                    // user to not know if the it was the username or password that was incorrect, but
                    // it's supposed to be another hurdle for a theoretical attacker.
                    ModelState.AddModelError("", "Login Failed: Invalid username or password");
                    // Log the failed login attempt in the Logger
                    _log.LogWarning($"Failed login attempt for {user?.UserName ?? "unknown user " + Login.UserName} " +
                    $"from IP address {HttpContext.Connection.RemoteIpAddress}");

                    // Delay the response to the user since the login failed.
                    watch.Stop();
                    long toSleepMs = 500/*ms*/ - watch.ElapsedMilliseconds;
                    _log.LogDebug($"Delaying response by {toSleepMs}ms after failed login attempt");
                    if (toSleepMs > 0)
                    {
                        await Task.Delay((int)toSleepMs);
                    }

                    return Page();
                }

                // User and Password matches, but check they're not a disabled user:
                if (user.Role == UserRole.DISABLED)
                {
                    // No authentication cookie is set for this user.
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
                    new Claim("FullName", user.FullName),
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

        // This is called a "Handler".  It serves to log out the user.  To access this,
        // the user navigates to either /Account/Login?handler=Logout or, because the
        // @page directive at the top of Login.cshtml specifies that the handler may be
        // provided, it simplifies the url to simply /Account/Login/Logout.
        // By using Routes, it may be possible to rewrite that url to /Account/Logout
        /// <summary>
        /// This handler is used to log out the user.
        /// </summary>
        /// <param name="returnUrl">(defaults to "~/") The redirect path after the user is logged out/param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLogout(string returnUrl = "~/")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(returnUrl);
        }
    }
}
