using Microsoft.AspNetCore.Mvc;

namespace OAuthServer.NET.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string IpAddress
        {
            get
            {
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    return Request.Headers["X-Forwarded-For"];
                }
                else
                {
                    return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
            }
        }
    }
}
