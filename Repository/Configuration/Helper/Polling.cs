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
            _timer = new Timer(RunWeekly, null, TimeSpan.Zero, TimeSpan.FromDays(7));
            return Task.CompletedTask;
        }

        public async void RunWeekly(object? state) {
           using var scope = _scopeFactory.CreateScope();
            var playlistService = scope.ServiceProvider.GetRequiredService<IPlaylistService>();
            await playlistService.WeeklyRanking();
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
