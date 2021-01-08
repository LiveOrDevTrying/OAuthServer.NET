using JWTs.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("AuthorizationCodes")]
    public class AuthorizationCode : BaseEntity, IAuthorizationCode
    {
        public string ApplicationUserId { get; set; }
        public Guid ClientId { get; set; }

        public string RedirectURI { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Client Client { get; set; }

        public string CodeDecoded
        {
            get
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(Code));
            }
        }
        public string RedirectURIDecoded
        {
            get
            {
                return WebUtility.HtmlDecode(RedirectURI);
            }
        }
    }
}
