using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("Clients/PostLogoutRedirectURIs")]
    [ApiController]
    [Authorize]
    public class ClientsPostLogoutRedirectURIsController : ControllerBase
    {
        protected readonly IDAL _dal;

        public ClientsPostLogoutRedirectURIsController(IDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> CreateAsync(ClientPostLogoutRedirectURICreateRequest request)
        {
            var dto = await _dal.CreateClientPostLogoutRedirectURIAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ClientPostLogoutRedirectURIUpdateRequest request)
        {
            if (id == request.Id)
            {
                var dto = await _dal.UpdateClientPostLogoutRedirectURIAsync(request);
                if (dto != null)
                {
                    return Ok(dto);
                }
            }
            return BadRequest(request);
        }


        [HttpDelete("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (await _dal.DeleteClientPostLogoutRedirectURIAsync(id))
            {
                return NoContent();
            }

            return NotFound(id);
        }
    }
}
