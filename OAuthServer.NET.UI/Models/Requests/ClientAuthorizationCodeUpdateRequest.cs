using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientAuthorizationCodeUpdateRequest : ClientAuthorizationCodeCreateRequest
    {
        public Guid Id { get; set; }
    }
}
