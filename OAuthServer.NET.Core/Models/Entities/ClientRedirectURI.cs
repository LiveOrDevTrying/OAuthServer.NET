using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("ClientsRedirectURIs")]
    public class ClientRedirectURI : BaseEntity
    {
        public Guid ClientId { get; set; }

        public string RedirectURI { get; set; }

        public Client Client { get; set; }
    }
}
