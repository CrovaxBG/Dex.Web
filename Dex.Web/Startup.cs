using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Infrastructure.Implementations.Services;
using Dex.Web.MappingConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DexContext = Dex.DataAccess.Models.DexContext;

namespace Dex.Web
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            var host = Configuration.GetSection("Data").GetSection("Host").Value;
            var projectsControllerBaseAddress = new Uri(host + "api/projects/");
            var projectFavoritesControllerBaseAddress = new Uri(host + "api/projectfavorites/");
            var loggerControllerBaseAddress = new Uri(host + "api/logger/");

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Home/Index", "");

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddMvcOptions(options => options.EnableEndpointRouting = false);

            services.AddDbContext<DexContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(provider => new DexContext(ConnectionString));

            services.AddIdentity<AspNetUsers, AspNetRoles>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<DexContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/LogIn";
                options.LogoutPath = $"/Identity/LogOut";
                options.AccessDeniedPath = $"/Home/Index";
            });

            services.AddSingleton<IConfiguration>(Configuration);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<EntityProfile>();
                mc.AddProfile<ViewModelProfile>();
            });
            
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped<SmtpClient>(provider =>
            {
                var host = Configuration.GetValue<string>("Data:Smtp:Host");
                var port = Configuration.GetValue<int>("Data:Smtp:Port");
                var email = Configuration.GetValue<string>("Data:Smtp:Email");
                var password = Configuration.GetValue<string>("Data:Smtp:Password");

                return new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(email, password)
                };
            });

            services.AddHttpClient<ILoggerService, LoggerService>(client =>
            {
                client.BaseAddress = loggerControllerBaseAddress;
            });

            services.AddHttpClient<IProjectsService, ProjectsService>(client =>
            {
                client.BaseAddress = projectsControllerBaseAddress;
            });

            services.AddHttpClient<IProjectFavoritesService, ProjectFavoritesService>(client =>
            {
                client.BaseAddress = projectFavoritesControllerBaseAddress;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role,"Admin");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "{area=Home}/{page=Index}");


                routes.MapRoute(
                    name: "defaultControllers",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "defaultControllers",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
