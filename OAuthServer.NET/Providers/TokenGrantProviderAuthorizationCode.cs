using JWTs.Models;
using Microsoft.AspNetCore.Http;
using OAuthServer.NET.BLL.Token;
using OAuthServer.NET.Models;
using System.Threading.Tasks;

namespace OAuthServer.NET.Providers
{
    public class TokenGrantProviderAuthorizationCode : BaseTokenGrantProvider, ITokenGrantProvider
    {
        public string grant_type => "authorization_code";

        public TokenGrantProviderAuthorizationCode(ITokenBLL tokenBLL) : base(tokenBLL)
        {
        }

        public virtual async Task<(IToken, string)> GenerateTokenAsync(HttpRequest request, TokenRequest tokenRequest, string ipAddressRequestingToken)
        {
            var (success, errorString) = await ValidateAsync(request, tokenRequest);

            if (!success)
            {
                return (null, errorString);
            }

            if (!request.Form.TryGetValue("code", out var code) ||
                string.IsNullOrWhiteSpace(code))
            {
                return (null, "invalid_request");
            }

            // Verify same URI in redirect and client configuration (saved with auth token) here
            var applicationUser = await _tokenBLL.ExchangeAuthorizationCodeAsync(code, tokenRequest.redirect_uri);

            if (applicationUser == null)
            {
                return (null, "Invalid authorization_code");
            }

            var hasScope = request.Form.TryGetValue("scope", out var scope);
            var hasState = request.Form.TryGetValue("state", out var state);

            var (token, refreshToken) = await _tokenBLL.GenerateTokenAsync(applicationUser.Id, hasScope && !string.IsNullOrWhiteSpace(scope) ? scope.ToString() : null, hasState && !string.IsNullOrWhiteSpace(state) ? state.ToString() : null, ipAddressRequestingToken);
            await _tokenBLL.SaveChangesAsync();
            return (token, null);
        }
    }
}
