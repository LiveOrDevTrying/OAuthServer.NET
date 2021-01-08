using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OAuthServer.NET.CORS
{
    public class OAuthCorsPolicyProvider : ICorsPolicyProvider
    {
        public virtual Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            return Task.FromResult(new CorsPolicy());
        }
    }
}
