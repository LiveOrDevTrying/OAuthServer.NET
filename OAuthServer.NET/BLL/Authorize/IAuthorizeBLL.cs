using System.Threading.Tasks;

namespace OAuthServer.NET.BLL.Authorize
{
    public interface IAuthorizeBLL : IBaseResponseTypeBLL
    {
        Task<bool> LoadClientAsync(string response_type, string client_id, string redirect_uri, string scope, string state);
    }
}