using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OAuthServer.NET.Core.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<AuthorizationCode> AuthorizationCodes { get; set; } =
            new List<AuthorizationCode>();

        public ICollection<RefreshToken> RefreshTokens { get; set; }
             = new List<RefreshToken>();
    }
}
