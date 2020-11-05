using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
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
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            database = client.GetDatabase("TodoApp");
            TodoAppData.Init(database);

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
               {
                   opts.RequireHttpsMetadata = false;
                   opts.SaveToken = true;
                   opts.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = "MEIRL",
                       ValidateAudience = true,
                       ValidAudience = "LOL",
                       ValidateIssuerSigningKey = false
                   };
                   opts.Audience = "LOL";
               });
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            services.AddSpaStaticFiles(configuration => // Static web files
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
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseStatusCodePages();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
