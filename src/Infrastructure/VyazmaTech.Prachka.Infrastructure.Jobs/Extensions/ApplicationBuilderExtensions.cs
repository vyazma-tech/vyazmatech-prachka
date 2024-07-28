using Hangfire;
using Microsoft.AspNetCore.Builder;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

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

        RecurringJob.AddOrUpdate<QueueSeedingJob>(
            recurringJobId: nameof(QueueSeedingJob),
            methodCall: job => job.Execute(),
            cronExpression: "0 0 * * 7");

        return builder;
    }
}