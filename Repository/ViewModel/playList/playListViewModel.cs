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
        public ICollection<TagsViewModel> Tags { get; set; } = new List<TagsViewModel>();
        public ICollection<PopularityViewModel> Popularity { get; set; } = new List<PopularityViewModel>();
        public PlatformViewModel Platform { get; set; } = new PlatformViewModel();
    }

    public class PlaylistRequestModel
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? playList_Urls { get; set; }
        public int[]? tags { get; set; }
    }
}
