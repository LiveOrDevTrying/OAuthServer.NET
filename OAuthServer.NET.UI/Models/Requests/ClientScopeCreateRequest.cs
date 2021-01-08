using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientScopeCreateRequest
    {
        public Guid ClientId { get; set; }
        public string ScopeName { get; set; }
    }
}
