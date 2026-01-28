using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Model.platform;
using tunepool.Repository.Model.playlist;
using tunepool.Repository.Model.playlistTags;
using tunepool.Repository.ViewModel.platformViewModel;
using tunepool.Repository.ViewModel.playlistViewModel;
using tunepool.Repository.ViewModel.popularityViewModel;
using tunepool.Repository.ViewModel.tagsViewModel;

namespace tunepool.Repository.Service.PlaylistService
{
    public class PlaylistService : IPlaylistService
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

        public async Task Add(string link, string title, string description, string[] tags, string thumbnail, string platform)
        {
            var platformId = _context.PlatForm.FirstOrDefault(p => p.name == platform);
            var tagsList = _context.Tags.Where(p => tags.Contains(p.name)).ToList();
            var playlist = new Playlist
            {
                title = title,
                description = description,
                playList_Urls = link,
                platform_id = platformId!.Id,
                thumbnail = thumbnail,
                createdAt = DateTime.Now
            };

            _context.Add(playlist);
            await _context.SaveChangesAsync();

            var playlistTags = tagsList.Select(t => new PlaylistTags
            {
                playlist_id = playlist.Id,
                tags_id = t.Id,
            });

            _context.AddRange(playlistTags);
            await _context.SaveChangesAsync();
        }
    }

}
