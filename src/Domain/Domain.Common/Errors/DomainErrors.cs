namespace Domain.Common.Errors;

public static class DomainErrors
{
    public static class Order
    {
        public static Error AlreadyPaid => new(
            $"{nameof(Order)}.{nameof(AlreadyPaid)}",
            "The order was already paid.");

        public static Error IsReady => new(
            $"{nameof(Order)}.{nameof(IsReady)}",
            "The order is ready.");
    }

    public static class QueueDate
    {
        public static Error InThePast => new(
            $"{nameof(QueueDate)}.{nameof(InThePast)}",
            "The queue date should be later than now");

        public static Error NotNextWeek => new(
            $"{nameof(QueueDate)}.{nameof(NotNextWeek)}",
            "The queue date be on this week");
    }

    public static class Queue
    {
        public static Error ContainsOrderWithId(Guid id) => new(
            $"{nameof(Queue)}.{nameof(ContainsOrderWithId)}",
            $"The queue already contains order with id: {id}");

        public static Error OrderIsNotInQueue(Guid id) => new(
            $"{nameof(Queue)}.{nameof(OrderIsNotInQueue)}",
            $"The queue does not contain order with id: {id}");

        public static Error InvalidNewCapacity => new(
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "New queue capacity should not be less then current capacity.");
    }

    public static class OrderDate
    {
        public static Error InThePast => new(
            $"{nameof(OrderDate)}.{nameof(InThePast)}",
            "The order creation date should not be in the past");
    }
}