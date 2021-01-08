using JWTs.Models;
using OAuthServer.NET.UI.Models.DTOs;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Models;
using System;
using System.Threading.Tasks;

namespace OAuthServer.NET.UI.Services
{
    public interface IDAL
    {
        Task<IToken> LoginAsync(LoginVM loginVM, JWTParameters parameters);

        Task<Payload> GetPayloadAsync();

        Task<ClientDTO> CreateClientAsync(ClientImplicitCreateRequest request);
        Task<ClientDTO> CreateClientAsync(ClientAuthorizationCodeCreateRequest request);
        Task<ClientDTO> CreateClientAsync(ClientClientCredentialsCreateRequest request);
        Task<ClientDTO> CreateClientAsync(ClientResourceOwnerPasswordCreateRequest request);
        Task<ClientDTO> UpdateClientAsync(ClientImplicitUpdateRequest request);
        Task<ClientDTO> UpdateClientAsync(ClientAuthorizationCodeUpdateRequest request);
        Task<ClientDTO> UpdateClientAsync(ClientClientCredentialsUpdateRequest request);
        Task<ClientDTO> UpdateClientAsync(ClientResourceOwnerPasswordUpdateRequest request);
        Task<bool> DeleteClientAsync(Guid id);

        Task<ClientScopeDTO> CreateClientScopeAsync(ClientScopeCreateRequest request);
        Task<ClientScopeDTO> UpdateClientScopeAsync(ClientScopeUpdateRequest request);
        Task<bool> DeleteClientScopeAsync(Guid id);

        Task<ClientCORSOriginDTO> CreateClientCORSOriginAsync(ClientCORSOriginCreateRequest request);
        Task<ClientCORSOriginDTO> UpdateClientCORSOriginAsync(ClientCORSOriginUpdateRequest request);
        Task<bool> DeleteClientCORSOriginAsync(Guid id);

        Task<ClientRedirectURIDTO> CreateClientRedirectURIAsync(ClientRedirectURICreateRequest request);
        Task<ClientRedirectURIDTO> UpdateClientRedirectURIAsync(ClientRedirectURIUpdateRequest request);
        Task<bool> DeleteClientRedirectURIAsync(Guid id);

        Task<ClientPostLogoutRedirectURIDTO> CreateClientPostLogoutRedirectURIAsync(ClientPostLogoutRedirectURICreateRequest request);
        Task<ClientPostLogoutRedirectURIDTO> UpdateClientPostLogoutRedirectURIAsync(ClientPostLogoutRedirectURIUpdateRequest request);
        Task<bool> DeleteClientPostLogoutRedirectURIAsync(Guid id);

        Task<GrantDTO> CreateGrantAsync(GrantDTO grant);
        Task<bool> UpdateGrantAsync(GrantDTO grant);
        Task<bool> DeleteGrantAsync(Guid id);
    }
}