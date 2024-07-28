using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSchedulingDashboard(this IApplicationBuilder builder)
    {
        builder.UseHangfireDashboard(
            options: new DashboardOptions
            {
                Authorization = [],
                DarkModeEnabled = true
            });

        return builder;
    }
}