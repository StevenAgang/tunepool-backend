using Microsoft.EntityFrameworkCore;
using tunepool.Repository.Model.platform;
using tunepool.Repository.Model.playlist;
using tunepool.Repository.Model.playlistTags;
using tunepool.Repository.Model.popularity;
using tunepool.Repository.Model.serviceProviderToken;
using tunepool.Repository.Model.tags;

namespace tunepool.Repository
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option): base(option){}

        public DbSet<Playlist> Playlist {  get; set; }
        public DbSet<Platform> PlatForm { get; set; }
        public DbSet<Popularity> Popularity { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<PlaylistTags> PlaylistTags { get; set; }
        public DbSet<ServiceProviderToken> ServiceProviderToken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Popularity>()
                .HasKey(p => new {p.playListId});

            modelBuilder.Entity<Popularity>()
                .HasOne(p => p.Playlist)
                .WithMany(p => p.Popularity)
                .HasForeignKey(p => p.playListId);

            modelBuilder.Entity<PlaylistTags>()
                .HasKey(pt => new { pt.playlist_id, pt.tags_id });

            modelBuilder.Entity<PlaylistTags>()
                .HasOne(pt => pt.Playlist)
                .WithMany(p => p.PlaylistTags)
                .HasForeignKey(pt => pt.playlist_id);

            modelBuilder.Entity<PlaylistTags>()
                .HasOne(pt => pt.Tags)
                .WithMany(p => p.PlaylistTags)
                .HasForeignKey(pt => pt.tags_id);

            modelBuilder.Entity<ServiceProviderToken>()
                .HasOne(p => p.platform)
                .WithMany()
                .HasForeignKey(p => p.platformId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
