using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace OAuthServer.NET.Services
{
    public class OAuthServerServices
    {
        protected readonly OAuthServerDAL _dal;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly OAuthServerParams _parameters;
        protected readonly IWebHostEnvironment _environment;

        public OAuthServerServices(OAuthServerDAL dal, RoleManager<IdentityRole> roleManager, OAuthServerParams parameters, IWebHostEnvironment environment)
        {
            _dal = dal;
            _roleManager = roleManager;
            _parameters = parameters;
            _environment = environment;
        }

        public OAuthServerDAL DAL
        {
            get
            {
                return _dal;
            }
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager;
            }
        }
        public OAuthServerParams Parameters
        {
            get
            {
                return _parameters;
            }
        }
        public bool IsDevelopment
        {
            get
            {
                return _environment.IsDevelopment();
            }
        }
    }
}
