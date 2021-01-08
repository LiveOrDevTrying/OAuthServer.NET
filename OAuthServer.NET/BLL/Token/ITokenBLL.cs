using JWTs.Models;
using Microsoft.Extensions.Primitives;
using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.Models;
using System;
using System.Threading.Tasks;

namespace OAuthServer.NET.BLL.Token
{
    public interface ITokenBLL : IBaseBLL
    {
        Task<ApplicationUser> ExchangeAuthorizationCodeAsync(string code, string redirect_uri);
        Task<(IToken, IRefreshToken)> GenerateTokenAsync(string applicationUserId, string scope, string state, string ipAddressIssuingTokens);
        Task<bool> LoadClientAndTokenRequestAsync(StringValues bearerToken, TokenRequest request);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(RefreshToken token, DateTime timeRevoked, string ipAddressIssuingTokens, RefreshToken replacementToken);
        Task<bool> IsScopeMatchingOriginalScopeAsync(RefreshToken refreshToken, string scope);

        Task<bool> SaveChangesAsync();
    }
}