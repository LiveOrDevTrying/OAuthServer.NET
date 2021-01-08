using System;

namespace OAuthServer.NET.UI.Models.DTOs
{
    public class GrantDTO : BaseDTO
    {
        public string GrantName { get; set; }
        public string AuthorizeResponseType { get; set; }
        public string TokenGrantType { get; set; }
    }
}
