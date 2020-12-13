using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OAuthServer.OpenIdDict
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.ConfigureKestrel(options =>
                    {
                        options.ConfigureHttpsDefaults(adapterOptions =>
                        {
                            adapterOptions.ServerCertificate = CertHelper.GetCertificate();
                        });
                    });
                    
                    builder.ConfigureAppConfiguration(configurationBuilder =>
                    {
                        configurationBuilder.AddEnvironmentVariables();
                    });
                    builder.UseStartup<Startup>();
                }); 
        }
    }
}
