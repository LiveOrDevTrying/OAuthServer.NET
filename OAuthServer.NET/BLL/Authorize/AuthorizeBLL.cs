using OAuthServer.NET.Services;
using System.Threading.Tasks;

namespace OAuthServer.NET.BLL.Authorize
{
    public class AuthorizeBLL : BaseResponseTypeBLL<BLLResponseTypeParameters>, IAuthorizeBLL
    {
        public AuthorizeBLL(OAuthServerDAL dal) : base(dal)
        {
        }

        public virtual async Task<bool> LoadClientAsync(
            string response_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state)
        {
            _parameters = new BLLResponseTypeParameters
            {
                response_type = response_type,
                client_id = client_id,
                redirect_uri = redirect_uri,
                scope = scope,
                state = state,
            };
            _client = await _dal.GetClientAsync(client_id);
            return _client != null;
        }
    }
}
