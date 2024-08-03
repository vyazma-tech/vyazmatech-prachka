using VyazmaTech.Platform.Rtc.Attributes;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;

[RealtimeOption(SectionKey)]
public class OutboxConfiguration
{
    public const string SectionKey = "Application:OutboxConfiguration";

    public TimeSpan Delay { get; set; }

    public int BatchSize { get; set; }
}