using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Api.Attributes;
using Api.Configs;
using Api.Extensions;
using Dal.Interfaces;
using Dal.Utilities;
using EfCoreRepository.Extensions;
using IF.Lastfm.Core.Api;
using Logic.Interfaces;
using Logic.Providers;
using Logic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MlkPwgen;
using Models.Constants;
using Models.Models;
using Models.ViewModels.Config;
using Newtonsoft.Json;
using Refit;
using StructureMap;
using static Dal.Utilities.ConnectionStringUtility;

namespace Api;

public class Startup
{
    private readonly IWebHostEnvironment _env;

    private readonly IConfigurationRoot _configuration;

    /// <summary>
    /// Constructor
    /// </summary>uriString
    /// <param name="env"></param>
    public Startup(IWebHostEnvironment env)
    {
        _env = env;

        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
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
        services.AddMiniProfiler(opt =>
        {
            // opt.RouteBasePath = "/profiler";
            opt.ShouldProfile = _ => true;
            opt.ShowControls = true;
            opt.StackMaxLength = short.MaxValue;
            opt.PopupStartHidden = false;
            opt.PopupShowTrivial = true;
            opt.PopupShowTimeWithChildren = true;
        });
            
        services.AddHttpsRedirection(options => options.HttpsPort = 443);

        // If environment is localhost, then enable CORS policy, otherwise no cross-origin access
        services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder
            .WithOrigins(_configuration.GetSection("TrustedSpaUrls").Get<string[]>())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

        // Add framework services
        // Add functionality to inject IOptions<T>
        services.AddOptions();

        services.AddResponseCompression();

        services.Configure<JwtSettings>(_configuration.GetSection("JwtSettings"));

        services.AddLogging();
            
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddSession(options =>
        {
            // Set a short timeout for easy testing.
            options.IdleTimeout = TimeSpan.FromMinutes(60);
            options.Cookie.HttpOnly = true;
            options.Cookie.Name = ApiConstants.AuthenticationSessionCookieName;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Stream-Ripper-API", Version = "v1"});

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
                
            c.AddSecurityDefinition("Bearer", // Name the security scheme
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http, // We set the scheme type to http since we're using bearer authentication
                    Scheme = "bearer" // The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                });
        });

        services.AddMvc(x =>
        {
            x.ModelValidatorProviders.Clear();

            // Not need to have https
            x.RequireHttpsPermanent = false;

            // Allow anonymous for localhost
            if (_env.IsDevelopment())
            {
                x.Filters.Add<AllowAnonymousFilter>();
            }

            // Exception filter attribute
            x.Filters.Add<ExceptionFilterAttribute>();
                
        }).AddNewtonsoftJson(x =>
        {
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }).AddRazorPagesOptions(x => x.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

        services.AddDbContext<EntityDbContext>(opt =>
        {
            if (_env.IsDevelopment())
            {
                opt.UseSqlite(_configuration.GetValue<string>("ConnectionStrings:Sqlite"));
            }
            else
            {
                var postgresConnectionString =
                    ConnectionStringUrlToPgResource(_configuration.GetValue<string>("DATABASE_URL")
                                                    ?? throw new Exception("DATABASE_URL is null"));
                opt.UseNpgsql(postgresConnectionString);
            }
        });

        services.AddIdentity<User, IdentityRole<int>>(x => x.User.RequireUniqueEmail = true)
            .AddEntityFrameworkStores<EntityDbContext>()
            .AddRoles<IdentityRole<int>>()
            .AddDefaultTokenProviders();

        services.AddEfRepository<EntityDbContext>(x => x.Profile(Assembly.Load("Dal"), Assembly.Load("Models")));

        var jwtSetting = _configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>();

        if (_env.IsDevelopment() && string.IsNullOrEmpty(jwtSetting.Key))
        {
            jwtSetting.Key = PasswordGenerator.Generate(length: 100, allowed: Sets.Alphanumerics);
                
            IdentityModelEventSource.ShowPII = true;
        }
            
        services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;

                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSetting.Issuer,
                    ValidAudience = jwtSetting.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key))
                };
            });

        services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        services.AddSignalR(config =>
        {
            config.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10 mega-bytes
            config.StreamBufferCapacity = 50;
            config.EnableDetailedErrors = true;
        });

        var container = new Container(config =>
        {
            config.For<JwtSettings>().Use(jwtSetting).Singleton();
                
            // If environment is localhost then use mock email service
            if (_env.IsDevelopment())
            {

            }
            else
            {
                config.For<ISimpleConfigServer>().Use(x =>
                    RestService.For<ISimpleConfigServer>(_configuration.GetValue<string>("ConfigApi")));

                config.For<SimpleConfigServerApiKey>().Use(new SimpleConfigServerApiKey
                    {ApiKey = _configuration.GetRequiredValue<string>("CONFIG_KEY")});

                var (lastFmKey, lastFmSecret) = (
                    _configuration.GetRequiredValue<string>("last.fm:Key"),
                    _configuration.GetRequiredValue<string>("last.fm:Secret")
                );

                config.For<LastfmClient>().Use("last.FM", () => new LastfmClient(lastFmKey, lastFmSecret, new HttpClient()));
            }

            // Register stuff in container, using the StructureMap APIs...
            config.Scan(_ =>
            {
                _.AssemblyContainingType(typeof(Startup));
                _.Assembly("Api");
                _.Assembly("Logic");
                _.Assembly("Dal");
                _.WithDefaultConventions();
            });

            // Populate the container using the service collection
            config.Populate(services);
        });

        container.AssertConfigurationIsValid();

        return container.GetInstance<IServiceProvider>();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configLogic"></param>
    /// <param name="streamRipperManager"></param>
    /// <param name="shoutcastDirectoryApi"></param>
    public void Configure(IApplicationBuilder app, IConfigLogic configLogic, IStreamRipperManager streamRipperManager, IShoutcastDirectoryApi shoutcastDirectoryApi)
    {
        configLogic.Refresh().Wait();

        streamRipperManager.Refresh().Wait();

        shoutcastDirectoryApi.Setup();

        app.UseMiniProfiler();
            
        app.UseCors("CorsPolicy");

        app.UseResponseCompression();

        if (_env.IsDevelopment())
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }

        // Not necessary for this workshop but useful when running behind kubernetes
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            // Read and use headers coming from reverse proxy: X-Forwarded-For X-Forwarded-Proto
            // This is particularly important so that HttpContent.Request.Scheme will be correct behind a SSL terminating proxy
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto
        });

        // Use wwwroot folder as default static path
        app.UseDefaultFiles()
            .UseStaticFiles()
            .UseCookiePolicy()
            .UseSession()
            .UseRouting()
            .UseCors("CorsPolicy")
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapHub<MessageHub>("/hub");
            });

        Console.WriteLine("Application Started!");
    }
}