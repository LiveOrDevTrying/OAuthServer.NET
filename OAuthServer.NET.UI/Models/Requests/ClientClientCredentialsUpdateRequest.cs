using System;

namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientClientCredentialsUpdateRequest : ClientClientCredentialsCreateRequest
    {
        public Guid Id { get; set; }
    }
}
