using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Application.Core.Errors;

public static class ApplicationErrors
{
    public static class BulkInsertOrders
    {
        public static Error AnonymousUserCantEnter => Error.Unprocessable(
            $"{nameof(BulkInsertOrders)}.{nameof(AnonymousUserCantEnter)}",
            "Чтобы встать в очередь, сначала залогинься");
    }

    public static class BulkRemoveOrders
    {
        public static Error AnonymousUserCantExit => Error.Unprocessable(
            $"{nameof(BulkInsertOrders)}.{nameof(AnonymousUserCantExit)}",
            "Чтобы выйти из очереди, сначала залогинься");

        public static Error UnableToRemoveWithExceededQuantity => Error.Unprocessable(
            $"{nameof(BulkRemoveOrders)}.{nameof(UnableToRemoveWithExceededQuantity)}",
            "Указанное количество пакетов не соответствует тому количество, с которым ты заходил");
    }

    public static class Subscription
    {
        public static Error AnonymousUserCantSubscribe => Error.Unprocessable(
            $"{nameof(Subscription)}.{nameof(AnonymousUserCantSubscribe)}",
            "Чтобы подписаться на уведомления, сначала залогинься");

        public static Error UserHasNoSubscriptions(Guid id)
        {
            return Error.NotFound(
                $"{nameof(Subscription)}.{nameof(UserHasNoSubscriptions)}",
                "У тебя нет уведомлений");
        }
    }

    public static class MyOrders
    {
        public static Error AnonymousUserCantSeeTheirOrders => Error.Unauthorized(
            $"{nameof(MyOrders)}.{nameof(AnonymousUserCantSeeTheirOrders)}",
            "Чтобы посмотреть свои заказы, сначала залогинься");
    }
}