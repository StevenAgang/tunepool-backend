using System.ComponentModel.DataAnnotations.Schema;
using tunepool.Repository.Model.platform;

namespace tunepool.Repository.Model.playList
{
    public class Playlist : BaseModel
    {
        public string? title {  get; set; }
        public string? description { get; set; }
        public string? playList_Urls { get; set; }

        [ForeignKey("Platform")]
        public int platform_id { get; set; }
        
        public Platform? Platform { get; set; }

        public string? thumbnail { get; set; }
    }
}
