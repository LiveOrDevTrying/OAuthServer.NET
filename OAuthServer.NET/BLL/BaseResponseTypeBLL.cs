using OAuthServer.NET.Core.Models.Exceptions;
using OAuthServer.NET.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthServer.NET.BLL
{
    public abstract class BaseResponseTypeBLL<T> : BaseBLL<T>, IBaseResponseTypeBLL where T : BLLResponseTypeParameters
    {
        public BaseResponseTypeBLL(OAuthServerDAL dal) : base(dal)
        {
        }

        public virtual async Task<bool> IsAuthorizeResponseTypeValidAsync()
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in AuthorizeBLL before validations");
            }

            var grants = await _dal.GetGrantsAsync();

            var splitResponseCode = _parameters.response_type.Split(" ");

            if (splitResponseCode.Length == 0 ||
                !splitResponseCode.Any(x => grants.Any(t => t.AuthorizeResponseType == x)))
            {
                return false;
            }

            return true;
        }
        public virtual async Task<bool> IsGrantValidAsync()
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in AuthorizeBLL before validations");
            }

            var grants = await _dal.GetGrantsAsync();

            var grant = grants.FirstOrDefault(x => x.AuthorizeResponseType == _parameters.response_type);

            if (grant == null || _client.GrantId != grant.Id)
            {
                return false;
            }

            return true;
        }
        public virtual async Task<bool> IsScopeValidAsync()
        {
            if (_parameters == null || _client == null)
            {
                throw new AppException("Call LoadClientAsync() in AuthorizeBLL before validations");
            }

            var scopesSplit = _parameters.scope.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var scopes = await _dal.GetClientScopesAsync(_client.Id);

            foreach (var scopeSplit in scopesSplit)
            {
                if (!scopes.Any(x => x.ScopeName.Trim().ToLower() == scopeSplit.Trim().ToLower()))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class BLLResponseTypeParameters : BLLParameters
    {
        public string response_type { get; set; }
        public string state { get; set; }
        public string scope { get; set; }
    }
}
