using JWTs.Models;
using Microsoft.AspNetCore.Http;
using OAuthServer.NET.BLL.Token;
using OAuthServer.NET.Core.Models.Entities;
using System;
using System.Threading.Tasks;
using OAuthServer.NET.Models;

namespace OAuthServer.NET.Providers
{
    public class TokenGrantProviderRefreshToken : BaseTokenGrantProvider, ITokenGrantProvider
    {
        public string grant_type => "refresh_token";

        public TokenGrantProviderRefreshToken(ITokenBLL tokenBLL) : base(tokenBLL)
        {
        }

        public virtual async Task<(IToken, string)> GenerateTokenAsync(HttpRequest request, TokenRequest tokenRequest, string ipAddressRequestingToken)
        {
            var (success, errorString) = await ValidateAsync(request, tokenRequest);

            if (!success)
            {
                return (null, errorString);
            }

            if (!request.Form.TryGetValue("refresh_token", out var refreshToken))
            {
                return (null, "invalid_request");
            }

            var refreshTokenEntity = await _tokenBLL.GetRefreshTokenAsync(refreshToken);

            if (refreshTokenEntity == null)
            {
                return (null, "invalid_request");
            }

            var hasScope = request.Form.TryGetValue("scope", out var scope);
            var hasState = request.Form.TryGetValue("state", out var state);

            // Verify requested scopes match original scopes
            if (hasScope && !await _tokenBLL.IsScopeMatchingOriginalScopeAsync(refreshTokenEntity, scope.ToString()))
            {
                return (null, "invalid_scope");
            }

            var (newToken, newRefreshToken) = await _tokenBLL.GenerateTokenAsync(refreshTokenEntity.ApplicationUserId, hasScope && !string.IsNullOrWhiteSpace(scope) ? scope.ToString() : null, hasState && !string.IsNullOrWhiteSpace(state) ? state.ToString() : null, ipAddressRequestingToken);

            await _tokenBLL.RevokeRefreshTokenAsync(refreshTokenEntity, DateTime.UtcNow, ipAddressRequestingToken, newRefreshToken as RefreshToken);

            await _tokenBLL.SaveChangesAsync();

            return (newToken, null);
        }
    }
}
