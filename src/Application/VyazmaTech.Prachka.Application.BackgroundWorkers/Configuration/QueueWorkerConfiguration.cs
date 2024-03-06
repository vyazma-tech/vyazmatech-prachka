namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;

public class QueueWorkerConfiguration
{
    public const string SectionKey = "Application:WorkersConfiguration";

    public TimeSpan SharedDelay { get; set; }
}