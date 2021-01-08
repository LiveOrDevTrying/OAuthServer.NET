using OAuthServer.NET.UI.Models.DTOs;

namespace OAuthServer.NET.UI.Models
{
    public class Payload
    {
        public ClientDTO[] Clients { get; set; }
        public GrantDTO[] Grants { get; set; }
        public ClientPostLogoutRedirectURIDTO[] ClientsPostLogoutRedirectURIs { get; set; }
        public ClientRedirectURIDTO[] ClientsRedirectURIs { get; set; }
        public ClientCORSOriginDTO[] ClientsCORSOrigins { get; set; }
        public ClientScopeDTO[] ClientsScopes { get; set; }
    }
}
