using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OAuthServer.NET.Providers
{
    public interface IAuthorizeTypeProvider
    {
        string response_type { get; }

        // Returns true if successful, or if there is a validation / error string, it is contained in the string return parameter.
        Task<(bool, string)> ValidateAsync (HttpRequest request);
    }
}
