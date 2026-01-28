using Microsoft.AspNetCore.Mvc;
using tunepool.Repository.Configuration.Helper;
using tunepool.Repository.Interface.playList;

namespace tunepool.Controllers.playList
{
    [ApiController]
    [Route("playList")]
    public class playListController : ControllerBase
    {
        private RequestStatusHelper _requestStatusHelper;
        private IplayListService _playListService;
        public playListController(RequestStatusHelper requestStatusHelper, IplayListService iplayListService)
        {
            _requestStatusHelper = requestStatusHelper;
            _playListService = iplayListService;
        }

        [HttpGet("First")]
        public async Task<IActionResult> getAllPlayList()
        {
            try
            {
                var result = await _playListService.All();
                return StatusCode(500, _requestStatusHelper.Success(500, false, null, result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message,null));
            }
        }
    }
}
