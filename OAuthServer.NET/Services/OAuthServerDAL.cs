using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using OAuthServer.NET.Core.Data;
using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.Core.Models.Exceptions;
using JWTs.Models;

namespace OAuthServer.NET.Services
{
    public class OAuthServerDAL : JWTs.Services.IDAL
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;

        public OAuthServerDAL(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public virtual async Task MigrateDatabaseAsync()
        {
            await _context.Database.MigrateAsync();
        }

        // Identity
        public virtual async Task<ApplicationUser> GetApplicationUserAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public virtual async Task<ApplicationUser> GetApplicationUserAsync(string username, string password)
        {
            var applicationUser = await _userManager.FindByNameAsync(username);

            if (await _userManager.CheckPasswordAsync(applicationUser, password))
            {
                return applicationUser;
            }

            return null;
        }
        public virtual async Task<ApplicationUser> GetApplicationUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public virtual async Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser)
        {
            var result = await _userManager.CreateAsync(applicationUser);

            if (result.Succeeded)
            {
                return await GetApplicationUserAsync(applicationUser.Id);
            }

            return null;
        }
        public virtual async Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser applicationUser, string password)
        {
            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                return await GetApplicationUserAsync(applicationUser.Id);
            }

            return null;
        }
        public virtual async Task<bool> UpdateApplicationUserAsync(ApplicationUser applicationUser)
        {
            var result = await _userManager.UpdateAsync(applicationUser);
            return result.Succeeded;
        }
        public virtual async Task<bool> DeleteApplicationUserAsync(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser != null)
            {
                var result = await _userManager.DeleteAsync(applicationUser);
                return result.Succeeded;
            }

            return false;
        }
        public virtual async Task<bool> AddToRoleAsync(ApplicationUser applicationUser, string role)
        {
            var result = await _userManager.AddToRoleAsync(applicationUser, role);
            return result.Succeeded;
        }
        public virtual async Task<IdentityResult> CreateLoginAsync(ApplicationUser applicationUser, ExternalLoginInfo info)
        {
            return await _userManager.AddLoginAsync(applicationUser, info);
        }
        public virtual Task<AuthenticationProperties> ExternalLoginChallengeAsync(string providerName, string redirect_uri)
        {
            return Task.FromResult(_signInManager.ConfigureExternalAuthenticationProperties(providerName, redirect_uri));
        }
        public virtual async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }
        public virtual async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info)
        {
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            return result;
        }

        // OAuthServer
        public virtual async Task<Grant[]> GetGrantsAsync()
        {
            return await _context.Grants.ToArrayAsync();
        }
        public virtual async Task<Grant> GetGrantAsync(Guid id)
        {
            return await _context.Grants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<Client> GetClientAsync(string client_id)
        {
            return await _context.Clients.FirstOrDefaultAsync(x => x.ClientId.Trim().ToLower() == client_id.Trim().ToLower());
        }
        public virtual async Task<Client> GetClientAsync(string client_id, string client_secret)
        {
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(client_secret));
            return await _context.Clients.FirstOrDefaultAsync(x => x.ClientId.Trim().ToLower() == client_id.Trim().ToLower() &&
                x.ClientSecret == base64EncodedSecret);
        }

        public virtual async Task<ClientCORSOrigin[]> GetClientCORSOriginsAsync(Guid clientId)
        {
            return await _context.ClientsCORSOrigins
                .Where(x => x.ClientId == clientId)
                .ToArrayAsync();
        }
        public virtual async Task<ClientScope[]> GetClientScopesAsync(Guid clientId)
        {
            return await _context.ClientsScopes
                .Where(x => x.ClientId == clientId)
                .ToArrayAsync();
        }
        public virtual async Task<ClientRedirectURI[]> GetClientRedirectURIsAsync(Guid clientId)
        {
            return await _context.ClientsRedirectURIs
                .Where(x => x.ClientId == clientId)
                .ToArrayAsync();
        }
        public virtual async Task<ClientRedirectURI> GetClientRedirectURIAsync(Guid id)
        {
            return await _context.ClientsRedirectURIs
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<ClientPostLogoutRedirectURI[]> GetClientPostLogoutRedirectURIs(Guid clientId)
        {
            return await _context.ClientsPostLogoutRedirectURIs
                .Where(x => x.ClientId == clientId)
                .ToArrayAsync();
        }
        public virtual async Task<ClientPostLogoutRedirectURI> GetClientPostLogoutRedirectURI(Guid id)
        {
            return await _context.ClientsPostLogoutRedirectURIs
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<AuthorizationCode> GetAuthorizationCodeAsync(Client client, string code, string redirectUri, bool markInactive)
        {
            var entity = await GetAuthorizationCodeAsync(code, redirectUri, markInactive) as AuthorizationCode;

            if (entity != null &&
                entity.ClientId == client.Id)
            {
                return entity;
            }

            return null;
        }
        public virtual async Task<IAuthorizationCode> GetAuthorizationCodeAsync(string code, string redirectUri, bool markInactive)
        {
            var encodedAuthorizationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(code));
            var encodedRedirectUri = WebUtility.HtmlEncode(redirectUri);

            var entity = await _context.AuthorizationCodes.FirstOrDefaultAsync(x => x.Code == encodedAuthorizationCode &&
                x.RedirectURI == encodedRedirectUri);

            if (entity == null || entity.IsExpired)
            {
                return null;
            }

            if (markInactive)
            {
                _context.AuthorizationCodes.Remove(entity);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    return null;
                }
            }

            return entity;
        }
        public virtual async Task<IAuthorizationCode> CreateAuthorizationCodeAsync(string subject, string code, string redirect_uri, string scope, int lifespanMin, string createdByIP, Dictionary<string, object> additionalClaims = null)
        {
            var existingAuthorizationCodes = await _context.AuthorizationCodes
                .Where(x => x.ApplicationUserId == subject)
                .ToListAsync();

            for (int i = 0; i < existingAuthorizationCodes.Count; i++)
            {
                _context.AuthorizationCodes.Remove(existingAuthorizationCodes[i]);
            }

            var clientId = additionalClaims["client_id"].ToString();
            var client = await GetClientAsync(clientId);

            var authorizationCode = new AuthorizationCode
            {
                Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(code)),
                RedirectURI = WebUtility.HtmlEncode(redirect_uri),
                ApplicationUserId = subject,
                ClientId = client.Id,
                Created = DateTime.UtcNow,
                CreatedByIp = createdByIP,
                Scope = scope,
                Expires = DateTime.UtcNow.AddMinutes(lifespanMin),
            };
            _context.AuthorizationCodes.Add(authorizationCode);
            return authorizationCode;
        }

        public virtual async Task<IRefreshToken> GetRefreshTokenAsync(Guid id)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (refreshTokenEntity == null ||
                !refreshTokenEntity.IsActive)
            {
                throw new AppException("Invalid token");
            }

            return refreshTokenEntity;
        }
        public virtual async Task<IRefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (refreshTokenEntity == null ||
                !refreshTokenEntity.IsActive)
            {
                throw new AppException("Invalid token");
            }

            return refreshTokenEntity;
        }
        public virtual async Task<IRefreshToken> AddRefreshTokenAsync(string subject, string token, int expirationTimeDays, string createdByIp, Dictionary<string, object> additionalClaims = null)
        {
            if (!additionalClaims.TryGetValue("client_id", out var clientId))
            {
                return null;
            }

            var client = await GetClientAsync(clientId.ToString());

            additionalClaims.TryGetValue("scope", out var scope);

            var refreshToken = new RefreshToken
            {
                ApplicationUserId = subject,
                ClientId = client.Id,
                Token = token,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(expirationTimeDays),
                CreatedByIp = createdByIp,
                Scopes = scope?.ToString(),
            };
            _context.RefreshTokens.Add(refreshToken);
            return refreshToken;
        }
        public virtual async Task<bool> RevokeRefreshTokenAsync(IRefreshToken refreshToken, DateTime revokedTime, string revokedByIp, IRefreshToken replacedByRefreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Id == ((RefreshToken)refreshToken).Id);

            if (refreshTokenEntity == null ||
                !refreshTokenEntity.IsActive)
            {
                throw new AppException("Invalid token");
            }

            refreshTokenEntity.Revoked = revokedTime;
            refreshTokenEntity.RevokedByIp = revokedByIp;
            refreshTokenEntity.ReplacedByToken = replacedByRefreshToken as RefreshToken;

            _context.RefreshTokens.Update(refreshTokenEntity);
            return true;
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
