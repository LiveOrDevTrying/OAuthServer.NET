using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthServer.NET.Core.Models.Entities
{
    [Table("Grants")]
    public class Grant : BaseEntity
    {
        public string GrantName { get; set; }
        public string AuthorizeResponseType { get; set; }
        public string TokenGrantType { get; set; }

        public ICollection<Client> Clients { get; set; } =
            new List<Client>();
    }
}
