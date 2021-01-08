using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("ClientsPostLogoutRedirectURIs")]
    public class ClientPostLogoutRedirectURI : BaseEntity
    {
        public Guid ClientId { get; set; }

        public string PostLogoutRedirectURI { get; set; }

        public Client Client { get; set; }
    }
}
