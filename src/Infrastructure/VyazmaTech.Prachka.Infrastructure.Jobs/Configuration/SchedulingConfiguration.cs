using VyazmaTech.Platform.Rtc.Attributes;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Configuration;

[RealtimeOption(SectionKey)]
public class SchedulingConfiguration
{
    public const string SectionKey = "Application:SchedulingConfiguration";

    public TimeOnly WeekdayActiveFrom { get; set; }

    public TimeOnly WeekdayActiveUntil { get; set; }

    public TimeOnly DayOfActiveFrom { get; set; }

    public TimeOnly DayOfActiveUntil { get; set; }

    public int DefaultCapacity { get; set; }

    public int SeedingInterval { get; set; }
}