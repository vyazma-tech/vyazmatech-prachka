using Microsoft.Extensions.Options;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;

internal static class OptionsMonitorExtensions
{
    public static IDisposable? OnValueChange<T>(this IOptionsMonitor<T> monitor, string propertyName, Action action)
    {
        object? initialPropertyValue = GetNamedPropertyValue(monitor.CurrentValue, propertyName);
        return monitor.OnChange(
            monitorValue =>
            {
                object? newPropertyValue = GetNamedPropertyValue(monitorValue, propertyName);

                if (Equals(initialPropertyValue, newPropertyValue) is false)
                    action.Invoke();
            });
    }

    private static object? GetNamedPropertyValue<T>(T @object, string propertyName)
    {
        return @object?.GetType().GetProperty(propertyName)?.GetValue(@object);
    }
}