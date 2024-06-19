namespace VyazmaTech.Prachka.Domain.Common.Errors;

public static class DomainErrors
{
    public static class Entity
    {
        public static Error NotFoundFor<TEntity>(string searchInfo)
        {
            return Error.NotFound(
                $"{nameof(Entity)}.{nameof(NotFoundFor)}",
                $"The entity of type {typeof(TEntity)} with {searchInfo} was not found.");
        }
    }

    public static class Order
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(Order)}.{nameof(NotFound)}",
            "The order with the specified identifier was not found.");

        public static Error NotFoundForRequest => Error.NotFound(
            $"{nameof(Order)}.{nameof(NotFoundForRequest)}",
            "The order for this request was not found");

        public static Error AlreadyPaid => Error.Conflict(
            $"{nameof(Order)}.{nameof(AlreadyPaid)}",
            "The order was already paid.");

        public static Error IsReady => Error.Conflict(
            $"{nameof(Order)}.{nameof(IsReady)}",
            "The order is ready.");

        public static Error UnableToTransferIntoSameQueue => Error.Unprocessable(
            $"{nameof(Order)}.{nameof(UnableToTransferIntoSameQueue)}",
            "The order cannot be transferred into the same queue.");

        public static Error UnableToTransferIntoFullQueue => Error.Unprocessable(
            $"{nameof(Order)}.{nameof(UnableToTransferIntoFullQueue)}",
            "The order cannot be transferred into full queue.");
    }

    public static class QueueDate
    {
        public static Error InThePast => Error.Validation(
            $"{nameof(QueueDate)}.{nameof(InThePast)}",
            "The queue date should be later than now");

        public static Error NotNextWeek => Error.Unprocessable(
            $"{nameof(QueueDate)}.{nameof(NotNextWeek)}",
            "The queue date should be on this week");
    }

    public static class Queue
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(Queue)}.{nameof(NotFound)}",
            "The queue with the specified identifier was not found.");

        public static Error NotFoundForRequest => Error.NotFound(
            $"{nameof(Queue)}.{nameof(NotFoundForRequest)}",
            "The queue for this request was not found");

        public static Error InvalidNewCapacity => Error.Validation(
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "New queue capacity should not be less then current capacity.");

        public static Error InvalidNewActivityBoundaries => Error.Validation(
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "New queue activity boundaries should not be equal current activity boundaries.");

        public static Error Overfull => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(Overfull)}",
            "Queue is overfull. You are not able to enter it now.");

        public static Error NotEnoughOrders(int count) => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(NotEnoughOrders)}",
            $"You have not enough orders. You are not able to exit queue with {count} orders");

        public static Error WillOverflow => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(WillOverflow)}",
            "Queue has not enough places. You are not able to enter it now.");

        public static Error Expired => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(Expired)}",
            "Queue already expired. You cannot perform this action.");

        public static Error ContainsOrderWithId(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Queue)}.{nameof(ContainsOrderWithId)}",
                $"The queue already contains order with OrderId = {id}");
        }

        public static Error OrderIsNotInQueue(Guid id)
        {
            return Error.Validation(
                $"{nameof(Queue)}.{nameof(OrderIsNotInQueue)}",
                $"The queue does not contain order with OrderId = {id}");
        }
    }

    public static class Subscription
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(Subscription)}.{nameof(NotFound)}",
            "The subscription with the specified identifier was not found.");

        public static Error NotFoundForRequest => Error.NotFound(
            $"{nameof(Subscription)}.{nameof(NotFoundForRequest)}",
            "The subscription for this request was not found");

        public static Error ContainsOrderWithId(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(ContainsOrderWithId)}",
                $"Already subscribed on newsletter about order with OrderId = {id}");
        }

        public static Error OrderIsNotInSubscription(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(OrderIsNotInSubscription)}",
                $"Not subscribed on newsletter about order with OrderId = {id}");
        }

        public static Error ContainsQueueWithId(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(ContainsQueueWithId)}",
                $"Already subscribed on newsletter about queue with QueueId = {id}");
        }

        public static Error QueueIsNotInSubscription(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(QueueIsNotInSubscription)}",
                $"Not subscribed on newsletter about queue with QueueId = {id}");
        }
    }

    public static class TelegramId
    {
        public static Error NullOrEmpty => Error.Validation(
            $"{nameof(TelegramId)}.{nameof(NullOrEmpty)}",
            "Telegram ID should not be null or empty.");

        public static Error InvalidFormat => Error.Validation(
            $"{nameof(TelegramId)}.{nameof(InvalidFormat)}",
            "Telegram username should start with '@'.");
    }

    public static class Fullname
    {
        public static Error NameIsNullOrEmpty => Error.Validation(
            $"{nameof(Fullname)}.{nameof(NameIsNullOrEmpty)}",
            "Name should not be null or empty.");

        public static Error InvalidNameFormat => Error.Validation(
            $"{nameof(Fullname)}.{nameof(InvalidNameFormat)}",
            "Name should start with uppercase and contain only letters");
    }

    public static class Capacity
    {
        public static Error Negative => Error.Validation(
            $"{nameof(Capacity)}.{nameof(Negative)}",
            "Capacity should be at least zero.");
    }

    public static class QueueActivityBoundaries
    {
        public static Error EmptyRange => Error.Validation(
            $"{nameof(QueueActivityBoundaries)}.{nameof(EmptyRange)}",
            "The queue activity boundaries should describe time range during the day.");
    }

    public static class User
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(User)}.{nameof(NotFound)}",
            "The user with the specified identifier was not found.");
    }
}