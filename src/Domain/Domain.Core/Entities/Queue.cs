using Domain.Core.ValueObjects;

namespace Domain.Core.Entities;

public class Queue
{
    public Queue(uint capacity, QueueDay date)
    {
        if (capacity == 0)
        {
            throw new ArgumentException("Capacity should be more than 0", nameof(capacity));
        }

        Capacity = capacity;
        CreationDate = date;
    }

    private Queue()
    {
    }

    // private readonly List<Order> _orders = new();
    public uint Capacity { get; private set; }
    public QueueDay? CreationDate { get; private set; }

    // public IReadOnlyList<Order> Items => _orders.AsReadOnly();
}