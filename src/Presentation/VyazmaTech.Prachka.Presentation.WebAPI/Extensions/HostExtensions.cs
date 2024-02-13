using Serilog;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

internal static class HostExtensions
{
    internal static IHostBuilder AddSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        return host;
    }
}