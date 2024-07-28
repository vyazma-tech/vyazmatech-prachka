using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSchedulingDashboard(this IApplicationBuilder builder)
    {
        var options = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        };
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();

        builder.UseForwardedHeaders(options);

        // builder.UsePathBase("/hangfire/");
        builder.UseHangfireDashboard(
            options: new DashboardOptions
            {
                Authorization = [],
                DarkModeEnabled = true
            });

        return builder;
    }
}