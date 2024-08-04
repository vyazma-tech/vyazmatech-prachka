namespace VyazmaTech.Prachka.Domain.Common.Errors;

public static class DomainErrors
{
    public static class Entity
    {
        public static Error NotFoundFor<TEntity>(string searchInfo)
        {
            return Error.NotFound(
                $"{nameof(Entity)}.{nameof(NotFoundFor)}",
                $"Не удалось найти сущность {typeof(TEntity).Name} по запросу \"{searchInfo}\"");
        }
    }

    public static class Order
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(Order)}.{nameof(NotFound)}",
            "Не удалось найти заказ с данным идентификатором");

        public static Error NotFoundForRequest => Error.NotFound(
            $"{nameof(Order)}.{nameof(NotFoundForRequest)}",
            "Не удалось найти заказ");

        public static Error AlreadyPaid => Error.Conflict(
            $"{nameof(Order)}.{nameof(AlreadyPaid)}",
            "Заказ уже оплачен");

        public static Error IsReady => Error.Conflict(
            $"{nameof(Order)}.{nameof(IsReady)}",
            "Заказ уже готов");

        public static Error UnableToTransferIntoSameQueue => Error.Unprocessable(
            $"{nameof(Order)}.{nameof(UnableToTransferIntoSameQueue)}",
            "Нельзя перевести заказ в ту же очередь");

        public static Error UnableToTransferIntoFullQueue => Error.Unprocessable(
            $"{nameof(Order)}.{nameof(UnableToTransferIntoFullQueue)}",
            "Нельзя перевести заказ в переполненную очередь");
    }

    public static class QueueDate
    {
        public static Error InThePast => Error.Validation(
            $"{nameof(QueueDate)}.{nameof(InThePast)}",
            "Дата активности очереди должна быть сегодня или на текущей неделе");

        public static Error NotNextWeek => Error.Unprocessable(
            $"{nameof(QueueDate)}.{nameof(NotNextWeek)}",
            "Дата активности очереди должна быть на текущей неделе");
    }

    public static class Queue
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(Queue)}.{nameof(NotFound)}",
            "Не удалось найти очередь с данным идентификатором");

        public static Error NotFoundForRequest => Error.NotFound(
            $"{nameof(Queue)}.{nameof(NotFoundForRequest)}",
            "Не удалось найти очередь");

        public static Error InvalidNewCapacity => Error.Validation(
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "Новое значение вместимости очереди должно быть больше текущего значения");

        public static Error InvalidNewActivityBoundaries => Error.Validation(
            $"{nameof(Queue)}.{nameof(InvalidNewCapacity)}",
            "Новое значение времени активности очереди должно отличаться от предыдущего");

        public static Error NotEnoughOrders(int count) => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(NotEnoughOrders)}",
            $"У тебя недостаточно заказов в очереди. Ты не можешь покинуть ее с {count} заказами");

        public static Error WillOverflow => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(WillOverflow)}",
            "В очереди недостаточно места. Ты не можешь войти сейчас");

        public static Error Expired => Error.Unprocessable(
            $"{nameof(Queue)}.{nameof(Expired)}",
            "Очередь уже истекла. Ты не можешь сделать это");

        public static Error ContainsOrderWithId(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Queue)}.{nameof(ContainsOrderWithId)}",
                $"В очереди уже есть заказ с OrderId = {id}");
        }

        public static Error OrderIsNotInQueue(Guid id)
        {
            return Error.Validation(
                $"{nameof(Queue)}.{nameof(OrderIsNotInQueue)}",
                $"В очереди нет заказа с OrderId = {id}");
        }
    }

    public static class Subscription
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(Subscription)}.{nameof(NotFound)}",
            "Не удалость найти уведомление по данному идентификатору");

        public static Error NotFoundForRequest => Error.NotFound(
            $"{nameof(Subscription)}.{nameof(NotFoundForRequest)}",
            "Не удалось найти уведомление");

        public static Error ContainsOrderWithId(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(ContainsOrderWithId)}",
                "Ты уже подписан на уведомления об этом заказе");
        }

        public static Error OrderIsNotInSubscription(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(OrderIsNotInSubscription)}",
                "Ты не подписался на уведомления об этом заказе");
        }

        public static Error ContainsQueueWithId(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(ContainsQueueWithId)}",
                "Ты уже подписан на уведомления об этой очереди");
        }

        public static Error QueueIsNotInSubscription(Guid id)
        {
            return Error.Unprocessable(
                $"{nameof(Subscription)}.{nameof(QueueIsNotInSubscription)}",
                "Ты не подписался на уведомления об этой очереди");
        }
    }

    public static class TelegramId
    {
        public static Error NullOrEmpty => Error.Validation(
            $"{nameof(TelegramId)}.{nameof(NullOrEmpty)}",
            "Telegram ID должен быть числом");

        public static Error InvalidFormat => Error.Validation(
            $"{nameof(TelegramId)}.{nameof(InvalidFormat)}",
            "Юзернейм должен начинаться с '@'");
    }

    public static class Fullname
    {
        public static Error NameIsNullOrEmpty => Error.Validation(
            $"{nameof(Fullname)}.{nameof(NameIsNullOrEmpty)}",
            "ФИО не должно быть пустым");

        public static Error InvalidNameFormat => Error.Validation(
            $"{nameof(Fullname)}.{nameof(InvalidNameFormat)}",
            "ФИО должно начинаться с большой буквы и содержать только буквы");
    }

    public static class Capacity
    {
        public static Error Negative => Error.Validation(
            $"{nameof(Capacity)}.{nameof(Negative)}",
            "Вместимость очереди должна быть больше нуля");
    }

    public static class QueueActivityBoundaries
    {
        public static Error EmptyRange => Error.Validation(
            $"{nameof(QueueActivityBoundaries)}.{nameof(EmptyRange)}",
            "Значение времени активности очереди должно быть не пустым временным промежутком");
    }

    public static class User
    {
        public static Error NotFound => Error.NotFound(
            $"{nameof(User)}.{nameof(NotFound)}",
            "Не удалось найти юзера по данному идентификатору");
    }

    public static class Price
    {
        public static Error NegativePrice => Error.Validation(
            $"{nameof(Price)}.{nameof(NegativePrice)}",
            "Заплаченная студентом сумма не должна быть отрицательной");
    }
}