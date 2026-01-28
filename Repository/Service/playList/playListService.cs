using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using tunepool.Repository.Interface.playList;
using tunepool.Repository.Model.platform;
using tunepool.Repository.ViewModel.Platform;
using tunepool.Repository.ViewModel.playList;
using tunepool.Repository.ViewModel.Popularity;
using tunepool.Repository.ViewModel.Tags;

namespace tunepool.Repository.Service.playList
{
    public class PlaylistService : IplayListService
    {
        private readonly DatabaseContext _context;
        public PlaylistService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<PlaylistViewModel>> All()
        {
            var Playlist = await _context.Playlist
                .Include(p => p.Platform)
                .Include(p => p.Popularity)
                .Include(p => p.PlaylistTags)
                .ThenInclude(pt => pt.Tags)
                .Select(p => new PlaylistViewModel
                {
                    id = p.Id,
                    title = p.title,
                    description = p.description,
                    playList_Urls = p.playList_Urls,
                    thumbnail = p.thumbnail,
                    Tags = p.PlaylistTags!.Select(t => new TagsViewModel
                    {
                        id = t.tags_id,
                        name = t.Tags!.name
                    }).ToList(),
                    Popularity = p.Popularity!.Select(pop => new PopularityViewModel
                    {
                        playlist_id = p.Id,
                        likes = pop.likes,
                        hearts = pop.hearts
                    }).ToList(),
                    Platform = new PlatformViewModel { Id = p.Id, name = p.Platform!.name},
                }).ToListAsync();

            return Playlist;
        }
    }

}
