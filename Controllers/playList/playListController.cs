using Microsoft.AspNetCore.Mvc;

namespace tunepool.Controllers.playList
{
    public class playListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
