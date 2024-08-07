using VyazmaTech.Prachka.Presentation.Hubs.Queue.Models;

namespace VyazmaTech.Prachka.Presentation.Hubs.Queue;

public interface IQueueHubClient
{
    Task ReceiveQueueUpdated(QueueUpdatedModel Id);
}