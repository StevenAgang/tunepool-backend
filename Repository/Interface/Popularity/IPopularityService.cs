namespace tunepool.Repository.Interface.popularityInterface
{
    public interface IPopularityService
    {
        /// <summary>
        ///     Add like to Playlist
        /// </summary>
        Task Like(int playlistId);
        /// <summary>
        ///     Add hearts to Playlist
        /// </summary>
        Task Hearts(int playlistId);
    }
}
