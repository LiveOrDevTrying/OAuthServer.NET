using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Models.DTOs;
using OAuthServer.NET.UI.Models;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GrantsController : ControllerBase
    {
        protected readonly IDAL _dal;

        public GrantsController(IDAL dal)
        {
            _dal = dal;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> CreateAsync(GrantDTO grant)
        {
            var dto = await _dal.CreateGrantAsync(grant);
            if (dto != null)
            {
                return CreatedAtAction("CreateGrant", dto);
            }

            return BadRequest(grant);
        }

        [HttpPut("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody]GrantDTO grant)
        {
            if (id == grant.Id &&
                await _dal.UpdateGrantAsync(grant))
            {
                return NoContent();
            }

            return BadRequest(grant);
        }

        [HttpDelete("{id}")]
        [Route("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (await _dal.DeleteGrantAsync(id))
            {
                return NoContent();
            }

            return NotFound(id);
        }
    }
}
