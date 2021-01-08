using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OAuthServer.NET.Providers;
using System.Linq;
using System.Collections.Generic;
using OAuthServer.NET.Models;

namespace OAuthServer.NET.Controllers
{
    [Route("Connect")]
    public class TokenController : BaseController
    {
        protected readonly ILogger<TokenController> _logger;
        protected readonly IEnumerable<ITokenGrantProvider> _tokenProviders;

        public TokenController(ILogger<TokenController> logger,
            IEnumerable<ITokenGrantProvider> grantProviders)
        {
            _logger = logger;
            _tokenProviders = grantProviders;
        }

        [HttpPost]
        [Route("Token")]
        [Consumes("application/x-www-form-urlencoded")]
        public virtual async Task<IActionResult> Token([FromForm]TokenRequest tokenRequest)
        {
            var provider = _tokenProviders.SingleOrDefault(x => x.grant_type == tokenRequest.grant_type);

            if (provider == null)
            {
                _logger.LogWarning("invalid_grant", tokenRequest);
                return BadRequest("invalid_grant");
            }

            var (token, error) = await provider.GenerateTokenAsync(Request, tokenRequest, IpAddress);

            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.LogWarning(error, tokenRequest);
                return BadRequest(error);
            }

            _logger.LogInformation($"token issued for {tokenRequest.client_id}" + (!string.IsNullOrWhiteSpace(tokenRequest.redirect_uri) ? "with redirect {tokenRequest.redirect_uri}" : string.Empty), new object[] { tokenRequest, token });
            return Ok(token);
        }
    }
}
