using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.Core.Models.Exceptions;
using OAuthServer.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTs;
using JWTs.Models;
using Microsoft.Extensions.Primitives;
using OAuthServer.NET.Models;

namespace OAuthServer.NET.BLL.Token
{
    public class TokenBLL : BaseBLL<TokenParameters>, ITokenBLL
    {
        protected readonly IJWTService _jwtService;

        public TokenBLL(OAuthServerDAL dal,
            IJWTService jwtService) : base(dal)
        {
            _jwtService = jwtService;
        }

        public virtual async Task<bool> LoadClientAndTokenRequestAsync(StringValues bearerToken, TokenRequest request)
        {
            _parameters = new TokenParameters
            {
                redirect_uri = request.redirect_uri,
                client_id = request.client_id,
                client_secret = request.client_secret,
                grant_type = request.grant_type,
            };

            string clientId, clientSecret;
            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                var credentials = bearerToken.DecodeCredentials();
                clientId = credentials.Item1;
                clientSecret = credentials.Item2;
            }
            else
            {
                clientId = request.client_id;
                clientSecret = request.client_secret;
            }

            _client = await _dal.GetClientAsync(clientId, clientSecret);
            return _client != null;
        }

        public virtual async Task<ApplicationUser> ExchangeAuthorizationCodeAsync(string code, string redirect_uri)
        {
            if (_client == null)
            {
                throw new AppException("Call LoadClientAsync() in TokenBLL before validations");
            }

            if (await _jwtService.GetAuthorizationCodeAsync(code, redirect_uri, true, _dal) is AuthorizationCode authCode)
            {
                return await _dal.GetApplicationUserAsync(authCode.ApplicationUserId);
            }

            return null;
        }
        public virtual async Task<(IToken, IRefreshToken)> GenerateTokenAsync(string applicationUserId, string scope, string state, string createdByIp)
        {
            if (_client == null)
            {
                throw new AppException("Call LoadClientAsync() in TokenBLL before validations");
            }

            var additionalClaims = new Dictionary<string, object>
            {
                { "client_id", _client.ClientId }
            };

            if (!string.IsNullOrWhiteSpace(scope))
            {
                additionalClaims.Add("scope", scope);
            }

            return _client.IssueRefreshTokens
                ? await _jwtService.GenerateTokenWithRefreshTokenAsync(applicationUserId, _client.Audience, _client.IssuerURI, state, _client.TokenExpirationMin, _client.ClientSecretDecoded, createdByIp, _client.RefreshTokenExpirationDays, _dal, additionalClaims)
                : (await _jwtService.GenerateTokenAsync(applicationUserId, _client.Audience, _client.IssuerURI, state, _client.TokenExpirationMin, _client.ClientSecretDecoded, additionalClaims), null);
        }
        public virtual async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            return await _dal.GetRefreshTokenAsync(refreshToken) as RefreshToken;
        }
        public virtual async Task<bool> RevokeRefreshTokenAsync(RefreshToken refreshToken, DateTime timeRevoked, string ipAddressIssuingTokens, RefreshToken replacementToken)
        {
            return await _dal.RevokeRefreshTokenAsync(refreshToken, timeRevoked, ipAddressIssuingTokens, replacementToken);
        }
        public virtual Task<bool> IsScopeMatchingOriginalScopeAsync(RefreshToken refreshToken, string scope)
        {
            var scopesSplit = scope.ToString().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var entityScopesSplit = refreshToken.Scopes.ToString().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            foreach (var item in scopesSplit)
            {
                if (!entityScopesSplit.Contains(item))
                {
                    return Task.FromResult(false);
                }
            }

            foreach (var item in entityScopesSplit)
            {
                if (!scopesSplit.Contains(item))
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }
        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _dal.SaveChangesAsync();
        }
    }

    public class TokenParameters : BLLParameters
    {
        public string grant_type { get; set; }
        public string client_secret { get; set; }
    }
}
