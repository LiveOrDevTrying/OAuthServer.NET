using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OAuthServer.NET.Providers;

namespace OAuthServer.NET.Controllers
{
    [Route("Connect")]
    public class AuthorizeController : BaseController
    {
        private readonly ILogger<AuthorizeController> _logger;
        private readonly IEnumerable<IAuthorizeTypeProvider> _authorizeProviders;

        public AuthorizeController(ILogger<AuthorizeController> logger,
            IEnumerable<IAuthorizeTypeProvider> authorizeProviders)
        {
            _logger = logger;
            _authorizeProviders = authorizeProviders;
        }

        [HttpGet]
        [Route("Authorize")]
        public virtual async Task<IActionResult> Authorize()
        {
            var response_type = Request.Query["response_type"].ToString();
            var redirect_uri = Request.Query["redirect_uri"].ToString();

            var provider = _authorizeProviders.SingleOrDefault(x => x.response_type == response_type);

            if (provider == null)
            {
                _logger.LogWarning("invalid_request", Request.QueryString);
                Response.Headers.Add("Location", $"{redirect_uri}{Request.QueryString}&error=invalid_request");
                return BadRequest();
            }

            var (success, error) = await provider.ValidateAsync(Request);

            if (!success)
            {
                _logger.LogWarning("invalid_request", Request.QueryString);
                Response.Headers.Add("Location", $"{redirect_uri}{Request.QueryString}&error={error}");
                return BadRequest();
            }

            _logger.LogWarning("authorize validated - redirecting to login", Request.QueryString);
            return RedirectToAction($"Login", "Login", new { parameters = Request.QueryString });
        }
    }
}
