using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CCSInventory.Models;

namespace CCSInventory {
    // https://go.microsoft.com/fwlink/?LinkID=398940
    public class Startup
    {
        /// <summary>
        /// This field provides access to the appsettings*.json configuration files
        /// </summary>
        /// <value></value>
        private IConfiguration AppSettings { get; }
        
        public Startup(IConfiguration config)
        {
            AppSettings = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration of the two DbContext (Connection strings are defined in appsettings.json):
            // CCSDbContext: The app's primary data.
            // CCSLogDbContext: So far unused, this context is for storing logs.  The contents
            //     of this database are not required for the rest of the app.
            services.AddDbContext<CCSDbContext>(o => o.UseSqlite(AppSettings["Databases:Production:SQLiteConnectionString"]));
            services.AddDbContext<CCSLogDbContext>(o => o.UseSqlite(AppSettings["Databases:Logging:SQLiteConnectionString"]));
            
            // C#, by convention, uses PascalCasing.  By default, urls also have PascalCasing
            // This makes urls lowercase instead.
            services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

            // Setting the configuration options for cookie authentication.
            // The Login/Logout paths below are so that the framework can automatically redirect
            // users to the proper pages.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => {
                    o.EventsType = typeof(LoginValidator); // To validate the user's session against the database
                    o.LoginPath = "/account/login";
                    o.LogoutPath = "/account/login/logout";
                    o.AccessDeniedPath = "/account/accessdenied"; // Already the default.  Razor page is: Pages/Account/AccessDenied.cshtml
                    o.ReturnUrlParameter = "returnUrl"; // from PascalCasing to camelCasing. Default is "ReturnUrl"
                    o.Cookie.HttpOnly = false; // Makes it so the cookie can be sent over ajax requests
                    // Apparently it's a potential XSS vulnerability, though
                });
            services.AddScoped<LoginValidator>(); // For authentication above

            // These authorizations are to restrict access to specific subsets of users.  For example, an authorization of
            // ReadonlyUser will only allow access to users of Role READONLY, STANDARD, or ADMIN.
            // StandardUser will only allow access to users of Role STANDARD, or ADMIN.
            // AdminUser will only allow access to users of Role ADMIN *only*.
            // To use it, decorate your controller or action with the AuthorizeAttribute:
            // [Authorize("AdminUser)] 
            // public class AdminController {...}
            // see how "AdminUser" in the above attribute matches the policy below?
            //
            // For Razor Pages, put the [Authorize()] attribute on the PageModel.
            // Users that try to access a page without proper authorization will be redirected
            // to /account/accessdenied.
            services.AddAuthorization(o => {
                o.AddPolicy("ReadonlyUser", policy => policy.RequireClaim("UserRole", UserRole.READONLY.ToString(), UserRole.STANDARD.ToString(), UserRole.ADMIN.ToString()));
                o.AddPolicy("StandardUser", policy => policy.RequireClaim("UserRole", UserRole.STANDARD.ToString(), UserRole.ADMIN.ToString()));
                o.AddPolicy("AdminUser", policy => policy.RequireClaim("UserRole", UserRole.ADMIN.ToString()));
            });

            // Authorization (requiring login) of Razor Pages can also be done with the [Authorize]
            // attribute like in the MVC controllers.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1) // For backwards compatibility in later versions?
                .AddRazorPagesOptions(o => {
                    o.Conventions.AuthorizePage("/Index");
                    o.Conventions.AuthorizeFolder("/Account");
                    o.Conventions.AllowAnonymousToPage("/Account/Login");
                    o.Conventions.AllowAnonymousToPage("/Account/DisabledUser");
                });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Status Code Pages are so the user receives the 404 error code page rather
            // than just a blank page.
            app.UseStatusCodePages();
            app.UseStaticFiles();

            // Order matters here.  Authentication needs to be specified before MVC
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
