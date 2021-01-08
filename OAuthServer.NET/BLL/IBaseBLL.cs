using OAuthServer.NET.Core.Models.Entities;
using System.Threading.Tasks;

namespace OAuthServer.NET.BLL
{
    public interface IBaseBLL
    {
        Client Client { get; }

        Task<bool> IsGrantValidAsync(string grant_type);
        Task<bool> IsRedirectURIValidAsync();
        Task<bool> IsScopeValidAsync(string scope);
        Task<bool> IsPostLogoutURIValidAsync(string post_logout_redirect_uri);

        Task<ApplicationUser> GetApplicationUserAsync(string username, string password);
    }
}