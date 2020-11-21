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

namespace AuctionCars
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs\\AllLogs.txt")
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Verbose)
                .WriteTo.File("Logs\\ErrorLogs.txt"))
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
                .WriteTo.Console())
                .CreateLogger();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
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
        
    }
}
