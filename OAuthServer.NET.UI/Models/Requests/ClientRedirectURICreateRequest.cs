using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientRedirectURICreateRequest
    {
        public Guid ClientId { get; set; }
        public string RedirectURI { get; set; }
    }
}
