using JWTs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuthServer.NET.BLL.Authorize;
using OAuthServer.NET.BLL.Login;
using OAuthServer.NET.BLL.Token;
using OAuthServer.NET.Core.Data;
using OAuthServer.NET.Core.Middleware;
using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.CORS;
using OAuthServer.NET.Middleware;
using OAuthServer.NET.Services;
using System;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using OAuthServer.NET.Core.Models.Exceptions;
using System.ComponentModel.DataAnnotations;
using OAuthServer.NET.Providers;

namespace OAuthServer.NET
{
    public static class Extensions
    {
        public static void StartOAuthServer(this IServiceCollection services, OAuthServerParams oauthServerParams)
        {
            if (string.IsNullOrWhiteSpace(oauthServerParams.ConnectionString))
            {
                throw new AppException("Invalid connection string");
            }

            if (string.IsNullOrWhiteSpace(oauthServerParams.AdminPassword))
            {
                throw new AppException("Invalid admin password");
            }

            services.AddSingleton(oauthServerParams);

            services.AddTransient<OAuthServerDAL>();
            services.AddTransient<IJWTService, JWTService>();

            services.AddTransient<IAuthorizeBLL, AuthorizeBLL>();
            services.AddTransient<ILoginBLL, LoginBLL>();
            services.AddTransient<ITokenBLL, TokenBLL>();

            services.AddTransient<ICorsPolicyProvider, OAuthCorsPolicyProvider>();

            services.AddTransient<IAuthorizeTypeProvider, AuthorizeTypeProviderAuthorizationCode>();
            services.AddTransient<IAuthorizeTypeProvider, AuthorizeTypeProviderImplicit>();

            services.AddTransient<ITokenGrantProvider, TokenGrantProviderAuthorizationCode>();
            services.AddTransient<ITokenGrantProvider, TokenGrantProviderClientCredentials>();
            services.AddTransient<ITokenGrantProvider, TokenGrantProviderRefreshToken>();
            services.AddTransient<ITokenGrantProvider, TokenGrantProviderROPassword>();

            services.AddTransient<OAuthServerServices>();

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(oauthServerParams.ConnectionString);
            });
            

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = oauthServerParams.RequireConfirmedEmail;
            })
                .AddSignInManager<OAuthServerSignInManager>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(oauthServerParams.OAuthCookieExpirationDays);
                    options.Cookie.Name = "OAuthServer.NET";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = oauthServerParams.ExternalProviders.GoogleClientId;
                    options.ClientSecret = oauthServerParams.ExternalProviders.GoogleClientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddTwitch(options =>
                {
                    options.ClientId = oauthServerParams.ExternalProviders.TwitchClientId;
                    options.ClientSecret = oauthServerParams.ExternalProviders.TwitchClientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddFacebook(options =>
                {
                    options.ClientId = oauthServerParams.ExternalProviders.FacebookClientId;
                    options.ClientSecret = oauthServerParams.ExternalProviders.FacebookClientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = oauthServerParams.ExternalProviders.TwitchClientId;
                    options.ConsumerSecret = oauthServerParams.ExternalProviders.TwitchClientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = oauthServerParams.ExternalProviders.MicrosoftClientId;
                    options.ClientSecret = oauthServerParams.ExternalProviders.MicrosoftClientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public static void StartOAuthServer(this IApplicationBuilder app, OAuthServerServices services)
        {
            // Will create database if not created
            services.DAL.MigrateDatabaseAsync().Wait();
            SeedDatabaseAsync(services).Wait();

            if (services.IsDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMiddleware<OAuthServerCORSMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        private static async Task SeedDatabaseAsync(OAuthServerServices oauthServerServices)
        {
            // Create admin user
            var adminUser = await oauthServerServices.DAL.GetApplicationUserByNameAsync(oauthServerServices.Parameters.AdminUsername);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = oauthServerServices.Parameters.AdminUsername
                };

                adminUser = await oauthServerServices.DAL.CreateApplicationUserAsync(adminUser, oauthServerServices.Parameters.AdminPassword);
                if (adminUser == null)
                {
                    throw new AppException($"Could not seed {oauthServerServices.Parameters.AdminUsername} user");
                }

                // Role added only for UI application
                var role = await oauthServerServices.RoleManager.FindByNameAsync("admin");

                if (role == null)
                {
                    var result = await oauthServerServices.RoleManager.CreateAsync(new IdentityRole
                    {
                        Name = "admin"
                    });

                    if (result.Succeeded)
                    {
                        role = await oauthServerServices.RoleManager.FindByNameAsync("admin");
                    }
                }

                await oauthServerServices.DAL.AddToRoleAsync(adminUser, "admin");
            }

            oauthServerServices.DAL.SaveChangesAsync().Wait();
        }

        public static Tuple<string, string> DecodeCredentials(this StringValues bearerToken)
        {
            var authorizationSplit = bearerToken.ToString().Split(" ");

            if (authorizationSplit.Length != 2)
            {
                return null;
            }

            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationSplit.Last()));
            var decodedCredentialsSplit = decodedCredentials.Split(":");

            return new Tuple<string, string>(decodedCredentialsSplit.First(), decodedCredentialsSplit.Last());
        }
    }

    public class OAuthServerParams
    {
        public string AdminUsername { get; set; } = "admin";
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
        public string ConnectionString { get; set; }
        public bool RequireConfirmedEmail { get; set; } = false;
        public bool Show3rdPartyLoginGraphics { get; set; } = true;
        public int OAuthCookieExpirationDays { get; set; } = 7;
        public OAuthServerExternalProviders ExternalProviders { get; set; } = new OAuthServerExternalProviders();
    }

    public class OAuthServerExternalProviders
    {
        public string GoogleClientId { get; set; } = "GoogleClientId";
        public string GoogleClientSecret { get; set; } = "GoogleClientSecret";
        public string TwitchClientId { get; set; } = "TwitchClientId";
        public string TwitchClientSecret { get; set; } = "TwitchClientSecret";
        public string FacebookClientId { get; set; } = "FacebookClientId";
        public string FacebookClientSecret { get; set; } = "FacebookClientSecret";
        public string TwitterClientId { get; set; } = "TwitterClientId";
        public string TwitterClientSecret { get; set; } = "TwitterClientSecret";
        public string MicrosoftClientId { get; set; } = "MicrosoftClientId";
        public string MicrosoftClientSecret { get; set; } = "MicrosoftClientSecret";
    }

}
