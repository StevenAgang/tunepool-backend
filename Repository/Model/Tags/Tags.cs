using tunepool.Repository.Model.playlistTags;

namespace tunepool.Repository.Model.tags
{
    public class Tags : BaseModel
    {
        public string? name {  get; set; }
        public ICollection<PlaylistTags> PlaylistTags { get; set; } = new List<PlaylistTags>();
    }
}
