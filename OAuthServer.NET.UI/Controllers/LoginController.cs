using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OAuthServer.NET.UI.Models;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected readonly IDAL _dal;
        protected readonly IConfiguration _configuration;

        public LoginController(IDAL dal,
            IConfiguration configuration)
        {
            _dal = dal;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {
            var token = await _dal.LoginAsync(loginVM, GenerateJWTParameters());

            if (token == null)
            {
                return Ok();
            }

            return Ok(token);
        }

        protected virtual JWTParameters GenerateJWTParameters()
        {
            return new JWTParameters
            {
                Audience = Startup.AUDIENCE,
                IPAddressIssuingToken = IpAddress,
                IssuerURI = Startup.ISSUER,
                TokenExpirationMin = 60,
                SigningKey = Startup.SIGNING_KEY,
            };
        }

        protected string IpAddress
        {
            get
            {
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    return Request.Headers["X-Forwarded-For"];
                }
                else
                {
                    return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
            }
        }
    }
}
