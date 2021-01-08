using OAuthServer.NET.BLL.Authorize;

namespace OAuthServer.NET.Providers
{
    public class AuthorizeTypeProviderAuthorizationCode : BaseAuthorizeTypeProvider, IAuthorizeTypeProvider
    {
        public string response_type => "code";

        public AuthorizeTypeProviderAuthorizationCode(IAuthorizeBLL authorizeBLL) : base(authorizeBLL)
        {
        }
    }
}
