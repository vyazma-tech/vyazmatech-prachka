using Hangfire;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EntryJobs;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

public sealed class QueueJobScheduler
{
    private readonly IBackgroundJobClient _client;
    private readonly IDateTimeProvider _timeProvider;

    public QueueJobScheduler(IBackgroundJobClient client, IDateTimeProvider timeProvider)
    {
        _client = client;
        _timeProvider = timeProvider;
    }

    public string Schedule(IEntryLifecycleCommand command)
    {
        string jobId = command.Execute(_client, _timeProvider);

        return jobId;
    }

    public void Reschedule(IEnclosingLifecycleCommand command)
    {
        command.Execute(_client, _timeProvider);
    }
}