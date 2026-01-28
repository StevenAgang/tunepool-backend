using tunepool.Repository.ViewModel.playList;

namespace tunepool.Repository.Interface.playList
{
    public interface IplayListService
    {
        /// <summary>
        /// Return all Playlist
        /// </summary>
        /// <returns>Returns a set of Playlist</returns>
        Task<List<PlaylistViewModel>> All();
    }
}
