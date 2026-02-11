using Microsoft.EntityFrameworkCore;
using tunepool.Repository;
using tunepool.Repository.Configuration.Helper;
using tunepool.Repository.Interface.playlistInterface;
using tunepool.Repository.Interface.serviceProviderTokenInterface;
using tunepool.Repository.Service.PlaylistService;
using tunepool.Repository.Service.serviceProviderTokenService;
using tunepool.Repository.Service.Validation.Playlist;

namespace tunepool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontEnd", policy =>
                {
                    policy.WithOrigins("https://localhost:4200", "http://localhost:4200", "http://192.168.31.190:4200", "https://192.168.31.190:4200", "http://172.28.144.1:4200/", "https://172.28.144.1:4200/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            // Add services to the container.

            //Scoped Service
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IServiceProviderToken, ServiceProviderTokenService>();

            //Singleton Service
            builder.Services.AddSingleton<RequestStatusHelper >();
            builder.Services.AddSingleton<PlaylistValidation>();
            builder.Services.AddHostedService<Polling>();

            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHttpClient<LinkExtractor>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors("FrontEnd");
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
