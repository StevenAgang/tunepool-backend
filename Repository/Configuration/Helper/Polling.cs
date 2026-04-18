using tunepool.Repository.Interface.playlistInterface;

namespace tunepool.Repository.Configuration.Helper
{
    public class Polling : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public Polling(IServiceScopeFactory scopeFactory) {
            _scopeFactory = scopeFactory;
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
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
 