using VyazmaTech.Prachka.Domain.Core.Queues.Events;

namespace VyazmaTech.Prachka.Presentation.Hubs.Queue;

public interface IQueueHubClient
{
    Task ReceiveQueueUpdated(QueueUpdatedDomainEvent Id);
}