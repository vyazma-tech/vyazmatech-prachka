using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers;

internal abstract class RestartableBackgroundWorker : BackgroundService
{
    private readonly ILogger<RestartableBackgroundWorker> _logger;

    protected RestartableBackgroundWorker(ILogger<RestartableBackgroundWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            try
            {
                await ExecuteAsync(cts);
            }
            catch (Exception e) when (e is OperationCanceledException or TaskCanceledException &&
                                      cts.IsCancellationRequested)
            {
                _logger.LogWarning("Worker configuration changed. Restarting within 5 seconds");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    protected abstract Task ExecuteAsync(CancellationTokenSource cts);
}