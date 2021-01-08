using System;

namespace OAuthServer.NET.UI.Models.DTOs
{
    public class ClientScopeDTO : BaseDTO
    {
        public Guid ClientId { get; set; }

        public string ScopeName { get; set; }
    }
}
