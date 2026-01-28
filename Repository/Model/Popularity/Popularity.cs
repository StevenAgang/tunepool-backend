using System.ComponentModel.DataAnnotations.Schema;
using tunepool.Repository.Model.playlist;

namespace tunepool.Repository.Model.popularity
{
    public class Popularity
    {
        [ForeignKey("Playlist")]
        public int playListId { get; set; }
        public Playlist? Playlist { get; set; }
        public int likes { get; set; }
        public int hearts { get; set; }
    }
}
