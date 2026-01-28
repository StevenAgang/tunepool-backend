namespace tunepool.Repository.Interface.Popularity
{
    public interface IPopularityService
    {
        /// <summary>
        ///     Add like to Playlist
        /// </summary>
        Task Like();
        /// <summary>
        ///     Add hearts to Playlist
        /// </summary>
        Task Hearts();
    }
}
