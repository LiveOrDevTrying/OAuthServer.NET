using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OAuthServer.NET.Core.Data;
using OAuthServer.NET.Core.Middleware;
using OAuthServer.NET.UI.Models.DTOs;
using OAuthServer.NET.Core.Models.Entities;
using OAuthServer.NET.UI.Services;
using System.Text;
using JWTs;
using OAuthServer.NET.Core.Models.Exceptions;
using System;
using System.Security.Cryptography;

namespace OAuthServer.NET.UI
{
    public class Startup
    {
        public static string SIGNING_KEY { get; } = GetSigningKey();
        public static string ISSUER { get; } = Guid.NewGuid().ToString();
        public static string AUDIENCE { get; } = Guid.NewGuid().ToString();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (string.IsNullOrWhiteSpace(Configuration["ConnectionStrings:Database"]))
            {
                throw new AppException("Invalid connection string");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Database"]);
            });

            services.AddTransient<IDAL, DAL>();
            services.AddTransient<IJWTService, JWTService>();

            services.AddSingleton(CreateAutoMapper());

            services.AddDefaultIdentity<ApplicationUser>()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ISSUER,
                        ValidAudience = AUDIENCE,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SIGNING_KEY))
                    };
                });

            services.AddCors();

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "WebApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "WebApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        protected IMapper CreateAutoMapper()
        {
            return new MapperConfiguration(c =>
            {
                c.CreateMap<Client, ClientDTO>()
                    .ForMember(x => x.ClientSecret, o => o.MapFrom(t => t.ClientSecretDecoded));
                c.CreateMap<Grant, GrantDTO>();
                c.CreateMap<ClientCORSOrigin, ClientCORSOriginDTO>();
                c.CreateMap<ClientPostLogoutRedirectURI, ClientPostLogoutRedirectURIDTO>();
                c.CreateMap<ClientRedirectURI, ClientRedirectURIDTO>();
                c.CreateMap<ClientScope, ClientScopeDTO>();
            }).CreateMapper();
        }
        private static string GetSigningKey()
        {
            using var cryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[256];
            cryptoServiceProvider.GetBytes(randomBytes);

            // Convert random bytes to a hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
