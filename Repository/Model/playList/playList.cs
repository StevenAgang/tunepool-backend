using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using tunepool.Repository.Model.platform;
using tunepool.Repository.Model.playlistTags;
using tunepool.Repository.Model.popularity;

namespace tunepool.Repository.Model.playlist
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

        //Navigation Prop
        public ICollection<Popularity> Popularity { get; set; } = new List<Popularity>();
        public ICollection<PlaylistTags> PlaylistTags { get; set; } = new List<PlaylistTags>();
    }
}
