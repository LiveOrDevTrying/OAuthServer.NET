using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientPostLogoutRedirectURIUpdateRequest : ClientPostLogoutRedirectURICreateRequest
    {
        public Guid Id { get; set; }
    }
}
