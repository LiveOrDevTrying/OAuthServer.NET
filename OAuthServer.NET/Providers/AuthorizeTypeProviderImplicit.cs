using OAuthServer.NET.BLL.Authorize;

namespace OAuthServer.NET.Providers
{
    public class AuthorizeTypeProviderImplicit : BaseAuthorizeTypeProvider, IAuthorizeTypeProvider
    {
        public string response_type => "token";

        public AuthorizeTypeProviderImplicit(IAuthorizeBLL authorizeBLL) : base(authorizeBLL)
        {
        }
    }
}
