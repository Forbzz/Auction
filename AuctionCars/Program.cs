using System;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repo;
using Serilog;
using Serilog.Events;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace AuctionCars
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            

            using (var scope = host.Services.CreateScope())  
            {
                var services = scope.ServiceProvider;    
                try
                {
                    Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.File(path: "Logs\\AllLogs.txt", restrictedToMinimumLevel: LogEventLevel.Debug)
                    .WriteTo.File(path: "Logs\\ErrorLogs.txt", restrictedToMinimumLevel: LogEventLevel.Error)
                    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                    .CreateLogger();

                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await DB.RoleInitializer.InitializeAsync(userManager, roleManager);
                  
                    var context = services.GetRequiredService<ApplicationContext>();
                    SeedData.Initialize(services);
              
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
         .ConfigureWebHostDefaults(webBuilder =>
         {
             webBuilder.UseStartup<Startup>();
         })
         .UseSerilog(); 


        private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("VaultUri");  
    }
}
