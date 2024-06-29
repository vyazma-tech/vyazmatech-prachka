namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;

public class OutboxConfiguration
{
    public const string SectionKey = "Application:OutboxConfiguration";

    public TimeSpan Delay { get; set; }

    public int BatchSize { get; set; }
}