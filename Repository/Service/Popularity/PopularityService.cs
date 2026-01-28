using tunepool.Repository.Interface.popularityInterface;
using tunepool.Repository.Model.popularity;

namespace tunepool.Repository.Service.popularityService
{
    public class PopularityService : IPopularityService
    {
        private DatabaseContext _context;
        public PopularityService(DatabaseContext context) 
        {
            _context = context;
        }
        public async Task Like(int playlistId)
        {
            var total = await _context.Popularity.FindAsync(playlistId);
            total!.likes += 1;

            await _context.SaveChangesAsync();
        }

        public async Task Hearts(int playlistId)
        {
            var total = await _context.Popularity.FindAsync(playlistId);

            total!.hearts += 1;

            await _context.SaveChangesAsync();
        }
    }
}
