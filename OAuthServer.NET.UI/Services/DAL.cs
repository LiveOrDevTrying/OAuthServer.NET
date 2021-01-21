using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthServer.NET.Core.Data;
using OAuthServer.NET.UI.Models.DTOs;
using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JWTs;
using JWTs.Models;

namespace OAuthServer.NET.UI.Services
{
    public class DAL : IDAL
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IJWTService _jwtService;

        public DAL(ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IJWTService jwtService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public virtual async Task<IToken> LoginAsync(LoginVM loginVM, JWTParameters parameters)
        {
            var applicationUser = await _userManager.FindByNameAsync(loginVM.Username);

            if (applicationUser == null)
            {
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(applicationUser, loginVM.Password))
            {
                return null;
            }

            if (!await _userManager.IsInRoleAsync(applicationUser, "admin"))
            {
                return null;
            }

            return await _jwtService.GenerateTokenAsync(applicationUser.Id, parameters.Audience, parameters.IssuerURI, parameters.State, parameters.TokenExpirationMin, parameters.SigningKey);
        }

        public virtual async Task<Payload> GetPayloadAsync()
        {
            return new Payload
            {
                Clients = await GetClientsAsync(),
                ClientsCORSOrigins = await GetClientsCORSOriginsAsync(),
                ClientsPostLogoutRedirectURIs = await GetClientsPostLogoutRedirectURIsAsync(),
                ClientsRedirectURIs = await GetClientsRedirectURIsAsync(),
                ClientsScopes = await GetClientsScopesAsync(),
                Grants = await GetGrantsAsync()
            };
        }
        protected virtual async Task<ClientDTO[]> GetClientsAsync()
        {
            var clients = await _context.Clients.ToArrayAsync();
            var dtos = new List<ClientDTO>();

            foreach (var client in clients)
            {
                var dto = _mapper.Map<ClientDTO>(client);

                if (!string.IsNullOrWhiteSpace(client.ClientSecret))
                {
                    dto.ClientSecret = client.ClientSecretDecoded;
                }
                dtos.Add(dto);
            }

            return dtos.ToArray();
        }
        protected virtual async Task<GrantDTO[]> GetGrantsAsync()
        {
            var grants = await _context.Grants.ToArrayAsync();
            var dtos = new List<GrantDTO>();

            foreach (var grant in grants)
            {
                dtos.Add(_mapper.Map<GrantDTO>(grant));
            }

            return dtos.ToArray();
        }
        protected virtual async Task<ClientPostLogoutRedirectURIDTO[]> GetClientsPostLogoutRedirectURIsAsync()
        {
            var clientsPostLogoutRedirectURIs = await _context.ClientsPostLogoutRedirectURIs.ToArrayAsync();
            var dtos = new List<ClientPostLogoutRedirectURIDTO>();

            foreach (var clientPostLogoutRedirectURI in clientsPostLogoutRedirectURIs)
            {
                dtos.Add(_mapper.Map<ClientPostLogoutRedirectURIDTO>(clientPostLogoutRedirectURI));
            }

            return dtos.ToArray();
        }
        protected virtual async Task<ClientRedirectURIDTO[]> GetClientsRedirectURIsAsync()
        {
            var clientsRedirectURIs = await _context.ClientsRedirectURIs.ToArrayAsync();
            var dtos = new List<ClientRedirectURIDTO>();

            foreach (var clientRedirectURI in clientsRedirectURIs)
            {
                dtos.Add(_mapper.Map<ClientRedirectURIDTO>(clientRedirectURI));
            }

            return dtos.ToArray();
        }
        protected virtual async Task<ClientScopeDTO[]> GetClientsScopesAsync()
        {
            var scopes = await _context.ClientsScopes.ToArrayAsync();
            var dtos = new List<ClientScopeDTO>();

            foreach (var scope in scopes)
            {
                dtos.Add(_mapper.Map<ClientScopeDTO>(scope));
            }

            return dtos.ToArray();
        }
        protected virtual async Task<ClientCORSOriginDTO[]> GetClientsCORSOriginsAsync()
        {
            var corsOrigins = await _context.ClientsCORSOrigins.ToArrayAsync();
            var dtos = new List<ClientCORSOriginDTO>();

            foreach (var corsOrigin in corsOrigins)
            {
                dtos.Add(_mapper.Map<ClientCORSOriginDTO>(corsOrigin));
            }

            return dtos.ToArray();
        }

        public virtual async Task<ClientDTO> CreateClientAsync(ClientImplicitCreateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.SigningKey));

