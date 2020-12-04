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
            services.AddSingleton<IEmail, Email>();

            services.AddHangfire(x => x.UseSqlServerStorage(
                Configuration.GetConnectionString("DefaultConnection")));

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
            services.AddControllers();

            
                

            var supportedCultures = new[]
            {
                new CultureInfo("ru"),
                new CultureInfo("en"),
                new CultureInfo("de")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
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
                    options.ClientId = "553190241471-1mfbonihr15q6jahim861cadpamqj7oo.apps.googleusercontent.com";
                    options.ClientSecret = "OjdFMvWKYnjr5EAxqZV1975h";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "720261328615445";
                    options.AppSecret = "60ec6a61050e16094cf0b13c04056db2";
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

            services.AddTransient<CarData>();

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
                app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseRequestLocalization();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Lots}/{action=Actual}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Detail",
                    pattern: "{controller=Lots}/{action=Detail}/{id?}",
                    defaults: new {Controller = "Lots", Action = "Detail"}
                    );
                endpoints.MapControllerRoute(
                    name: "Profile",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Users",
                    pattern: "{controller=Users}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Add",
                    pattern: "{controller=Users}/{action=CreateUser}/{id?}",
                    defaults: new { Controller = "Users", Action = "CreateUser" });
                endpoints.MapControllerRoute(
                    name: "Logout",
                    pattern: "{controller=Account}/{action=Logout}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Create",
                    pattern: "{controller=Lots}/{action=Create}",
                    defaults: new { Controller = "Lots", Action = "Create" }
                    );


                endpoints.MapControllerRoute(name: "loadLots", pattern: "Lots/Load/{id:int?}", defaults: new { Controller = "Lots", Action = "Load"});
                endpoints.MapControllerRoute(name: "actualLots", pattern: "Lots/Actual/{id:int?}", defaults: new { Controller = "Lots", Action = "Actual" });
                endpoints.MapHub<UpdateHub>("/updates");
                endpoints.MapControllers();
            });
        }
    }
}
