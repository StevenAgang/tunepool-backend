using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using tunepool.Repository.Interface.serviceProviderTokenInterface;
using tunepool.Repository.Model.serviceProviderToken;

namespace tunepool.Repository.Service.serviceProviderTokenService
{
    public class ServiceProviderTokenService :IServiceProviderToken
    {
        private DatabaseContext _context;
        public ServiceProviderTokenService(DatabaseContext context) 
        {
            _context = context;
        }

        public async Task<List<ServiceProviderToken>> GetSoundCloudAccessToken()
        {
            return await _context.ServiceProviderToken.ToListAsync();
        }

        public async Task<ServiceProviderToken> AddSoundCloudAccessToken(HttpResponseMessage response, string platform)
        {
            var platformId = _context.PlatForm.Where(p => p.name == platform).Select(p => p.Id).FirstOrDefault();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            int expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();
            var totalExpiration = DateTime.UtcNow.AddSeconds(expiresIn);
            var accessToken = new ServiceProviderToken
            {
                accessToken = doc.RootElement.GetProperty("access_token").GetString(),
                refreshToken = doc.RootElement.GetProperty("refresh_token").GetString(),
                expiresIn = totalExpiration,
                platformId = platformId,
                createdAt = DateTime.UtcNow
            };

            _context.Add(accessToken);

            await _context.SaveChangesAsync();

            return accessToken;
        }

        public async Task<ServiceProviderToken> RefreshSoundCloudAccessToken(HttpResponseMessage response, ServiceProviderToken accessToken) {

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            accessToken.accessToken = doc.RootElement.GetProperty("access_token").GetString();
            accessToken.refreshToken = doc.RootElement.GetProperty("refresh_token").GetString();
            int expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();
            var totalExpiration = DateTime.UtcNow.AddSeconds(expiresIn);

            await _context.SaveChangesAsync();

            return accessToken;
        }
    }
}
