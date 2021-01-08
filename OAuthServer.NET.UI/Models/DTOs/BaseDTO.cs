using System;

namespace OAuthServer.NET.UI.Models.DTOs
{
    public abstract class BaseDTO
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
