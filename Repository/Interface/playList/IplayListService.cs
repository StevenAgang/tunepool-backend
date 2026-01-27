using tunepool.Repository.ViewModel.playList;

namespace tunepool.Repository.Interface.playList
{
    public interface IplayListService
    {
        Task<playListViewModel> getAll();
    }
}
