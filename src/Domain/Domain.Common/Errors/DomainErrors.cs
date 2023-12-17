namespace Domain.Common.Errors;

public static class DomainErrors
{
    public static class Entity
    {
        public static Error NotFoundFor<TEntity>(string searchInfo)
        {
            return new Error(
                $"{nameof(Entity)}.{nameof(NotFoundFor)}",
                $"The entity of type {typeof(TEntity)} with {searchInfo} was not found.");
        }
    }

    public static class Order
    {
        public static Error NotFound => new (
            $"{nameof(Order)}.{nameof(NotFound)}",
            "The order with the specified identifier was not found.");

        public static Error NotFoundForRequest => new (
            $"{nameof(Order)}.{nameof(NotFoundForRequest)}",
            "The order for this request was not found");

        public static Error AlreadyPaid => new (
            $"{nameof(Order)}.{nameof(AlreadyPaid)}",
            "The order was already paid.");

        public static Error IsReady => new (
            $"{nameof(Order)}.{nameof(IsReady)}",
            "The order is ready.");

        public static Error UnableToTransferIntoSameQueue => new (
            $"{nameof(Order)}.{nameof(UnableToTransferIntoSameQueue)}",
            "The order cannot be transferred into the same queue.");

        public static Error UnableToTransferIntoFullQueue => new (
            $"{nameof(Order)}.{nameof(UnableToTransferIntoFullQueue)}",
            "The order cannot be transferred into full queue.");
    }

    public static class QueueDate
    {
        public static Error InThePast => new (
            $"{nameof(QueueDate)}.{nameof(InThePast)}",
            "The queue date should be later than now");

        public static Error NotNextWeek => new (
            $"{nameof(QueueDate)}.{nameof(NotNextWeek)}",
            "The queue date be on this week");
    }

    public static class Queue
    {
        public static Error NotFound => new (
            $"{nameof(Queue)}.{nameof(NotFound)}",
            "The queue with the specified identifier was not found.");

        public static Error NotFoundForRequest => new (
            $"{nameof(Queue)}.{nameof(NotFoundForRequest)}",
            "The queue for this request was not found");

        public static Error InvalidNewCapacity => new (
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "New queue capacity should not be less then current capacity.");

        public static Error InvalidNewActivityBoundaries => new (
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "New queue activity boundaries should not be equal current activity boundaries.");

        public static Error Overfull => new (
            $"{nameof(Queue)}.{nameof(Overfull)}",
            "Queue is overfull. You are not able to enter it now.");

        public static Error Expired => new (
            $"{nameof(Queue)}.{nameof(Expired)}",
            "Queue already expired. You cannot perform this action.");

        public static Error ContainsOrderWithId(Guid id) => new (
            $"{nameof(Queue)}.{nameof(ContainsOrderWithId)}",
            $"The queue already contains order with id: {id}");

        public static Error OrderIsNotInQueue(Guid id) => new (
            $"{nameof(Queue)}.{nameof(OrderIsNotInQueue)}",
            $"The queue does not contain order with id: {id}");
    }

    public static class Subscription
    {
        public static Error NotFound => new (
            $"{nameof(Subscription)}.{nameof(NotFound)}",
            "The subscription with the specified identifier was not found.");

        public static Error NotFoundForRequest => new (
            $"{nameof(Subscription)}.{nameof(NotFoundForRequest)}",
            "The subscription for this request was not found");

        public static Error ContainsOrderWithId(Guid id) => new (
            $"{nameof(Subscription)}.{nameof(ContainsOrderWithId)}",
            $"Already subscribed on newsletter about order with id: {id}");

        public static Error OrderIsNotInSubscription(Guid id) => new (
            $"{nameof(Subscription)}.{nameof(OrderIsNotInSubscription)}",
            $"Not subscribed on newsletter about order with id: {id}");

        public static Error ContainsQueueWithId(Guid id) => new (
            $"{nameof(Subscription)}.{nameof(ContainsQueueWithId)}",
            $"Already subscribed on newsletter about queue with id: {id}");

        public static Error QueueIsNotInSubscription(Guid id) => new (
            $"{nameof(Subscription)}.{nameof(QueueIsNotInSubscription)}",
            $"Not subscribed on newsletter about queue with id: {id}");
    }

    public static class TelegramId
    {
        public static Error NullOrEmpty => new (
            $"{nameof(TelegramId)}.{nameof(NullOrEmpty)}",
            "Telegram ID should not be null or empty.");

        public static Error InvalidFormat => new (
            $"{nameof(TelegramId)}.{nameof(InvalidFormat)}",
            "Telegram ID should be a number.");
    }

    public static class Fullname
    {
        public static Error NameIsNullOrEmpty => new (
            $"{nameof(Fullname)}.{nameof(NameIsNullOrEmpty)}",
            "Name should not be null or empty.");

        public static Error InvalidNameFormat => new (
            $"{nameof(Fullname)}.{nameof(InvalidNameFormat)}",
            "Name should start with uppercase and contain only letters");
    }

    public static class Capacity
    {
        public static Error Negative => new (
            $"{nameof(Capacity)}.{nameof(Negative)}",
            "Capacity should be at least zero.");
    }

    public static class QueueActivityBoundaries
    {
        public static Error EmptyRange => new (
            $"{nameof(QueueActivityBoundaries)}.{nameof(EmptyRange)}",
            "The queue activity boundaries should describe time range during the day.");
    }

    public static class User
    {
        public static Error NotFound => new (
            $"{nameof(User)}.{nameof(NotFound)}",
            "The user with the specified identifier was not found.");
    }
}