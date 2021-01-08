using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthServer.NET.UI.Models
{
    public class JWTParameters
    {
        public string Audience { get; set; }
        public string IssuerURI { get; set; }
        public string State { get; set; }
        public int TokenExpirationMin { get; set; }
        public string SigningKey { get; set; }
        public string IPAddressIssuingToken { get; set; }
    }
}
