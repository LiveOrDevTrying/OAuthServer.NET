using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientCORSOriginUpdateRequest : ClientCORSOriginCreateRequest
    {
        public Guid Id { get; set; }
    }
}
