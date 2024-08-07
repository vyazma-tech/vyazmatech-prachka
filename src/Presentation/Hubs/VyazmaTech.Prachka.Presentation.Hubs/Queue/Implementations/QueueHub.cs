using Microsoft.AspNetCore.SignalR;

namespace VyazmaTech.Prachka.Presentation.Hubs.Queue.Implementations;

public sealed class QueueHub : Hub<IQueueHubClient>
{
    private static long _connectionCounter;
    private readonly ILogger<QueueHub> _logger;

    public QueueHub(ILogger<QueueHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        Interlocked.Increment(ref _connectionCounter);
        _logger.LogInformation("New hub connection. Current connections {Count}", _connectionCounter);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Interlocked.Decrement(ref _connectionCounter);

        return base.OnDisconnectedAsync(exception);
    }
}