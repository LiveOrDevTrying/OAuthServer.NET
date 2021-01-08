using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OAuthServer.NET.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public virtual IActionResult Index()
        {
            return View();
        }
        
        public virtual IActionResult Privacy()
        {
            return View();
        }
    }
}
