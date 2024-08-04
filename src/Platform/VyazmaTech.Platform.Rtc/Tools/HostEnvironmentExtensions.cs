namespace VyazmaTech.Platform.Rtc.Tools;

internal static class HostEnvironmentExtensions
{
    internal static string GetBasePath(this IHostEnvironment environment)
    {
        return environment.IsStaging()
            ? ".stg"
            : ".prod";
    }
}