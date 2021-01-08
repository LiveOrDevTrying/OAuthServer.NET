using OAuthServer.NET.Core.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace OAuthServer.NET.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Grant> Grants { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientCORSOrigin> ClientsCORSOrigins { get; set; }
        public DbSet<ClientPostLogoutRedirectURI> ClientsPostLogoutRedirectURIs { get; set; }
        public DbSet<ClientRedirectURI> ClientsRedirectURIs { get; set; }
        public DbSet<ClientScope> ClientsScopes { get; set; }

        public DbSet<AuthorizationCode> AuthorizationCodes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Grant>()
                .HasIndex(x => x.AuthorizeResponseType)
                .IsUnique(true);

            modelBuilder.Entity<Grant>()
                .HasIndex(x => x.TokenGrantType)
                .IsUnique(true);

            modelBuilder.Entity<Grant>()
                .HasIndex(x => x.GrantName)
                .IsUnique(true);

            modelBuilder.Entity<Client>()
                .HasIndex(x => x.ClientId)
                .IsUnique(true);

            modelBuilder.Entity<ClientCORSOrigin>()
                .HasIndex(x => new { x.ClientId, x.OriginURI })
                .IsUnique(true);

            modelBuilder.Entity<ClientRedirectURI>()
                .HasIndex(x => new { x.ClientId, x.RedirectURI })
                .IsUnique(true);

            modelBuilder.Entity<AuthorizationCode>()
                .HasIndex(x => x.Code)
                .IsUnique(true);

            modelBuilder.Entity<AuthorizationCode>()
                .HasIndex(x => new { x.ApplicationUserId, x.ClientId, x.Code, x.RedirectURI })
                .IsUnique(true);

            modelBuilder.Entity<ClientScope>()
                .HasIndex(x => new { x.ClientId, x.ScopeName })
                .IsUnique(true);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(x => x.Token)
                .IsUnique(true);

            modelBuilder.Entity<Grant>()
                .HasData(new Grant[]
                {
                    new Grant
                    {
                        Id = Guid.Parse("3C7AC71F-2184-425C-AE0E-28E160872CD3"),
                        GrantName = "Implicit",
                        AuthorizeResponseType = "token",
                        Timestamp = new DateTime(2020, 12, 30, 12, 45, 0)
                        // No token grant type because no token endpoint is hit for implicit clients
                    },
                    new Grant
                    {
                        Id = Guid.Parse("00951B77-FE6F-4995-982E-1909DF5DABAC"),
                        GrantName = "Authorization Code",
                        AuthorizeResponseType = "code",
                        TokenGrantType = "authorization_code",
                        Timestamp = new DateTime(2020, 12, 30, 12, 45, 0)
                    },
                    new Grant
                    {
                        Id = Guid.Parse("A51637CB-AAF4-45EA-A195-D6059B835345"),
                        GrantName = "Resource Owner Password",
                        TokenGrantType = "password",
                        Timestamp = new DateTime(2020, 12, 30, 12, 45, 0)
                    },
                    new Grant
                    {
                        Id = Guid.Parse("F320146F-7D07-4DC1-BAD9-770E34702CB5"),
                        GrantName = "Client Credentials",
                        TokenGrantType ="client_credentials",
                        Timestamp = new DateTime(2020, 12, 30, 12, 45, 0)
                    }
                });
        }
    }
}
