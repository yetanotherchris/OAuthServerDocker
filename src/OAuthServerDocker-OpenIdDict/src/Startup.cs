﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static OpenIddict.Abstractions.OpenIddictConstants;
using OAuthServerDocker.OpenIdDict.Models;

namespace OAuthServerDocker.OpenIdDict
{
    public class Startup
    {
        private const string ConnectionString =
            "host=localhost;port=5432;database=openiddict;username=openiddict;password=openiddict;";

        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(ConnectionString);
                    options.UseOpenIddict();
                });
            
            services
                .AddIdentityCore<ApplicationUser>()
                .AddSignInManager()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            // The login cookie
            services.AddAuthentication("Identity.Application")
                .AddCookie("Identity.Application",
                    options =>
                    {
                        options.Cookie.Name = "openid-dict";
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                        options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    });

            services
                .AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                           .UseDbContext<ApplicationDbContext>();
                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    options.DisableAccessTokenEncryption();

                    // Enable the token endpoint.
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                           .SetLogoutEndpointUris("/connect/logout")
                           .SetTokenEndpointUris("/connect/token")
                           .SetUserinfoEndpointUris("/connect/userinfo");

                    options.RegisterScopes(Scopes.OpenId, Scopes.OfflineAccess);

                    // Enable the client credentials flow.
                    options.AllowAuthorizationCodeFlow()
                            .AllowRefreshTokenFlow();

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();

                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    options.UseAspNetCore()
                           .EnableAuthorizationEndpointPassthrough()
                           .EnableLogoutEndpointPassthrough()
                           .EnableStatusCodePagesIntegration()
                           .EnableTokenEndpointPassthrough();
                })

                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });


            services.AddControllersWithViews();
            services.AddRazorPages();

            // Register the worker responsible of seeding the database with the sample clients.
            // Note: in a real world application, this step should be part of a setup script.
            services.AddHostedService<DummyDataWorker>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
                options.MapDefaultControllerRoute();
                options.MapRazorPages();
            });

            app.UseWelcomePage();
        }
    }
    
    public class ApplicationUser : IdentityUser
    {
    }
}
