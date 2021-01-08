namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientResourceOwnerPasswordCreateRequest : ClientCreateRequest
    {
        public string ClientSecret { get; set; }

        public bool IssueRefreshTokens { get; set; }
        public int RefreshTokenExpirationDays { get; set; } = 15;

        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool ValidateCORS { get; set; } = false;
    }
}
