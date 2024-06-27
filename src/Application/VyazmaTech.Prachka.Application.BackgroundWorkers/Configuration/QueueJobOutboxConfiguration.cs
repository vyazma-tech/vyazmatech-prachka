namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;

public class QueueJobOutboxConfiguration
{
    public const string SectionKey = "Application:QueueJobOutboxConfiguration";

    public TimeSpan Delay { get; set; }

    public int BatchSize { get; set; }
}