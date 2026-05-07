using Microsoft.EntityFrameworkCore;
using System.Timers;
using tunepool.Repository.Interface.playlistInterface;
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

        #region Get
        public async Task<List<PlaylistViewModel>> All(int? lastId, string? metaData, int? platform, int? tags)
        {
            if(lastId == 0)
            {
                lastId = await _context.Playlist.CountAsync() + 1;
            } 

            var playlist = await _context.Playlist
                .AsNoTracking()
                .Where(p => p.Id < lastId && p.Popularity.Any(a => a.rank == 0) &&( metaData == null || p.title.Contains(metaData) || p.description.Contains(metaData)) && ( platform == null || p.platform_id == platform) && ( tags == null || p.PlaylistTags.Any(t => t.tags_id == tags)))
                .OrderByDescending(p => p.Id)
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
            }).Take(10).ToListAsync();
  
            return playlist;
        }

        public async Task<bool> CheckNextPage(int? lastId, string? metaData, int? platform, int? tags)
        {
            var status = await _context.Playlist.AsNoTracking().AnyAsync(p => p.Id < lastId && p.Popularity.Any(a => a.rank == 0) && (metaData == null || p.title.Contains(metaData) || p.description.Contains(metaData)) && (platform == null || p.platform_id == platform) && (tags == null || p.PlaylistTags.Any(t => t.tags_id == tags)));

            return status;
        }

        public async Task<List<PlaylistViewModel>> PlaylistRanking()
        {
            var Playlist = await _context.Playlist
                .AsNoTracking()
                .Where(p => p.Popularity.Any(a => a.rank != 0))
                .OrderBy(p => p.Popularity.Max(a => a.rank))
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
            var tags = await _context.Tags.AsNoTracking().Select(t => new TagsViewModel
            {
                id = t.Id,
                name = t.name
            }).ToListAsync();
            return tags;
        }

        public async Task<List<PlatformViewModel>> GetAllPlatform()
        {
            var platform = await _context.PlatForm.AsNoTracking().Select(p => new PlatformViewModel
            {
                Id = p.Id,
                name = p.name
            }).ToListAsync();
            return platform;
        }
        #endregion

        #region Add
        public async Task Add(PlaylistRequestModel list, string thumbnail, string platform)
        {
            var exisit = await _context.Playlist.Where(p => p.playList_Urls == list.playList_Urls).ToListAsync();

            if (exisit.Count != 0) throw new Exception("Playlist already exist");


            var platformId = _context.PlatForm.FirstOrDefault(p => p.name == platform);
            var tagsList = await _context.Tags.Where(p => list.tags.Contains(p.Id)).ToListAsync();
            var playlist = new Playlist
            {
                title = list.title,
                description = list.description,
                playList_Urls = list.playList_Urls,
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
        #endregion

        public async Task Unlike(int playlistId)
        {
            var total = await _context.Popularity.FindAsync(playlistId);
            total!.likes -= 1;

            await _context.SaveChangesAsync();
        }

        public async Task Unheart(int playlistId)
        {
            var total = await _context.Popularity.FindAsync(playlistId);
            total!.hearts -= 1;

            await _context.SaveChangesAsync();
        }

        public async Task WeeklyRanking(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var resetOldRank = await _context.Popularity.ExecuteUpdateAsync(p => p.SetProperty(x => x.rank, 0), token);

            var updatedrank = await _context.Popularity
                .Where(p => p.hearts != 0 || p.likes != 0)
                .OrderByDescending(p => p.hearts + p.likes)
                .ThenByDescending(p => p.hearts)
                .Take(3)
                .ToListAsync();

            if (updatedrank.Count == 0) return;

            updatedrank.ForEach(p => p.rank = 0);

            for (int iterator = 0; iterator < updatedrank.Count; iterator++)
            {
                token.ThrowIfCancellationRequested();
                updatedrank[iterator].rank = iterator + 1;
            }

            _context.UpdateRange(updatedrank);
            await _context.SaveChangesAsync();
        }
    }
}
