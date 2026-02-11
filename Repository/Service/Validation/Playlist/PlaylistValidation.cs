using tunepool.Repository.ViewModel.playlistViewModel;

namespace tunepool.Repository.Service.Validation.Playlist
{
    public class PlaylistValidation
    {
        public void PlaylistInput(PlaylistRequestModel playlist)
        {
            if (string.IsNullOrEmpty(playlist.playList_Urls)) throw new Exception("link is required");
            if (string.IsNullOrEmpty(playlist.title)) throw new Exception("title is required");
            if (string.IsNullOrEmpty(playlist.description)) throw new Exception("description is required");
            if (playlist.tags == null || playlist.tags.Length == 0) throw new Exception("tags is requried");
        }
    }
}
