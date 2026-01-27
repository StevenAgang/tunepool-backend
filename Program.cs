using Microsoft.EntityFrameworkCore;
using tunepool.Repository;

namespace tunepool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            DotNetEnv.Env.TraversePath().Load();
            var connection = Environment.GetEnvironmentVariable("CONNECTIONSTRING");

            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            builder.Services.AddHttpClient();
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
