using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

using CCSInventory.Models;

namespace CCSInventory
{
    // https://go.microsoft.com/fwlink/?LinkID=398940
    public class Startup
    {
        private IConfiguration AppSettings { get; }
        
        public Startup(IConfiguration config)
        {
            AppSettings = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CCSDbContext>(o => o.UseSqlite(AppSettings["Databases:Production:SQLiteConnectionString"]));
            services.AddDbContext<CCSLogDbContext>(o => o.UseSqlite(AppSettings["Databases:Logging:SQLiteConnectionString"]));
            services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => {
                    o.EventsType = typeof(LoginValidator);
                    o.LoginPath = "/account/login";
                    o.LogoutPath = "/account/login/logout";
                    o.ReturnUrlParameter = "returnUrl";
                    o.Cookie.HttpOnly = false; // Makes it so the cookie can be sent over ajax requests
                    // Apparently it's a potential XSS vulnerability, though
                });
            services.AddScoped<LoginValidator>(); // For lines above

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
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
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
