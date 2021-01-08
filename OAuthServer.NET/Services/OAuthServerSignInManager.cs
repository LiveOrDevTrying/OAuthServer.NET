using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OAuthServer.NET.Core.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OAuthServer.NET.Services
{
    public class OAuthServerSignInManager : SignInManager<ApplicationUser>
    {
        protected readonly IConfiguration _configuration;

        public OAuthServerSignInManager(UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<ApplicationUser>> logger, 
            IAuthenticationSchemeProvider schemes, 
            IConfiguration configuration,
            IUserConfirmation<ApplicationUser> confirmation) 
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _configuration = configuration;
        }

        public override Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            var schemes =  new List<AuthenticationScheme>();

            if (!string.IsNullOrWhiteSpace(_configuration["Google:ClientId"]) &&
                _configuration["Google:ClientId"] != "GoogleClientId" &&
                !string.IsNullOrWhiteSpace(_configuration["Google:ClientSecret"]) &&
                _configuration["Google:ClientSecret"] != "GoogleClientSecret")
            {
                schemes.Add(new AuthenticationScheme("Google", "Google", typeof(GoogleHandler)));
            }

            if (!string.IsNullOrWhiteSpace(_configuration["Twitch:ClientId"]) &&
               _configuration["Twitch:ClientId"] != "TwitchClientId" &&
               !string.IsNullOrWhiteSpace(_configuration["Twitch:ClientSecret"]) &&
               _configuration["Twitch:ClientSecret"] != "TwitchClientSecret")
            {
                schemes.Add(new AuthenticationScheme("Twitch", "Twitch", typeof(TwitchAuthenticationHandler)));
            }

            if (!string.IsNullOrWhiteSpace(_configuration["Facebook:ClientId"]) &&
              _configuration["Facebook:ClientId"] != "FacebookClientId" &&
              !string.IsNullOrWhiteSpace(_configuration["Facebook:ClientSecret"]) &&
              _configuration["Facebook:ClientSecret"] != "FacebookClientSecret")
            {
                schemes.Add(new AuthenticationScheme("Facebook", "Facebook", typeof(FacebookHandler)));
            }

            if (!string.IsNullOrWhiteSpace(_configuration["Twitter:ClientId"]) &&
            _configuration["Twitter:ClientId"] != "TwitterClientId" &&
            !string.IsNullOrWhiteSpace(_configuration["Twitter:ClientSecret"]) &&
            _configuration["Twitter:ClientSecret"] != "TwitterClientSecret")
            {
                schemes.Add(new AuthenticationScheme("Twitter", "Twitter", typeof(TwitterHandler)));
            }

            if (!string.IsNullOrWhiteSpace(_configuration["Microsoft:ClientId"]) &&
            _configuration["Microsoft:ClientId"] != "MicrosoftClientId" &&
            !string.IsNullOrWhiteSpace(_configuration["Microsoft:ClientSecret"]) &&
            _configuration["Microsoft:ClientSecret"] != "MicrosoftClientSecret")
            {
                schemes.Add(new AuthenticationScheme("Microsoft", "Microsoft", typeof(MicrosoftAccountHandler)));
            }

            return Task.FromResult(schemes as IEnumerable<AuthenticationScheme>);
        }
    }
}
