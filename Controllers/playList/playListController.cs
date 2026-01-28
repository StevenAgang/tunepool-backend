using Microsoft.AspNetCore.Mvc;
using tunepool.Repository.Configuration.Helper;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Service.Validation.Playlist;

namespace tunepool.Controllers.playList
{
    [ApiController]
    [Route("playList")]
    public class PlaylistController : ControllerBase
    {
        private RequestStatusHelper _requestStatusHelper;
        private IPlaylistService _playListService;
        private PlaylistValidation _playlistValidation;
        private LinkExtractor _linkExtractor;
        public PlaylistController(RequestStatusHelper requestStatusHelper, IPlaylistService iplayListService, PlaylistValidation playlistValidation, LinkExtractor linkExtractor)
        {
            _requestStatusHelper = requestStatusHelper;
            _playListService = iplayListService;
            _playlistValidation = playlistValidation;
            _linkExtractor = linkExtractor;
        }

        [HttpGet("getAllPlaylist")]
        public async Task<IActionResult> GetAllPlaylist()
        {
            try
            {
                var result = await _playListService.All();
                return StatusCode(500, _requestStatusHelper.Success(200, true, null, result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message,null));
            }
        }

        [HttpPost("addPlaylist")]
        public async Task<IActionResult> AddPlaylist(string link,string title,string description,string[] tags)
        {
            try
            {
                _playlistValidation.PlaylistInput(link,title,description,tags);
                string platform =  _linkExtractor.Domain(link);
                string thumbnail = await _linkExtractor.Thumbnails(link,platform);
                await _playListService.Add(link, title, description, tags, thumbnail, platform);
                return StatusCode(200, _requestStatusHelper.Success(200, true, "Playlist Added Successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500,false,ex.Message,null));
            }
        }
    }
}
