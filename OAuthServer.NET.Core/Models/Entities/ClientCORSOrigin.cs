using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("ClientsCORSOrigins")]
    public class ClientCORSOrigin : BaseEntity
    {
        public Guid ClientId { get; set; }

        public string OriginURI { get; set; }

        public Client Client { get; set; }
    }
}
