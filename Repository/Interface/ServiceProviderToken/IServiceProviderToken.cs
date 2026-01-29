using tunepool.Repository.Model.serviceProviderToken;

namespace tunepool.Repository.Interface.serviceProviderTokenInterface
{
    public interface IServiceProviderToken
    {
        /// <summary>
        ///     Get all data regarding Token
        /// </summary>
        /// <param name="platform"></param>
        Task<List<ServiceProviderToken>> GetAccessToken(string platform);
        /// <summary>
        ///     Add SoundCloud Token
        /// </summary>
        /// <param name="response"></param>
        /// <param name="platform"></param>
        Task<ServiceProviderToken> AddSoundCloudAccessToken(HttpResponseMessage response, string platform);
        /// <summary>
        ///     Updating SoundCloud Token
        /// </summary>
        /// <param name="response"></param>
        /// <param name="accessToken"></param>
        Task<ServiceProviderToken> RefreshSoundCloudAccessToken(HttpResponseMessage response, ServiceProviderToken accessToken);
        /// <summary>
        ///     Add Tidal Token
        /// </summary>
        /// <param name="response"></param>
        /// <param name="platform"></param>
        Task<ServiceProviderToken> AddTidalAccessToken(HttpResponseMessage response, string platform);
        /// <summary>
        ///     Updating Tidal Token
        /// </summary>
        /// <param name="response"></param>
        /// <param name="accessToken"></param>
        Task<ServiceProviderToken> RefreshTidalAccessToken(HttpResponseMessage response, ServiceProviderToken accessToken);



    }
}
