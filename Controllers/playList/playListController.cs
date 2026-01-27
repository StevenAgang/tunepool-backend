using Microsoft.AspNetCore.Mvc;

namespace tunepool.Controllers.playList
{
    [ApiController]
    [Route("playList")]
    public class playListController : ControllerBase
    {

        [HttpGet("First")]
        public async Task<IActionResult> getPLayList()
        {
            return StatusCode(200);
        }
    }
}
