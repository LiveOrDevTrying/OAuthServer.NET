namespace OAuthServer.NET.UI.Models.Requests
{
    public class ClientImplicitCreateRequest : ClientCreateRequest
    {
        public string SigningKey { get; set; }
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnabledLocalLogin { get; set; } = true;
        public bool EnableExternalLogin { get; set; } = true;

        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateSigningKey { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateCORS { get; set; } = true;
    }
}
