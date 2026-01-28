using tunepool.Repository.ViewModel.platformViewModel;
using tunepool.Repository.ViewModel.popularityViewModel;
using tunepool.Repository.ViewModel.tagsViewModel;

namespace tunepool.Repository.ViewModel.playlistViewModel
{
    public class PlaylistViewModel
    {
        public int id {  get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? playList_Urls { get; set; }
        public string? thumbnail { get; set; }
        public ICollection<TagsViewModel>? Tags { get; set; }
        public ICollection<PopularityViewModel>? Popularity { get; set; }
        public PlatformViewModel? Platform { get; set; }
    }
}
