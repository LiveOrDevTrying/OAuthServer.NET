
namespace OAuthServer.NET.Models
{
    public class TokenRequest
    {
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
