using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientResourceOwnerPasswordUpdateRequest : ClientResourceOwnerPasswordCreateRequest
    {
        public Guid Id { get; set; }
    }
}
