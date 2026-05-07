using tunepool.Repository.Interface.playlistInterface;

namespace tunepool.Repository.Configuration.Helper
{
    public class Polling(IServiceScopeFactory _scopeFactory, ILogger<Polling> _logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                try
                {
                    await RunWeekly(stoppingToken);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation($"Error in Background Service: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }

                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async _ => await RunWeekly(cancellationToken),  null, TimeSpan.FromMinutes(5), TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public async Task RunWeekly(CancellationToken token) {
            if (token.IsCancellationRequested) return;
            using var scope = _scopeFactory.CreateScope();
            var playlistService = scope.ServiceProvider.GetRequiredService<IPlaylistService>();
            await playlistService.WeeklyRanking(token);
        }
    }
}
 
