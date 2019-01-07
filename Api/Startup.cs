using System;
using System.Reflection;
using API.Extensions;
using AutoMapper;
using Dal;
using Logic;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;
using static API.Utilities.ConnectionStringUtility;

namespace Api
{
    public class Startup
    {
        private Container _container;

        private readonly IHostingEnvironment _env;

        private readonly IConfigurationRoot _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddRouting(options => { options.LowercaseUrls = true; });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(50);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ApiConstants.AuthenticationSessionCookieName;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            // All the other service configuration.
            services.AddAutoMapper(x => { x.AddProfiles(Assembly.Load("Models")); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Streaming-Subscription", Version = "v1",
                        Description = "Music Streaming Subscription"
                    });
            });

            var dbContext = new EntityDbContext(builder =>
            {
                if (_env.IsLocalhost())
                {
                    builder.UseSqlite(_configuration.GetValue<string>("ConnectionStrings:Sqlite"));
                }
                else
                {
                    builder.UseNpgsql(
                        ConnectionStringUrlToResource(Environment.GetEnvironmentVariable("DATABASE_URL"))
                        ?? throw new Exception("DATABASE_URL is null"));
                }
            });

            // Dependency injection
            _container = new Container(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup));
                    _.Assembly("Logic");
                    _.Assembly("Dal");
                    _.WithDefaultConventions();
                });

                config.For<EntityDbContext>().Use(dbContext).Singleton();

                config.For<IStreamRipperManagement>().Use<StreamRipperManagement>().Singleton();

                // Populate the container using the service collection
                config.Populate(services);
            });

            services.AddSingleton<DbContext>(dbContext);

            services.AddDataProtection();

            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<DbContext>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(googleOptions =>
            {
                var section = _configuration.GetSection("Authentication:Google");

                googleOptions.ClientId = section.GetValue<string>("ClientId");
                googleOptions.ClientSecret = section.GetValue<string>("ClientSecret");
            });

            services.AddMvc(x =>
                {
                    x.ModelValidatorProviders.Clear();

                    // Not need to have https
                    x.RequireHttpsPermanent = false;
                }).AddJsonOptions(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            return _container.GetInstance<IServiceProvider>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (_env.IsLocalhost())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            }

            app.UseCors();

            app.UseDeveloperExceptionPage();

            app.UseCookiePolicy();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}"); });

            app.UseStaticFiles();

            // Just to make sure everything is running fine
            _container.GetInstance<EntityDbContext>();

            Console.WriteLine("Application Started!");
        }
    }
}