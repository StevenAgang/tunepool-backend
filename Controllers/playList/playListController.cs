using Microsoft.AspNetCore.Mvc;
using Sprache;
using tunepool.Repository.Configuration.Helper;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Service.Validation.Playlist;
using tunepool.Repository.ViewModel.playlistViewModel;
using tunepool.Repository.ViewModel.popularityViewModel;

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
        public PlaylistController(
            RequestStatusHelper requestStatusHelper, 
            IPlaylistService iplayListService, 
            PlaylistValidation playlistValidation, 
            LinkExtractor linkExtractor)
        {
            _requestStatusHelper = requestStatusHelper;
            _playListService = iplayListService;
            _playlistValidation = playlistValidation;
            _linkExtractor = linkExtractor;
        }

        [HttpGet("GetAllPlaylist")]
        public async Task<IActionResult> GetAllPlaylist(int lastId)
        {
            try
            {
                var result = await _playListService.All();

                var pages = _playListService.SlicePage(result,lastId);

                var lastPageStatus = _playListService.CheckLastPage(result, lastId);

                return StatusCode(200, _requestStatusHelper.Success(200, true, null, pages, lastPageStatus));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message,null, null));
            }
        }

        [HttpGet("GetRanking")]
        public async Task<IActionResult> GetRanking()
        {
            try
            {
                var result = await _playListService.PlaylistRanking();
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, result, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpGet("GetAllTags")]
        public async Task<IActionResult> GetAllTags()
        {
            try
            {
                var result = await _playListService.GetAllTags();
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, result, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpPost("AddPlaylist")]
        public async Task<IActionResult> AddPlaylist(string link,string title,string description,string[] tags)
        {
            try
            {
                _playlistValidation.PlaylistInput(link,title,description,tags);
                string platform =  _linkExtractor.Domain(link);
                string thumbnail = await _linkExtractor.Thumbnails(link,platform);
                await _playListService.Add(link, title, description, tags, thumbnail, platform);
                return StatusCode(200, _requestStatusHelper.Success(200, true, "Playlist Added Successfully", null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500,false,ex.Message,null, null));
            }
        }

        [HttpPut("LikePlaylist")]
        public async Task<IActionResult> LikePlaylist([FromBody] PopularityViewModel playlist)
        {
            try
            {
                await _playListService.Like(playlist);
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpPut("HeartPlaylist")]
        public async Task<IActionResult> HeartsPlaylist([FromBody] PopularityViewModel playlist)
        {
            try
            {
                await 
                    _playListService.Hearts(playlist);
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

    }
}
