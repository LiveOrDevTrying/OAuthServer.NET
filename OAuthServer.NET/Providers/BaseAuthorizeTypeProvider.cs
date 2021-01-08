using Microsoft.AspNetCore.Http;
using OAuthServer.NET.BLL.Authorize;
using System.Threading.Tasks;

namespace OAuthServer.NET.Providers
{
    public abstract class BaseAuthorizeTypeProvider
    {
        protected readonly IAuthorizeBLL _authorizeBLL;

        public BaseAuthorizeTypeProvider(IAuthorizeBLL authorizeBLL)
        {
            _authorizeBLL = authorizeBLL;
        }

        public virtual async Task<(bool, string)> ValidateAsync(HttpRequest request)
        {
            var response_type = request.Query["response_type"].ToString();
            var client_id = request.Query["client_id"].ToString();
            var redirect_uri = request.Query["redirect_uri"].ToString();
            var scope = request.Query["scope"].ToString();
            var state = request.Query["state"].ToString();

            if (string.IsNullOrWhiteSpace(client_id) ||
                !await _authorizeBLL.LoadClientAsync(response_type, client_id, redirect_uri, scope, state))
            {
                return (false, "unauthorized_client");
            }

            if (!await _authorizeBLL.IsAuthorizeResponseTypeValidAsync())
            {
                return (false, "invalid_request");
            }

            if (!await _authorizeBLL.IsScopeValidAsync())
            {
                return (false, "invalid_scope");
            }

            if (!await _authorizeBLL.IsRedirectURIValidAsync())
            {
                return (false, "invalid_request");
            }

            if (!await _authorizeBLL.IsGrantValidAsync())
            {
                return (false, "unauthorized_client");
            }

            return (true, null);
        }
    }
}
