using JWTs.Models;
using System;

namespace OAuthServer.NET.Core.Models.Entities
{
    public class RefreshToken : BaseEntity, IRefreshToken
    {
        public string ApplicationUserId { get; set; }
        public Guid ClientId { get; set; }
        public Guid? ReplacedByTokenId { get; set; }

        public string Token { get; set; }
        public string Scopes { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public RefreshToken ReplacedByToken { get; set; }
        public Client Client { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
