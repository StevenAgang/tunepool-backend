using Microsoft.AspNetCore.Mvc;

namespace tunepool.Controllers.KeepAlive
{
    public class KeepAliveController : Controller
    {
        [HttpGet("KeepAlive")]
        public IActionResult Index()
        {
            var response = StatusCode(200);
            return response;
        }
    }
}
