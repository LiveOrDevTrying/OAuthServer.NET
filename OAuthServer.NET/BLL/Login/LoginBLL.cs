using JWTs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OAuthServer.NET.Core.Models;
using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.Core.Models.Exceptions;
using OAuthServer.NET.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OAuthServer.NET.BLL.Login
{
    public class LoginBLL : BaseResponseTypeBLL<BLLResponseTypeParameters>, ILoginBLL
    {
        protected readonly IConfiguration _configuration;
        protected readonly IJWTService _jwtService;

        public LoginBLL(OAuthServerDAL dal,
            IJWTService jwtService,
            IConfiguration configuration) : base(dal)
        {
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public virtual async Task<bool> LoadClientByClientIdAsync(string client_id)
        {
            _parameters = new BLLResponseTypeParameters
            {
                client_id = client_id,
            };
            _client = await _dal.GetClientAsync(_parameters.client_id);
            return _client != null;
        }
        public virtual async Task<bool> LoadClientByQueryStringAsync(string parameters)
        {
            var parsedQueryString = HttpUtility.ParseQueryString(parameters.Substring(parameters.IndexOf("?") + 1));

            _parameters = new BLLResponseTypeParameters
            {
                client_id = parsedQueryString.Get("client_id"),
                redirect_uri = parsedQueryString.Get("redirect_uri"),
                scope = parsedQueryString.Get("scope"),
                state = parsedQueryString.Get("state"),
                response_type = parsedQueryString.Get("response_type"),
            };
            _client = await _dal.GetClientAsync(_parameters.client_id);
            return _client != null;
        }

        public virtual (ClaimsPrincipal, AuthenticationProperties) GenerateCookie(ApplicationUser applicationUser, bool rememberLogin)
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in LoginBLL before validations");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
                new Claim(ClaimTypes.Name, applicationUser.UserName),
                new Claim("client_id", _client.ClientId)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = rememberLogin
            };
            return (principal, props);
        }
        public virtual async Task<string> GenerateTokenAndRedirectURIAsync(string applicationUserId, string IpAddressIssuingToken)
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in LoginBLL before validations");
            }

            if (_grant == null)
            {
                _grant = await _dal.GetGrantAsync(_client.GrantId);
            }

            var additionalClaims = new Dictionary<string, object>
            {
                {
                    "client_id", _client.ClientId
                },
            };

            if (_grant.AuthorizeResponseType == "token")
            {
                // For implicit clients
                if (!string.IsNullOrWhiteSpace(_parameters.scope))
                {
                    additionalClaims.Add("scope", _parameters.scope);
                }

                var token = await _jwtService.GenerateTokenAsync(applicationUserId, _client.Audience, _client.IssuerURI, _parameters.state, _client.TokenExpirationMin, _client.ClientSecretDecoded, additionalClaims);
                await _dal.SaveChangesAsync();
                return $"{_parameters.redirect_uri}?access_token={token.access_token}&expires_in={token.expires_in}&token_type={token.token_type}&scope={_parameters.scope}&state={_parameters.state}";
            }

            var code = await _jwtService.GenerateAuthorizationCodeAsync(applicationUserId, _parameters.redirect_uri, _parameters.scope, 15, IpAddressIssuingToken, _dal, additionalClaims);
            await _dal.SaveChangesAsync();
            return $"{_parameters.redirect_uri}?code={((AuthorizationCode)code).CodeDecoded}&state={_parameters.state}";
        }

        public virtual Task<ExternalProvider[]> GetExternalProvidersAsync()
        {
            var externalProviders = new List<ExternalProvider>();

            if (_client.EnableExternalLogin)
            {
                if (!string.IsNullOrWhiteSpace(_configuration["Google:ClientId"]) &&
                    _configuration["Google:ClientId"] != "GoogleClientId" &&
                    !string.IsNullOrWhiteSpace(_configuration["Google:ClientSecret"]) &&
                    _configuration["Google:ClientSecret"] != "GoogleClientSecret")
                {
                    externalProviders.Add(new ExternalProvider
                    {
                        AuthenticationScheme = "Google",
                        DisplayName = "Google"
                    });
                }

                if (!string.IsNullOrWhiteSpace(_configuration["Twitch:ClientId"]) &&
                   _configuration["Twitch:ClientId"] != "TwitchClientId" &&
                   !string.IsNullOrWhiteSpace(_configuration["Twitch:ClientSecret"]) &&
                   _configuration["Twitch:ClientSecret"] != "TwitchClientSecret")
                {
                    externalProviders.Add(new ExternalProvider
                    {
                        AuthenticationScheme = "Twitch",
                        DisplayName = "Twitch"
                    });
                }

                if (!string.IsNullOrWhiteSpace(_configuration["Facebook:ClientId"]) &&
                  _configuration["Facebook:ClientId"] != "FacebookClientId" &&
                  !string.IsNullOrWhiteSpace(_configuration["Facebook:ClientSecret"]) &&
                  _configuration["Facebook:ClientSecret"] != "FacebookClientSecret")
                {
                    externalProviders.Add(new ExternalProvider
                    {
                        AuthenticationScheme = "Facebook",
                        DisplayName = "Facebook"
                    });
                }

                if (!string.IsNullOrWhiteSpace(_configuration["Twitter:ClientId"]) &&
                _configuration["Twitter:ClientId"] != "TwitterClientId" &&
                !string.IsNullOrWhiteSpace(_configuration["Twitter:ClientSecret"]) &&
                _configuration["Twitter:ClientSecret"] != "TwitterClientSecret")
                {
                    externalProviders.Add(new ExternalProvider
                    {
                        AuthenticationScheme = "Twitter",
                        DisplayName = "Twitter"
                    });
                }

                if (!string.IsNullOrWhiteSpace(_configuration["Microsoft:ClientId"]) &&
                _configuration["Microsoft:ClientId"] != "MicrosoftClientId" &&
                !string.IsNullOrWhiteSpace(_configuration["Microsoft:ClientSecret"]) &&
                _configuration["Microsoft:ClientSecret"] != "MicrosoftClientSecret")
                {
                    externalProviders.Add(new ExternalProvider
                    {
                        AuthenticationScheme = "Microsoft",
                        DisplayName = "Microsoft"
                    });
                }
            }

            return Task.FromResult(externalProviders.ToArray());
        }
        public virtual async Task<AuthenticationProperties> SignInExternalChallengeAsync(string provider, string redirectUri)
        {
            return await _dal.ExternalLoginChallengeAsync(provider, redirectUri);
        }
        public virtual async Task<ApplicationUser> SignInExternalLoginAsync()
        {
            var info = await _dal.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return null;
            }

            var loginExists = await _dal.ExternalLoginSignInAsync(info);

            var applicationUser = await _dal.GetApplicationUserByNameAsync(info.Principal.FindFirst(ClaimTypes.Email).Value);

            if (applicationUser == null)
            {
                applicationUser = await _dal.CreateApplicationUserAsync(new ApplicationUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                });

                if (applicationUser == null)
                {
                    return null;
                }
            }

            if (!loginExists.Succeeded)
            {
                var result = await _dal.CreateLoginAsync(applicationUser, info);
                if (!result.Succeeded)
                {
                    return null;
                }
            }

            return applicationUser;
        }
    }
}
