using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthServer.NET.UI.Services;

namespace OAuthServer.NET.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PayloadController : ControllerBase
    {
        protected readonly IDAL _dal;

        public PayloadController(IDAL dal)
        {
            _dal = dal;
            var @test = string.Empty;
        }

        [HttpGet]
        [Route("")]
        public virtual async Task<IActionResult> GetAsync()
        {
            return Ok(await _dal.GetPayloadAsync());
        }
    }
}
