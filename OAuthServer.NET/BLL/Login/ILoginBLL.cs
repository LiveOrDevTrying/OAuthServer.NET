using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OAuthServer.NET.Core.Models;
using OAuthServer.NET.Core.Models.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuthServer.NET.BLL.Login
{
    public interface ILoginBLL : IBaseResponseTypeBLL
    {
        Task<bool> LoadClientByQueryStringAsync(string parameters);
        Task<bool> LoadClientByClientIdAsync(string client_id);
        
        Task<string> GenerateTokenAndRedirectURIAsync(string applicationUserId, string IpAddressIssuingToken);
        (ClaimsPrincipal, AuthenticationProperties) GenerateCookie(ApplicationUser applicationUser, bool rememberLogin);

        Task<ExternalProvider[]> GetExternalProvidersAsync();
        Task<AuthenticationProperties> SignInExternalChallengeAsync(string provider, string redirectUri);
        Task<ApplicationUser> SignInExternalLoginAsync();
    }
}