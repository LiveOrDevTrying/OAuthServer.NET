using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.Core.Models.Exceptions;
using OAuthServer.NET.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthServer.NET.BLL
{
    public abstract class BaseBLL<T> : IBaseBLL where T : BLLParameters
    {
        protected readonly OAuthServerDAL _dal;
        protected T _parameters;
        protected Client _client;
        protected Grant _grant;

        public BaseBLL(OAuthServerDAL dal)
        {
            _dal = dal;
        }

        public virtual async Task<bool> IsScopeValidAsync(string scope)
        {
            var scopesSplit = scope.ToString().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var scopes = await _dal.GetClientScopesAsync(_client.Id);

            foreach (var scopeSplit in scopesSplit)
            {
                if (!scopes.Any(x => x.ScopeName.Trim().ToLower() == scopeSplit.Trim().ToLower()))
                {
                    return false;
                }
            }

            return true;
        }
        public virtual async Task<bool> IsGrantValidAsync(string grant_type)
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in TokenBLL before validations");
            }

            _grant = await _dal.GetGrantAsync(_client.GrantId);

            return _grant != null && grant_type == "refresh_token" || _grant.TokenGrantType == grant_type;
        }
        public virtual async Task<bool> IsRedirectURIValidAsync()
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in AuthorizeBLL before validations");
            }

            var clientRedirectURIs = await _dal.GetClientRedirectURIsAsync(_client.Id);

            if ((!string.IsNullOrWhiteSpace(_parameters.redirect_uri) && clientRedirectURIs.Length == 0) ||
                (string.IsNullOrWhiteSpace(_parameters.redirect_uri) && clientRedirectURIs.Length > 0) ||
                (!string.IsNullOrWhiteSpace(_parameters.redirect_uri) && !clientRedirectURIs.Any(x => x.RedirectURI == _parameters.redirect_uri)))
            {
                return false;
            }

            return true;
        }
        public virtual async Task<bool> IsPostLogoutURIValidAsync(string postLogoutRedirectUri)
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in LoginBLL before validations");
            }

            var postLogoutRedirectURIs = await _dal.GetClientPostLogoutRedirectURIs(_client.Id);
            return postLogoutRedirectURIs.Any(x => x.PostLogoutRedirectURI == postLogoutRedirectUri);
        }
        public virtual async Task<ApplicationUser> GetApplicationUserAsync(string username, string password)
        {
            return await _dal.GetApplicationUserAsync(username, password);
        }

        public Client Client
        {
            get
            {
                return _client;
            }
        }
    }

    public class BLLParameters
    {
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
    }
}
