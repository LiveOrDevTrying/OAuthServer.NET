using JWTs.Models;
using Microsoft.AspNetCore.Http;
using OAuthServer.NET.BLL.Token;
using OAuthServer.NET.Models;
using System.Threading.Tasks;

namespace OAuthServer.NET.Providers
{
    public class TokenGrantProviderROPassword : BaseTokenGrantProvider, ITokenGrantProvider
    {
        public string grant_type => "password";

        public TokenGrantProviderROPassword(ITokenBLL tokenBLL) : base(tokenBLL)
        {
        }

        public virtual async Task<(IToken, string)> GenerateTokenAsync(HttpRequest request, TokenRequest tokenRequest, string ipAddressRequestingToken)
        {
            var (success, errorString) = await ValidateAsync(request, tokenRequest);

            if (!success)
            {
                return (null, errorString);
            }

            var hasScope = request.Form.TryGetValue("scope", out var scope);
            var hasState = request.Form.TryGetValue("state", out var state);

            if (hasScope && !await _tokenBLL.IsScopeValidAsync(scope))
            {
                return (null, "invalid_scope");
            }

            if (!request.Form.TryGetValue("username", out var username) ||
                !request.Form.TryGetValue("password", out var password))
            {
                return (null, "access_denied");
            }

            var applicationUser = await _tokenBLL.GetApplicationUserAsync(username, password);

            if (applicationUser == null)
            {
                return (null, "access_denied");
            }

            var (token, refreshToken) = await _tokenBLL.GenerateTokenAsync(applicationUser.Id, hasScope && !string.IsNullOrWhiteSpace(scope) ? scope.ToString() : null, hasState && !string.IsNullOrWhiteSpace(state) ? state.ToString() : null, ipAddressRequestingToken);
            await _tokenBLL.SaveChangesAsync();
            return (token, null);
        }
    }
}
