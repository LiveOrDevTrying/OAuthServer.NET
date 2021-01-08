using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientPostLogoutRedirectURICreateRequest
    {
        public Guid ClientId { get; set; }
        public string PostLogoutRedirectURI { get; set; }
    }
}
