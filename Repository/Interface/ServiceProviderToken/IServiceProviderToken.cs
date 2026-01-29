using tunepool.Repository.Model.serviceProviderToken;

namespace tunepool.Repository.Interface.serviceProviderTokenInterface
{
    public interface IServiceProviderToken
    {
        /// <summary>
        ///     Get all data regarding SoundCloud Token
        /// </summary>
        Task<List<ServiceProviderToken>> GetSoundCloudAccessToken();
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
    }
}
