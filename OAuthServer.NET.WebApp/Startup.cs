using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuthServer.NET.Services;

namespace OAuthServer.NET.WebApp
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
            services.StartOAuthServer(new OAuthServerParams
            {
                AdminPassword = Configuration["Admin:Password"],
                AdminUsername = Configuration["Admin:Username"],
                OAuthCookieExpirationDays = int.Parse(Configuration["OAuthServerCookieExpirationDays"]),
                ConnectionString = Configuration["ConnectionStrings:Database"],
                RequireConfirmedEmail = bool.Parse(Configuration["RequireConfirmedEmail"]),
                Show3rdPartyLoginGraphics = bool.Parse(Configuration["Show3rdPartyLoginGraphics"]),
                ExternalProviders = new OAuthServerExternalProviders
                {
                    GoogleClientId = Configuration["Google:ClientId"],
                    GoogleClientSecret = Configuration["Google:ClientSecret"],

                    TwitchClientId = Configuration["Twitch:ClientId"],
                    TwitchClientSecret = Configuration["Twitch:ClientSecret"],

                    FacebookClientId = Configuration["Facebook:ClientId"],
                    FacebookClientSecret = Configuration["Facebook:ClientSecret"],

                    TwitterClientId = Configuration["Twitter:ClientId"],
                    TwitterClientSecret = Configuration["Twitter:ClientSecret"],

                    MicrosoftClientId = Configuration["Microsoft:ClientId"],
                    MicrosoftClientSecret = Configuration["Microsoft:ClientSecret"],
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, OAuthServerServices services)
        {
            app.StartOAuthServer(services);
        }
    }
}
