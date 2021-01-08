using JWTs.Models;
using Microsoft.AspNetCore.Http;
using OAuthServer.NET.Models;
using System.Threading.Tasks;

namespace OAuthServer.NET.Providers
{
    public interface ITokenGrantProvider
    {
        string grant_type { get; }

        // Returns a token, or if there is a validation / error string, it is contained in the string return parameter.
        Task<(IToken, string)> GenerateTokenAsync(HttpRequest request, TokenRequest tokenRequest, string ipAddressRequestingToken);
    }
}