            var client = new Client
            {
                AllowRememberLogin = request.AllowRememberLogin,
                Audience = request.Audience,
                ValidateAudience = request.ValidateAudience,
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                ClientSecret = base64EncodedSecret,
                EnabledLocalLogin = request.EnabledLocalLogin,
                EnableExternalLogin = request.EnableExternalLogin,
                IssueRefreshTokens = false,
                IssuerURI = issuerURI,
                RefreshTokenExpirationDays = 0,
                ValidateIssuerSigningKey = request.ValidateSigningKey,
                TokenExpirationMin = request.TokenExpirationMin,
                ValidateCORS = request.ValidateCORS,
                GrantId = request.GrantId,
                ValidateIssuer = request.ValidateIssuer
            };
            _context.Clients.Add(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> CreateClientAsync(ClientAuthorizationCodeCreateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.ClientSecret));

            var client = new Client
            {
                AllowRememberLogin = request.AllowRememberLogin,
                Audience = request.Audience,
                ValidateAudience = request.ValidateAudience,
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                ClientSecret = base64EncodedSecret,
                EnabledLocalLogin = request.EnabledLocalLogin,
                EnableExternalLogin = request.EnableExternalLogin,
                IssueRefreshTokens = request.IssueRefreshTokens,
                IssuerURI = issuerURI,
                RefreshTokenExpirationDays = request.RefreshTokenExpirationDays,
                ValidateIssuerSigningKey = request.ValidateIssuerSigningKey,
                TokenExpirationMin = request.TokenExpirationMin,
                ValidateCORS = request.ValidateCORS,
                GrantId = request.GrantId,
                ValidateIssuer = request.ValidateIssuer,
            };
            _context.Clients.Add(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> CreateClientAsync(ClientClientCredentialsCreateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.ClientSecret));

            var client = new Client
            {
                AllowRememberLogin = false,
                Audience = request.Audience,
                ValidateAudience = request.ValidateAudience,
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                ClientSecret = base64EncodedSecret,
                EnabledLocalLogin = false,
                EnableExternalLogin = false,
                IssueRefreshTokens = request.IssueRefreshTokens,
                IssuerURI = issuerURI,
                RefreshTokenExpirationDays = request.RefreshTokenExpirationDays,
                ValidateIssuerSigningKey = request.ValidateIssuerSigningKey,
                TokenExpirationMin = request.TokenExpirationMin,
                ValidateCORS = request.ValidateCORS,
                GrantId = request.GrantId,
                ValidateIssuer = request.ValidateIssuer,
            };
            _context.Clients.Add(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> CreateClientAsync(ClientResourceOwnerPasswordCreateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.ClientSecret));

