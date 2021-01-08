using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("Clients/CORSOrigins")]
    [ApiController]
    [Authorize]
    public class ClientsCORSOriginsController : ControllerBase
    {
        protected readonly IDAL _dal;

        public ClientsCORSOriginsController(IDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> CreateAsync(ClientCORSOriginCreateRequest request)
        {
            var dto = await _dal.CreateClientCORSOriginAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ClientCORSOriginUpdateRequest request)
        {
            if (id == request.Id)
            {
                var dto = await _dal.UpdateClientCORSOriginAsync(request);
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
            if (await _dal.DeleteClientCORSOriginAsync(id))
            {
                return NoContent();
            }

            return NotFound(id);
        }
    }
}
