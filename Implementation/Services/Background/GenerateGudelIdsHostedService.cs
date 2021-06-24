using GudelIdService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Services.Background
{
    public class GenerateGudelIdsHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<GenerateGudelIdsHostedService> _logger;

        private Timer _timer;
        private IServiceProvider _services { get; }

        public GenerateGudelIdsHostedService(ILogger<GenerateGudelIdsHostedService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Generate GudelIds Hosted Service running.");

            using var scope = _services.CreateScope();
            var configService = scope.ServiceProvider.GetRequiredService<IConfigService>();
            _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(configService.CreateCronInterval()));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using var scope = _services.CreateScope();
            var gudelIdService = scope.ServiceProvider.GetRequiredService<IGudelIdService>();
            var configService = scope.ServiceProvider.GetRequiredService<IConfigService>();

            await gudelIdService.GenerateGudelIds(configService.CreateCronAmount(), null);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Generate GudelIds Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
