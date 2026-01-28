using Microsoft.EntityFrameworkCore;
using tunepool.Repository;
using tunepool.Repository.Configuration.Helper;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Interface.popularityInterface;
using tunepool.Repository.Service.PlaylistService;
using tunepool.Repository.Service.popularityService;
using tunepool.Repository.Service.Validation.Playlist;

namespace tunepool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            //Scoped Service
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IPopularityService, PopularityService>();

            //Singleton Service
            builder.Services.AddSingleton<RequestStatusHelper >();
            builder.Services.AddSingleton<PlaylistValidation>();

            DotNetEnv.Env.TraversePath().Load();
            var connection = Environment.GetEnvironmentVariable("CONNECTIONSTRING");

            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            builder.Services.AddHttpClient<LinkExtractor>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
