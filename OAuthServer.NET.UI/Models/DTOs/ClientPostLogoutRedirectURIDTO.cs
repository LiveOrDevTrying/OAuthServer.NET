using System;

namespace OAuthServer.NET.UI.Models.DTOs
{
    public class ClientPostLogoutRedirectURIDTO : BaseDTO
    {
        public Guid ClientId { get; set; }

        public string PostLogoutRedirectURI { get; set; }
    }
}
