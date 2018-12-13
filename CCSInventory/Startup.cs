using CCSInventory.Middleware;
using CCSInventory.Models;
using CCSInventory.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCSInventory
{

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
            // Configuration of the DbContext (Connection strings are defined in appsettings.json):
            // CCSDbContext: The app's primary data, and likely only Db.  Logging may be folded into this DB
            //services.AddDbContext<CCSDbContext>(o => o.UseSqlServer(AppSettings["Databases:Production:TitanConnectionString"]));
            services.AddDbContext<CCSDbContext>(o => o.UseSqlite(AppSettings["Databases:Development:SQLiteConnectionString"]));

            // C#, by convention, uses PascalCasing.  By default, urls also have PascalCasing
            // This makes urls lowercase instead.
            services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

            // Setting the configuration options for cookie authentication.
            // The Login/Logout paths below are so that the framework can automatically redirect
            // users to the proper pages.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
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
            services.AddAuthorization(o =>
            {
                o.AddPolicy("ReadonlyUser", policy => policy.RequireClaim("UserRole", UserRole.READONLY.ToString(), UserRole.STANDARD.ToString(), UserRole.ADMIN.ToString()));
                o.AddPolicy("StandardUser", policy => policy.RequireClaim("UserRole", UserRole.STANDARD.ToString(), UserRole.ADMIN.ToString()));
                o.AddPolicy("AdminUser", policy => policy.RequireClaim("UserRole", UserRole.ADMIN.ToString()));
            });

            // Authorization (requiring login) of Razor Pages can also be done with the [Authorize]
            // attribute like in the MVC controllers.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1) // For backwards compatibility in later versions?
                .AddRazorPagesOptions(o =>
                {
                    o.Conventions.AuthorizePage("/Index");
                    o.Conventions.AuthorizePage("/Help");
                    o.Conventions.AuthorizeFolder("/Account");
                    o.Conventions.AllowAnonymousToPage("/Account/Login");
                    o.Conventions.AllowAnonymousToPage("/Account/DisabledUser");
                    o.Conventions.AllowAnonymousToFolder("/Status");
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Status Code Pages are so the user receives the status error code message rather
            // than just a blank page.
            app.UseStatusCodePages();
            // For custom pages, like our /Status/404 Razor Page
            app.UseStatusCodePagesWithReExecute("/Status/{0}");
            app.UseStaticFiles();

            // For the barcode generator:
            // Implemented in Middleware/BarcodeMiddleware.cs
            app.UseBarcodeGenerator();

            // Order matters here.  Authentication needs to be specified before MVC
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
