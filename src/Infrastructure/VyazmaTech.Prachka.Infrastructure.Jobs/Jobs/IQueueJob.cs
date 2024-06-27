namespace VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

public interface IQueueJob
{
    Task ExecuteAsync(DateOnly assignmentDate, CancellationToken stoppingToken);
}