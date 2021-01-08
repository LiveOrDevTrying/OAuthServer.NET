using Microsoft.AspNetCore.Http;
using OAuthServer.NET.BLL.Token;
using OAuthServer.NET.Models;
using System.Threading.Tasks;

namespace OAuthServer.NET.Providers
{
    public abstract class BaseTokenGrantProvider
    {
        protected readonly ITokenBLL _tokenBLL;

        public BaseTokenGrantProvider(ITokenBLL tokenBLL)
        {
            _tokenBLL = tokenBLL;
        }

        protected virtual async Task<(bool, string)> ValidateAsync(HttpRequest request, TokenRequest tokenRequest)
        {
            if (!await _tokenBLL.LoadClientAndTokenRequestAsync(request.Headers["Authorization"], tokenRequest))
            {
                return (false, "invalid_client");
            }

            if (!await _tokenBLL.IsGrantValidAsync(tokenRequest.grant_type))
            {
                return (false, "invalid_grant");
            }

            return (true, null);
        }
    }
}