            var client = new Client
            {
                AllowRememberLogin = false,
                Audience = request.Audience,
                ValidateAudience = request.ValidateAudience,
                ClientId = request.ClientId,
                ClientName = request.ClientName,
                ClientSecret = base64EncodedSecret,
                EnabledLocalLogin = false,
                EnableExternalLogin = false,
                IssueRefreshTokens = request.IssueRefreshTokens,
                IssuerURI = issuerURI,
                RefreshTokenExpirationDays = request.RefreshTokenExpirationDays,
                ValidateIssuerSigningKey = request.ValidateIssuerSigningKey,
                TokenExpirationMin = request.TokenExpirationMin,
                ValidateCORS = request.ValidateCORS,
                GrantId = request.GrantId,
                ValidateIssuer = request.ValidateIssuer,
            };
            _context.Clients.Add(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> UpdateClientAsync(ClientImplicitUpdateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.SigningKey));

            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (client != null)
            {
                client.AllowRememberLogin = request.AllowRememberLogin;
                client.Audience = request.Audience;
                client.ValidateAudience = request.ValidateAudience;
                client.ClientId = request.ClientId;
                client.ClientName = request.ClientName;
                client.ClientSecret = base64EncodedSecret;
                client.EnabledLocalLogin = request.EnabledLocalLogin;
                client.EnableExternalLogin = request.EnableExternalLogin;
                client.IssueRefreshTokens = false;
                client.IssuerURI = issuerURI;
                client.RefreshTokenExpirationDays = 0;
                client.ValidateIssuerSigningKey = request.ValidateSigningKey;
                client.TokenExpirationMin = request.TokenExpirationMin;
                client.ValidateCORS = request.ValidateCORS;
                client.GrantId = request.GrantId;
                client.ValidateIssuer = request.ValidateIssuer;
            }
            _context.Clients.Update(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> UpdateClientAsync(ClientAuthorizationCodeUpdateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.ClientSecret));

            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (client != null)
            {
                client.AllowRememberLogin = request.AllowRememberLogin;
                client.Audience = request.Audience;
                client.ValidateAudience = request.ValidateAudience;
                client.ClientId = request.ClientId;
                client.ClientName = request.ClientName;
                client.ClientSecret = base64EncodedSecret;
                client.EnabledLocalLogin = request.EnabledLocalLogin;
                client.EnableExternalLogin = request.EnableExternalLogin;
                client.IssueRefreshTokens = request.IssueRefreshTokens;
                client.IssuerURI = issuerURI;
                client.RefreshTokenExpirationDays = request.RefreshTokenExpirationDays;
                client.ValidateIssuerSigningKey = request.ValidateIssuerSigningKey; ;
                client.TokenExpirationMin = request.TokenExpirationMin;
                client.ValidateCORS = request.ValidateCORS;
                client.GrantId = request.GrantId;
                client.ValidateIssuer = request.ValidateIssuer;
            }
            _context.Clients.Update(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> UpdateClientAsync(ClientClientCredentialsUpdateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.ClientSecret));

            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (client != null)
            {
                client.AllowRememberLogin = false;
                client.Audience = request.Audience;
                client.ValidateAudience = request.ValidateAudience;
                client.ClientId = request.ClientId;
                client.ClientName = request.ClientName;
                client.ClientSecret = base64EncodedSecret;
                client.EnabledLocalLogin = false;
                client.EnableExternalLogin = false;
                client.IssueRefreshTokens = request.IssueRefreshTokens;
                client.IssuerURI = issuerURI;
                client.RefreshTokenExpirationDays = request.RefreshTokenExpirationDays;
                client.ValidateIssuerSigningKey = request.ValidateIssuerSigningKey;
                client.TokenExpirationMin = request.TokenExpirationMin;
                client.ValidateCORS = request.ValidateCORS;
                client.GrantId = request.GrantId;
                client.ValidateIssuer = request.ValidateIssuer;
            }
            _context.Clients.Update(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<ClientDTO> UpdateClientAsync(ClientResourceOwnerPasswordUpdateRequest request)
        {
            var issuerURI = WebUtility.HtmlEncode(request.IssuerURI);
            var base64EncodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.ClientSecret));

            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (client != null)
            {
                client.AllowRememberLogin = false;
                client.Audience = request.Audience;
                client.ValidateAudience = request.ValidateAudience;
                client.ClientId = request.ClientId;
                client.ClientName = request.ClientName;
                client.ClientSecret = base64EncodedSecret;
                client.EnabledLocalLogin = false;
                client.EnableExternalLogin = false;
                client.IssueRefreshTokens = request.IssueRefreshTokens;
                client.IssuerURI = issuerURI;
                client.RefreshTokenExpirationDays = request.RefreshTokenExpirationDays;
                client.ValidateIssuerSigningKey = request.ValidateIssuerSigningKey;
                client.TokenExpirationMin = request.TokenExpirationMin;
                client.ValidateCORS = request.ValidateCORS;
                client.GrantId = request.GrantId;
                client.ValidateIssuer = request.ValidateIssuer;
            }
            _context.Clients.Update(client);

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ClientDTO>(client) : null;
        }
        public virtual async Task<bool> DeleteClientAsync(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client != null)
            {
                _context.Clients.Remove(client);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public virtual async Task<ClientScopeDTO> CreateClientScopeAsync(ClientScopeCreateRequest request)
        {
            var entity = new ClientScope
            {
                ClientId = request.ClientId,
                ScopeName = request.ScopeName,
            };
            _context.ClientsScopes.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientScopeDTO>(entity);
            }

            return null;
        }
        public virtual async Task<ClientScopeDTO> UpdateClientScopeAsync(ClientScopeUpdateRequest request)
        {
            var entity = await _context.ClientsScopes
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity != null)
            {
                entity.ScopeName = request.ScopeName;
                entity.Timestamp = DateTime.UtcNow;
                _context.ClientsScopes.Update(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<ClientScopeDTO>(entity);
                }
            }

            return null;
        }
        public virtual async Task<bool> DeleteClientScopeAsync(Guid id)
        {
            var clientScope = await _context.ClientsScopes.FirstOrDefaultAsync(x => x.Id == id);

            if (clientScope != null)
            {
                _context.ClientsScopes.Remove(clientScope);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public virtual async Task<ClientCORSOriginDTO> CreateClientCORSOriginAsync(ClientCORSOriginCreateRequest request)
        {
            var entity = new ClientCORSOrigin
            {
                ClientId = request.ClientId,
                OriginURI = request.OriginURI,
            };
            _context.ClientsCORSOrigins.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientCORSOriginDTO>(entity);
            }

            return null;
        }
        public virtual async Task<ClientCORSOriginDTO> UpdateClientCORSOriginAsync(ClientCORSOriginUpdateRequest request)
        {
            var entity = await _context.ClientsCORSOrigins
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity != null)
            {
                entity.OriginURI = request.OriginURI;
                entity.Timestamp = DateTime.UtcNow;
                _context.ClientsCORSOrigins.Update(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<ClientCORSOriginDTO>(entity);
                }
            }

            return null;
        }
        public virtual async Task<bool> DeleteClientCORSOriginAsync(Guid id)
        {
            var clientCORSOrigin = await _context.ClientsCORSOrigins.FirstOrDefaultAsync(x => x.Id == id);

            if (clientCORSOrigin != null)
            {
                _context.ClientsCORSOrigins.Remove(clientCORSOrigin);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public virtual async Task<ClientRedirectURIDTO> CreateClientRedirectURIAsync(ClientRedirectURICreateRequest request)
        {
            var entity = new ClientRedirectURI
            {
                ClientId = request.ClientId,
                RedirectURI = request.RedirectURI,
            };
            _context.ClientsRedirectURIs.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientRedirectURIDTO>(entity);
            }

            return null;
        }
        public virtual async Task<ClientRedirectURIDTO> UpdateClientRedirectURIAsync(ClientRedirectURIUpdateRequest request)
        {
            var entity = await _context.ClientsRedirectURIs
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity != null)
            {
                entity.RedirectURI = request.RedirectURI;
                entity.Timestamp = DateTime.UtcNow;
                _context.ClientsRedirectURIs.Update(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<ClientRedirectURIDTO>(entity);
                }
            }

            return null;
        }
        public virtual async Task<bool> DeleteClientRedirectURIAsync(Guid id)
        {
            var clientRedirectURI = await _context.ClientsRedirectURIs.FirstOrDefaultAsync(x => x.Id == id);

            if (clientRedirectURI != null)
            {
                _context.ClientsRedirectURIs.Remove(clientRedirectURI);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public virtual async Task<ClientPostLogoutRedirectURIDTO> CreateClientPostLogoutRedirectURIAsync(ClientPostLogoutRedirectURICreateRequest request)
        {
            var entity = new ClientPostLogoutRedirectURI
            {
                ClientId = request.ClientId,
                PostLogoutRedirectURI = request.PostLogoutRedirectURI,
            };
            _context.ClientsPostLogoutRedirectURIs.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientPostLogoutRedirectURIDTO>(entity);
            }

            return null;
        }
        public virtual async Task<ClientPostLogoutRedirectURIDTO> UpdateClientPostLogoutRedirectURIAsync(ClientPostLogoutRedirectURIUpdateRequest request)
        {
            var entity = await _context.ClientsPostLogoutRedirectURIs
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity != null)
            {
                entity.PostLogoutRedirectURI = request.PostLogoutRedirectURI;
                entity.Timestamp = DateTime.UtcNow;
                _context.ClientsPostLogoutRedirectURIs.Update(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<ClientPostLogoutRedirectURIDTO>(entity);
                }
            }

            return null;
        }
        public virtual async Task<bool> DeleteClientPostLogoutRedirectURIAsync(Guid id)
        {
            var clientPostLogoutRedirectURI = await _context.ClientsPostLogoutRedirectURIs.FirstOrDefaultAsync(x => x.Id == id);

            if (clientPostLogoutRedirectURI != null)
            {
                _context.ClientsPostLogoutRedirectURIs.Remove(clientPostLogoutRedirectURI);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public virtual async Task<GrantDTO> CreateGrantAsync(GrantDTO grant)
        {
            var entity = new Grant
            {
                GrantName = grant.GrantName,
                TokenGrantType = grant.TokenGrantType,
                AuthorizeResponseType = grant.AuthorizeResponseType,
            };
            _context.Grants.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<GrantDTO>(entity);
            }

            return null;
        }
        public virtual async Task<bool> UpdateGrantAsync(GrantDTO grant)
        {
            var entity = await _context.Grants
                .FirstOrDefaultAsync(x => x.Id == grant.Id);

            if (entity != null) 
            {
                entity.GrantName = grant.GrantName;
                entity.AuthorizeResponseType = grant.AuthorizeResponseType;
                entity.TokenGrantType = grant.TokenGrantType;
                entity.Timestamp = DateTime.UtcNow;
                _context.Grants.Update(entity);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }
        public virtual async Task<bool> DeleteGrantAsync(Guid id)
        {
            var grant = await _context.Grants.FirstOrDefaultAsync(x => x.Id == id);

            if (grant != null)
            {
                _context.Grants.Remove(grant);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }
    }
}
