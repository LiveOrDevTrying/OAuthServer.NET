using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using OAuthServer.NET.BLL.Login;
using OAuthServer.NET.Core.Models;

namespace OAuthServer.NET.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginBLL _loginBLL;
        private readonly OAuthServerParams _parameters;

        public LoginController(
            ILogger<LoginController> logger, 
            OAuthServerParams parameters,
            ILoginBLL loginBLL)
        {
            _logger = logger;
            _parameters = parameters;
            _loginBLL = loginBLL;
        }

        [HttpGet]
        [Route("Login")]
        public virtual async Task<IActionResult> Login(string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                _logger.LogWarning("invalid_request", parameters);
                return BadRequest("invalid_request");
            }

            if (!await _loginBLL.LoadClientByQueryStringAsync(parameters))
            {
                _logger.LogWarning("invalid_client", parameters);
                return BadRequest("invalid_client");
            }

            if (!await _loginBLL.IsGrantValidAsync())
            {
                _logger.LogWarning("invalid_grant", parameters);
                return BadRequest("invalid_grant");
            }

            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("already logged in - issuing token and redircting", parameters);
                return Redirect(await _loginBLL.GenerateTokenAndRedirectURIAsync(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, IpAddress));
            }

            _logger.LogInformation("login requested", parameters);
            return View(new LoginVM
            {
                Parameters = parameters,
                AllowRememberLogin = _loginBLL.Client.AllowRememberLogin,
                EnableLocalLogin = _loginBLL.Client.EnabledLocalLogin,
                VisibleExternalProviders = await _loginBLL.GetExternalProvidersAsync(),
                RememberLogin = false, // Defaults to False unless the user checks this, and then authentication is automatic with cookies (need to call signout endpoint)
                ClientName = _loginBLL.Client.ClientName,
                Show3rdPartyLoginGraphics = _parameters.Show3rdPartyLoginGraphics
            });
        }

        [HttpPost]
        [Route("Login")]
        public virtual async Task<IActionResult> Login(LoginVM loginViewModel)
        {
            if (!await _loginBLL.LoadClientByQueryStringAsync(loginViewModel.Parameters))
            {
                _logger.LogWarning("invalid_request", loginViewModel.Username);
                return BadRequest("invalid_request");
            }

            if (!await _loginBLL.IsGrantValidAsync())
            {
                _logger.LogWarning("invalid_grant", loginViewModel.Username);
                return BadRequest("invalid_grant");
            }

            var applicationUser = await _loginBLL.GetApplicationUserAsync(loginViewModel.Username, loginViewModel.Password);

            if (applicationUser == null)
            {
                _logger.LogWarning("access_denied", loginViewModel.Username);
                return RedirectToAction("Login", new { parameters = $"{loginViewModel.Parameters}&error=access_denied" });
            }

            // Login - props contains information if login is persistant
            var (principal, props) = _loginBLL.GenerateCookie(applicationUser, loginViewModel.RememberLogin);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            _logger.LogInformation("login successful - issuing token and redircting", new object[] { applicationUser.Id, applicationUser.UserName });
            return Redirect(await _loginBLL.GenerateTokenAndRedirectURIAsync(applicationUser.Id, IpAddress));
        }
        [Authorize]
        [HttpGet]
        [Route("Logout")]
        public virtual async Task<IActionResult> Logout(string post_logout_redirect_uri)
        {
            var clientId = User.Claims.FirstOrDefault(x => x.Type == "client_id");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (clientId != null && !await _loginBLL.LoadClientByClientIdAsync(clientId.Value))
            {
                _logger.LogWarning("invalid_client", post_logout_redirect_uri);
                return BadRequest("invalid_client");
            }

            if (clientId != null && await _loginBLL.IsPostLogoutURIValidAsync(post_logout_redirect_uri))
            {
                _logger.LogInformation($"logged out - redirecting to {post_logout_redirect_uri}", post_logout_redirect_uri);
                return Redirect(post_logout_redirect_uri);
            }
            else
            {
                _logger.LogInformation($"logged out - redirecting to home - index");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("ExternalLogin")]
        public virtual async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Login", new { redirect = returnUrl });
            var properties = await _loginBLL.SignInExternalChallengeAsync(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [Route("ExternalLoginCallback")]
        public virtual async Task<IActionResult> ExternalLoginCallback(string redirect)
        {
            if (!await _loginBLL.LoadClientByQueryStringAsync(redirect.Substring(redirect.IndexOf("?") + 1)))
            {
                _logger.LogWarning("invalid_client", redirect);
                return BadRequest("invalid_client");
            }

            var applicationUser = await _loginBLL.SignInExternalLoginAsync();

            var (principal, props) = _loginBLL.GenerateCookie(applicationUser, false);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            if (applicationUser == null)
            {
                _logger.LogWarning("invalid_login", redirect);
                return BadRequest("invalid_login");
            }

            _logger.LogInformation("external login successful - issuing token and redirecting", new object[] { applicationUser.Id, applicationUser.UserName });
            return Redirect(await _loginBLL.GenerateTokenAndRedirectURIAsync(applicationUser.Id, IpAddress));
        }
    }
}
