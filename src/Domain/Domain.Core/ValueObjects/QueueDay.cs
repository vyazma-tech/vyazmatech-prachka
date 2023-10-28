namespace Domain.Core.ValueObjects;

public class QueueDay
{
    public const int Week = 7;

    private QueueDay(DateTime value) => Value = value;

    public DateTime Value { get; }

    public static QueueDay Create(DateTime value)
    {
        if (value <= DateTime.Now)
        {
            throw new ArgumentException("Day of queue should be later than now");
        }

        if (value >= DateTime.Now.AddDays(Week))
        {
            throw new ArgumentException("Day of queue should be on this week");
        }

        return new QueueDay(value);
    }
}