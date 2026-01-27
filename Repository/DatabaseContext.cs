using Microsoft.EntityFrameworkCore;
using tunepool.Repository.Model.platform;
using tunepool.Repository.Model.playList;
using tunepool.Repository.Model.PlaylistTags;
using tunepool.Repository.Model.Popularity;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Popularity>()
                .HasNoKey();
            modelBuilder.Entity<PlaylistTags>()
                .HasNoKey();
        }
    }
}
