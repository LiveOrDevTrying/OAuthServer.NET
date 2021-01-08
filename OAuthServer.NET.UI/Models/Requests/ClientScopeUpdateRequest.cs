using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientScopeUpdateRequest : ClientScopeCreateRequest
    {
        public Guid Id { get; set; }
    }
}
