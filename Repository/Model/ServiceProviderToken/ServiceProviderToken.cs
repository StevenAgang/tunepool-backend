using System.ComponentModel.DataAnnotations.Schema;
using tunepool.Repository.Model.platform;

namespace tunepool.Repository.Model.serviceProviderToken
{
    public class ServiceProviderToken : BaseModel
    {
        public string? accessToken { get; set; }
        public DateTime? expiresIn { get; set; }
        public string? refreshToken { get; set; }
        [ForeignKey("platform")]
        public int platformId { get; set; }
        public Platform platform { get; set; } = null!;
    }
}
