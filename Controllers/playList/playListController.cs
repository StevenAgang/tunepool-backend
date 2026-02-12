using Microsoft.AspNetCore.Mvc;
using tunepool.Repository.Configuration.AttributeExtender;
using tunepool.Repository.Configuration.Helper;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Service.Validation.Playlist;
using tunepool.Repository.ViewModel.playlistViewModel;

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
        [Control]
        public async Task<IActionResult> GetAllPlaylist(int? lastId, string? metaData, int? platform, int? tags)
        {
            try
            {
                var result = await _playListService.All(lastId, metaData, platform, tags);

                var lastItem = result.LastOrDefault();
                if(lastItem == null)
                {
                    lastId = 0;
                }
                else
                {
                    lastId = lastItem.id;
                }
                bool lastPageStatus = await _playListService.CheckNextPage(lastId, metaData, platform, tags);

                return StatusCode(200, _requestStatusHelper.Success(200, true, null, result, lastPageStatus));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message,null, null));
            }
        }

        [HttpGet("GetRanking")]
        [Control]
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
        [Control]
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

        [HttpGet("GetAllPlatform")]
        [Control]
        public async Task<IActionResult> SupportedPlatform()
        {
            try
            {
                var result = await _playListService.GetAllPlatform();
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, result, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpPost("AddPlaylist")]
        [Control]
        public async Task<IActionResult> AddPlaylist([FromBody] PlaylistRequestModel playlist)
        {
            try
            {
                _playlistValidation.PlaylistInput(playlist);
                string platform =  _linkExtractor.Domain(playlist.playList_Urls);
                string thumbnail = await _linkExtractor.Thumbnails(playlist.playList_Urls,platform);
                await _playListService.Add(playlist, thumbnail, platform);
                return StatusCode(200, _requestStatusHelper.Success(200, true, "Playlist Added Successfully", null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500,false,ex.Message,null, null));
            }
        }

        [HttpPatch("LikePlaylist")]
        [Control]
        public async Task<IActionResult> LikePlaylist([FromQuery] int playlistId)
        {
            try
            {
                await _playListService.Like(playlistId);
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpPatch("HeartPlaylist")]
        [Control]
        public async Task<IActionResult> HeartsPlaylist([FromQuery] int playlistId)
        {
            try
            {
                await 
                    _playListService.Hearts(playlistId);
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpPatch("UnlikePlaylist")]
        [Control]
        public async Task<IActionResult> UnlikePlaylist([FromQuery] int playlistId)
        {
            try
            {
                await _playListService.Unlike(playlistId);
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }

        [HttpPatch("UnheartPlaylist")]
        [Control]
        public async Task<IActionResult> UnheartPlaylist([FromQuery] int playlistId)
        {
            try
            {
                await _playListService.Unheart(playlistId);
                return StatusCode(200, _requestStatusHelper.Success(200, true, null, null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _requestStatusHelper.Success(500, false, ex.Message, null, null));
            }
        }
    }
}
