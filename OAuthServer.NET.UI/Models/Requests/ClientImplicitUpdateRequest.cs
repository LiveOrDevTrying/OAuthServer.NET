using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientImplicitUpdateRequest : ClientImplicitCreateRequest
    {
        public Guid Id { get; set; }
    }
}
