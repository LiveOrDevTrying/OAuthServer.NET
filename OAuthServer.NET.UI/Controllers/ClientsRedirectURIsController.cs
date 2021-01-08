using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("Clients/RedirectURIs")]
    [ApiController]
    [Authorize]
    public class ClientsRedirectURIsController : ControllerBase
    {
        protected readonly IDAL _dal;

        public ClientsRedirectURIsController(IDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> CreateAsync(ClientRedirectURICreateRequest request)
        {
            var dto = await _dal.CreateClientRedirectURIAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ClientRedirectURIUpdateRequest request)
        {
            if (id == request.Id)
            {
                var dto = await _dal.UpdateClientRedirectURIAsync(request);
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
            if (await _dal.DeleteClientRedirectURIAsync(id))
            {
                return NoContent();
            }

            return NotFound(id);
        }
    }
}
