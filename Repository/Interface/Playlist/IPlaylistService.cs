using tunepool.Repository.ViewModel.playlistViewModel;

namespace tunepool.Repository.Interface.playlistInterface
{
    public interface IPlaylistService
    {
        /// <summary>
        /// Return all Playlist
        /// </summary>
        /// <returns>Returns a set of Playlist</returns>
        Task<List<PlaylistViewModel>> All();
        /// <summary>
        /// Add new Playlist
        /// </summary>
        /// <param name="link">Play list link</param>
        /// <param name="title">Playlist Title</param>
        /// <param name="description">Playlist Description</param>
        /// <param name="tags">Playlist Tags</param>
        /// <param name="thumbnail">Playlist Thumbnail</param>
        /// <param name="platform">Playlist Platform</param>
        /// <returns></returns>
        Task Add(string link, string title, string description, string[] tags, string thumbnail, string platform);
    }
}
