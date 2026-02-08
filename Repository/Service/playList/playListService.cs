using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Model.platform;
using tunepool.Repository.Model.playlist;
using tunepool.Repository.Model.playlistTags;
using tunepool.Repository.Model.popularity;
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

        // Continue: Find a way to properly send to client if the current page is the last page

        #region Get
        public async Task<List<PlaylistViewModel>> All(int lastId)
        {
            bool lastPage = false;

            var playlists = await _context.Playlist
                .Include(p => p.Platform)
                .Include(p => p.Popularity)
                .Include(p => p.PlaylistTags)
                .ThenInclude(pt => pt.Tags)
                .Where(p => p.Popularity.Any(a => a.rank == 0))
                .ToListAsync();

            if (lastId != 0) playlists = playlists.Where(p => p.Id > lastId).ToList();

            var nextPage = playlists.Where(P => P.Id > lastId + 10).ToList();

            if (nextPage.Count == 0) lastPage = true;

            if(playlists.Count == 0) throw new Exception("No more data to show");


            var list = playlists.Take(10).Select(p => new PlaylistViewModel
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
                    hearts = pop.hearts,
                    rank = pop.rank
                }).ToList(),
                Platform = new PlatformViewModel { Id = p.Id, name = p.Platform!.name },
            }).ToList();
  
            return list;
        }

        public async Task<List<PlaylistViewModel>> PlaylistRanking()
        {
            var Playlist = await _context.Playlist
                .Include(p => p.Platform)
                .Include(p => p.Popularity)
                .Include(p => p.PlaylistTags)
                .ThenInclude(pt => pt.Tags)
                .Where(p => p.Popularity.Any(a => a.rank != 0))
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
                        hearts = pop.hearts,
                        rank = pop.rank
                    }).ToList(),
                    Platform = new PlatformViewModel { Id = p.Id, name = p.Platform!.name },
                }).Take(3).ToListAsync();

            return Playlist;
        }

        public async Task<List<TagsViewModel>> GetAllTags()
        {
            var tags = await _context.Tags.Select(t => new TagsViewModel
            {
                id = t.Id,
                name = t.name
            }).ToListAsync();
            return tags;
        }
        #endregion

        #region Add
        public async Task Add(string link, string title, string description, string[] tags, string thumbnail, string platform)
        {
            var exisit = _context.Playlist.Where(p => p.playList_Urls == link).ToList();

            if (exisit.Count != 0) throw new Exception("Playlist already exist");


            var platformId = _context.PlatForm.FirstOrDefault(p => p.name == platform);
            var tagsList = _context.Tags.Where(p => tags.Contains(p.name)).ToList();
            var playlist = new Playlist
            {
                title = title,
                description = description,
                playList_Urls = link,
                platform_id = platformId!.Id,
                thumbnail = thumbnail,
                createdAt = DateTime.UtcNow
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

            var popularity = new Popularity
            {
                playListId = playlist.Id,
                likes = 0,
                hearts = 0,
                rank = 0
            };

            _context.Add(popularity);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Update
        public async Task Like(PopularityViewModel playlist)
        {
            var total = await _context.Popularity.FindAsync(playlist.playlist_id);
            total!.likes += 1;

            await _context.SaveChangesAsync();
        }

        public async Task Hearts(PopularityViewModel playlist)
        {
            var total = await _context.Popularity.FindAsync(playlist.playlist_id);

            total!.hearts += 1;

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
