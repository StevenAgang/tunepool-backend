using System.Timers;
using tunepool.Repository.ViewModel.platformViewModel;
using tunepool.Repository.ViewModel.playlistViewModel;
using tunepool.Repository.ViewModel.popularityViewModel;
using tunepool.Repository.ViewModel.tagsViewModel;

namespace tunepool.Repository.Interface.playlistInterface
{
    public interface IPlaylistService
    {
        /// <summary>
        /// Return all Playlist
        /// </summary>
        /// <returns>Returns a set of Playlist</returns>
        Task<List<PlaylistViewModel>> All(int? lastId, string? metaData, int? platform, int? tags);

        Task<bool> CheckNextPage(int? lastId, string? metaData, int? platform, int? tags);

        //bool CheckLastPage(List<PlaylistViewModel> playlist, int lastId);

        Task<List<PlaylistViewModel>> PlaylistRanking();

        /// <summary>
        ///  Return all available mood tags
        /// </summary>
        /// <returns></returns>
        Task<List<TagsViewModel>> GetAllTags();
        /// <summary>
        ///    Return all available platform
        /// </summary>
        /// <returns></returns>
        Task<List<PlatformViewModel>> GetAllPlatform();

        /// <summary>
        /// Add new Playlist
        /// </summary>
        /// <param name="playlist">Play list DTO</param>
        /// <param name="thumbnail">Playlist Thumbnail</param>
        /// <param name="platform">Playlist Platform</param>
        /// <returns></returns>
        Task Add(PlaylistRequestModel playlist, string thumbnail, string platform);

        /// <summary>
        ///     Add like to Playlist
        /// </summary>
        Task Like(int playlistId);
        /// <summary>
        ///     Add hearts to Playlist
        /// </summary>
        Task Hearts(int playlistId);

        /// <summary>
        ///     Unlike a playlist
        /// </summary>
        /// <param name="playlist"></param>

        Task Unlike(int playlistId);

        /// <summary>
        ///     Unheart a playlist
        /// </summary>
        /// <param name="playlist"></param>

        Task Unheart(int playlistId);

        Task WeeklyRanking(CancellationToken token);
    }
}
