using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AuctionCars.Components;
using AuctionCars.DB;
using AuctionCars.Hubs;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Repo;
using Services;
using Services.Abstract;
using Services.Entity;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace AuctionCars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("AuctionCars")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ICarLotsRepository, CarLotRepository>();
            services.AddTransient<IBetPerository, BetRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();
            services.AddTransient<ILikesRepository, LikesRepository>();
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEmail, Email>();
            

            services.AddHangfire(x => x.UseSqlServerStorage(
                Configuration.GetConnectionString("DefaultConnection")));

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            var supportedCultures = new[]
            {
                new CultureInfo("ru"),
                new CultureInfo("en"),
                new CultureInfo("de")
            };
            services.AddScoped<CarData>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                };
            });

            
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["google-ClientId"];
                    options.ClientSecret = Configuration["google-ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration["facebook-AppId"];
                    options.AppSecret = Configuration["facebook-AppSecret"];
                });

            
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();


            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                
            }
            if(env.IsProduction())
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRequestLocalization();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer();
            app.UseHangfireDashboard();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Lots}/{action=Actual}/{id?}");
                endpoints.MapHub<UpdateHub>("/updates");
                endpoints.MapControllers();
            });
        }
    }
}
