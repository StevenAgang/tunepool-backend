using tunepool.Repository.ViewModel.Platform;
using tunepool.Repository.ViewModel.Popularity;
using tunepool.Repository.ViewModel.Tags;

namespace tunepool.Repository.ViewModel.playList
{
    public class playListViewModel
    {
        public int id {  get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? playList_Urls { get; set; }
        public string? thumbnail { get; set; }
        public ICollection<tagsViewModel>? Tags { get; set; }
        public ICollection<PopularityViewModel>? Popularity { get; set; }
        public PlatformViewModel? Platform { get; set; }


    }
}
