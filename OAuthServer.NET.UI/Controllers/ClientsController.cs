using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Models.Requests;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        protected readonly IDAL _dal;

        public ClientsController(IDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("implicit")]
        public virtual async Task<IActionResult> CreateImplicitAsync(ClientImplicitCreateRequest request)
        {
            var dto = await _dal.CreateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("implicit/{id}")]
        public virtual async Task<IActionResult> UpdateImplicitAsync(Guid id, [FromBody] ClientImplicitUpdateRequest request)
        {
            var dto = await _dal.UpdateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPost]
        [Route("authorizationcode")]
        public virtual async Task<IActionResult> CreateAuthorizationCodeAsync(ClientAuthorizationCodeCreateRequest request)
        {
            var dto = await _dal.CreateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("authorizationcode/{id}")]
        public virtual async Task<IActionResult> UpdateAuthorizationCodeAsync(Guid id, [FromBody] ClientAuthorizationCodeUpdateRequest request)
        {
            var dto = await _dal.UpdateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPost]
        [Route("clientcredentials")]
        public virtual async Task<IActionResult> CreateClientCredentialsAsync(ClientClientCredentialsCreateRequest request)
        {
            var dto = await _dal.CreateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("clientcredentials/{id}")]
        public virtual async Task<IActionResult> UpdateClientCredentialsAsync(Guid id, [FromBody] ClientClientCredentialsUpdateRequest request)
        {
            var dto = await _dal.UpdateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPost]
        [Route("ropassword")]
        public virtual async Task<IActionResult> CreateROPasswordAsync(ClientResourceOwnerPasswordCreateRequest request)
        {
            var dto = await _dal.CreateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpPut("{id}")]
        [Route("ropassword/{id}")]
        public virtual async Task<IActionResult> UpdateROPasswordAsync(Guid id, [FromBody] ClientResourceOwnerPasswordUpdateRequest request)
        {
            var dto = await _dal.UpdateClientAsync(request);
            if (dto != null)
            {
                return Ok(dto);
            }

            return BadRequest(request);
        }

        [HttpDelete("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (await _dal.DeleteClientAsync(id))
            {
                return NoContent();
            }

            return NotFound(id);
        }
    }
}
