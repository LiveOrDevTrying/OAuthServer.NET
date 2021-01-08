using System;

namespace OAuthServer.NET.UI.Models.DTOs
{
    public class ClientCORSOriginDTO : BaseDTO
    {
        public Guid ClientId { get; set; }

        public string OriginURI { get; set; }
    }
}
