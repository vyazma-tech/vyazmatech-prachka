namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;

public class QueueSchedulingConfiguration
{
    public const string SectionKey = "Application:SchedulingConfiguration";

    public TimeSpan Delay { get; set; }

    public TimeOnly WeekdayActiveFrom { get; set; }

    public TimeOnly WeekdayActiveUntil { get; set; }

    public TimeOnly DayOfActiveFrom { get; set; }

    public TimeOnly DayOfActiveUntil { get; set; }

    public int DefaultCapacity { get; set; }

    public int SeedingInterval { get; set; }
}