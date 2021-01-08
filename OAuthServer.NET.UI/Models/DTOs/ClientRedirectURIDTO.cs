using System;

namespace OAuthServer.NET.UI.Models.DTOs
{
    public class ClientRedirectURIDTO : BaseDTO
    {
        public Guid ClientId { get; set; }

        public string RedirectURI { get; set; }
    }
}
