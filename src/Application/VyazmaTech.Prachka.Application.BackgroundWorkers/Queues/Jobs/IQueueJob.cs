namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queues.Jobs;

public interface IQueueJob
{
    Task ExecuteAsync(DateOnly assignmentDate, CancellationToken stoppingToken);
}