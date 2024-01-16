namespace Application.BackgroundWorkers.Configuration;

public class QueueWorkerConfiguration
{
    public const string SectionKey = nameof(QueueWorkerConfiguration);

    public TimeSpan SharedDelay { get; set; }
}