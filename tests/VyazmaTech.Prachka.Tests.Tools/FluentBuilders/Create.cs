namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

internal static class Create
{
    public static OrderFluentBuilder Order => new();

    public static QueueFluentBuilder Queue => new();

    public static UserFluentBuilder User => new();
}