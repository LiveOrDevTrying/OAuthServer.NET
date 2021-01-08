using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("Scopes")]
    public class ClientScope : BaseEntity
    {
        public Guid ClientId { get; set; }

        public string ScopeName { get; set; }

        public Client Client { get; set; }
    }
}
