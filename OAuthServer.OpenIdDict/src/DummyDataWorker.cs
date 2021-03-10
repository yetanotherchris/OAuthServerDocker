using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OAuthServer.OpenIdDict.Models;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OAuthServer.OpenIdDict
{
    public class DummyDataWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DummyDataWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var openIdDictManager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var client = await openIdDictManager.FindByClientIdAsync("client1");
            if (client != null)
            {
                await openIdDictManager.DeleteAsync(client);
            }

            var clientDetails = new OpenIddictApplicationDescriptor
            {
                ClientId = "client1",
                ClientSecret = "secret",
                RedirectUris = { new Uri("https://oidcdebugger.com/debug") },
                DisplayName = "My client application",
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code
                }
            };
            await openIdDictManager.CreateAsync(clientDetails);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var testUser = await userManager.FindByEmailAsync("test@localhost");
            if (testUser != null)
            {
                await userManager.DeleteAsync(testUser);
            }

            var newUser = new ApplicationUser()
            {
                Email = "test@localhost",
                EmailConfirmed = true,
                NormalizedEmail = "test@localhost",
                UserName = "test@localhost"
            };
            var result = await userManager.CreateAsync(newUser, "Password123+");
            if (!result.Succeeded)
                throw new Exception("Failed to create a test user.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
