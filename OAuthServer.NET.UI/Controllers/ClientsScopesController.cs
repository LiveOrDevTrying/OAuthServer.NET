using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("Clients/Scopes")]
    [ApiController]
    [Authorize]
    public class ClientsScopesController : ControllerBase
    {
        protected readonly IDAL _dal;

        public ClientsScopesController(IDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> CreateAsync(ClientScopeCreateRequest request)
        {
            var dto = await _dal.CreateClientScopeAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ClientScopeUpdateRequest request)
        {
            if (id == request.Id)
            {
                var dto = await _dal.UpdateClientScopeAsync(request);
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
            if (await _dal.DeleteClientScopeAsync(id))
            {
                return NoContent();
            }

            return NotFound(id);
        }
    }
}
