using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using TodoApp.Data;
using TodoApp.Data.Collections;
using TodoApp.Data.Model;
using TodoApp.Security;

namespace TodoApp
{
    public class Startup
    {
        private IMongoDatabase database;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Create mongo connection
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(Configuration["Database:URL"])
            );
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            database = client.GetDatabase(Configuration["Database:Database"]);
            TodoAppData.Init(database);

            // Cors
#if DEBUG
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                    );
            });
#endif

            // Setup services
            services.AddControllers(); // Controllers

            // Auth
            services.AddAppIndentityManager(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            // Static web files
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            // Create root user
            await CreateRootAccess(serviceProvider);

            // Dev stuff
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // Setup HTTP stack
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCookiePolicy();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }

        private async Task CreateRootAccess(IServiceProvider services)
        {
            UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
            ILogger<Startup> logger = services.GetRequiredService<ILogger<Startup>>();

            User rootUser = new User()
            {
                Username = "root",
                FullName = "root",
                Roles = new string[] { "ROOT" },
                PasswordHash = null
            };

            if (UserCollection.FindByUsername(rootUser.Username) == null)
            {
                IdentityResult result = await userManager.CreateAsync(rootUser, "root");
                if (result.Succeeded)
                {
                    logger.LogInformation("Root user created!");
                }
                else
                {
                    logger.LogError("Failed to create root user!");
                }
            }
        }
    }
}
