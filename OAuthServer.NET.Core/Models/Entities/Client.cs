using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("Clients")]
    public class Client : BaseEntity
    {
        public Guid GrantId { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int TokenExpirationMin { get; set; } = 15;

        public bool IssueRefreshTokens { get; set; }
        public int RefreshTokenExpirationDays { get; set; } = 15;

        // URI who generates the JWTs
        public string IssuerURI { get; set; }
        // Name of the resource who the JWT is intended for
        public string Audience { get; set; }

        public bool AllowRememberLogin { get; set; } = true;
        public bool EnabledLocalLogin { get; set; } = true;
        public bool EnableExternalLogin { get; set; } = true;

        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool ValidateCORS { get; set; } = true;

        public Grant Grant { get; set; }

        public ICollection<ClientCORSOrigin> ClientCORSOrigins { get; set; } =
            new List<ClientCORSOrigin>();

        public ICollection<ClientRedirectURI> ClientRedirectURIs { get; set; } =
            new List<ClientRedirectURI>();

        public ICollection<ClientPostLogoutRedirectURI> ClientPostLogoutRedirectURIs { get; set; } =
            new List<ClientPostLogoutRedirectURI>();

        public ICollection<AuthorizationCode> AuthorizationCodes { get; set; } =
            new List<AuthorizationCode>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } =
            new List<RefreshToken>();

        public ICollection<ClientScope> ClientScopes { get; set; } =
            new List<ClientScope>();
        
        public string IssuerURIHtmlDecoded
        {
            get
            {
                return WebUtility.HtmlDecode(IssuerURI);
            }
        }
        public string ClientSecretDecoded
        {
            get
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(ClientSecret));
            }
        }
    }
}
