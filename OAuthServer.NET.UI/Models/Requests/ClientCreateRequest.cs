using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthServer.NET.UI.Models.Requests
{
    public abstract class ClientCreateRequest
    {
        public Guid GrantId { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public int TokenExpirationMin { get; set; } = 15;
        public string IssuerURI { get; set; }
        public string Audience { get; set; }
    }
}
