namespace VyazmaTech.Platform.Rtc.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class RealtimeOptionAttribute : Attribute
{
    public RealtimeOptionAttribute(string section)
    {
        Section = section;
    }

    public string Section { get; }
}