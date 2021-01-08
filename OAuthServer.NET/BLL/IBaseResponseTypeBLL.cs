using System.Threading.Tasks;

namespace OAuthServer.NET.BLL
{
    public interface IBaseResponseTypeBLL : IBaseBLL
    {
        Task<bool> IsAuthorizeResponseTypeValidAsync();
        Task<bool> IsGrantValidAsync();
        Task<bool> IsScopeValidAsync();
    }
}