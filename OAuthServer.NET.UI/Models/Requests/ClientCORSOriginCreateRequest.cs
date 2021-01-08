using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientCORSOriginCreateRequest
    {
        public Guid ClientId { get; set; }
        public string OriginURI { get; set; }
    }
}
