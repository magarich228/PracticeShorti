using Shorti.Identity.Api.Identity.Abstractions;

namespace Shorti.Identity.Api.Identity
{
    public class JwtRefreshTokenCache : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IJwtIdentityManager _jwtIdentityManager;

        public JwtRefreshTokenCache(IJwtIdentityManager jwtAuthManager)
        {
            _jwtIdentityManager = jwtAuthManager;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _jwtIdentityManager.RemoveExpiredRefreshTokens(DateTime.Now);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
