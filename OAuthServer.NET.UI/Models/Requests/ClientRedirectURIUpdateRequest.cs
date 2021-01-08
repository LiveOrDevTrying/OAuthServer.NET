using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientRedirectURIUpdateRequest : ClientRedirectURICreateRequest
    {
        public Guid Id { get; set; }
    }
}
