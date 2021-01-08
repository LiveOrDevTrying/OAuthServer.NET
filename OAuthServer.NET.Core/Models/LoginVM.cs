using System.ComponentModel.DataAnnotations;

namespace OAuthServer.NET.Core.Models
{
    public class LoginVM
    {
        [MaxLength(50)]
        [Required]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool EnableLocalLogin { get; set; }
        public bool AllowRememberLogin { get; set; }
        public bool RememberLogin { get; set; }
        public string ClientName { get; set; }
        public bool Show3rdPartyLoginGraphics { get; set; }

        public string Parameters { get; set; }

        public ExternalProvider[] VisibleExternalProviders { get; set; }
    }
}
