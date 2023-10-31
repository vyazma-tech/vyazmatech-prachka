namespace Domain.Common.Errors;

public static class DomainErrors
{
    public static class Order
    {
        public static Error AlreadyPaid => new (
            $"{nameof(Order)}.{nameof(AlreadyPaid)}",
            "The order was already paid.");

        public static Error IsReady => new (
            $"{nameof(Order)}.{nameof(IsReady)}",
            "The order is ready.");
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
        public static Error ContainsOrderWithId(Guid id) => new (
            $"{nameof(Queue)}.{nameof(ContainsOrderWithId)}",
            $"The queue already contains order with id: {id}");

        public static Error OrderIsNotInQueue(Guid id) => new (
            $"{nameof(Queue)}.{nameof(OrderIsNotInQueue)}",
            $"The queue does not contain order with id: {id}");

        public static Error InvalidNewCapacity => new (
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "New queue capacity should not be less then current capacity.");
    }

    public static class Subscriber
    {
        public static Error ContainsOrderWithId(Guid id) => new (
            $"{nameof(Subscriber)}.{nameof(ContainsOrderWithId)}",
            $"The subscription already contains order with id: {id}");

        public static Error OrderIsNotInSubscription(Guid id) => new (
            $"{nameof(Subscriber)}.{nameof(OrderIsNotInSubscription)}",
            $"The subscription does not contain order with id: {id}");
    }

    public static class OrderDate
    {
        public static Error InThePast => new (
            $"{nameof(OrderDate)}.{nameof(InThePast)}",
            "The order creation date should not be in the past");
    }

    public static class SubscriberDate
    {
        public static Error InThePast => new (
            $"{nameof(OrderDate)}.{nameof(InThePast)}",
            "The subscriber creation date should not be in the past");
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
        public static Error FirstnameIsNullOrEmpty => new (
            $"{nameof(Fullname)}.{nameof(FirstnameIsNullOrEmpty)}",
            "Firstname should not be null or empty.");

        public static Error MiddlenameIsNullOrEmpty => new (
            $"{nameof(Fullname)}.{nameof(MiddlenameIsNullOrEmpty)}",
            "Firstname should not be null or empty.");

        public static Error LastnameIsNullOrEmpty => new (
            $"{nameof(Fullname)}.{nameof(LastnameIsNullOrEmpty)}",
            "Firstname should not be null or empty.");

        public static Error InvalidFirstnameFormat => new (
            $"{nameof(Fullname)}.{nameof(InvalidFirstnameFormat)}",
            "Firstname should start with uppercase.");

        public static Error InvalidMiddlenameFormat => new (
            $"{nameof(Fullname)}.{nameof(InvalidMiddlenameFormat)}",
            "Middlename should start with uppercase.");

        public static Error InvalidLastnameFormat => new (
            $"{nameof(Fullname)}.{nameof(InvalidLastnameFormat)}",
            "Lastname should start with uppercase.");
    }

    public static class UserRegistrationDate
    {
        public static Error InThePast => new (
            $"{nameof(UserRegistrationDate)}.{nameof(InThePast)}",
            "User registration date should not be in the past");
    }
}