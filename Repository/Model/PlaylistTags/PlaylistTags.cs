using System.ComponentModel.DataAnnotations.Schema;
using tunepool.Repository.Model.playlist;
using tunepool.Repository.Model.tags;

namespace tunepool.Repository.Model.playlistTags
{
    public class PlaylistTags
    {
        [ForeignKey("Playlist")]
        public int playlist_id { get; set; }
        public Playlist Playlist { get; set; } = null!;
        [ForeignKey("Tags")]
        public int tags_id { get; set; }
        public Tags Tags { get; set; } = null!;
    }
}
