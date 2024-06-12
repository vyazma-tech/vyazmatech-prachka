using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Application.Core.Errors;

public static class ApplicationErrors
{
    public static class BulkInsertOrders
    {
        public static Error AnonymousUserCantEnter => Error.Unprocessable(
            $"{nameof(BulkInsertOrders)}.{nameof(AnonymousUserCantEnter)}",
            "Anonymous user can't enter the queue.",
            ErrorArea.Application);
    }

    public static class BulkRemoveOrders
    {
        public static Error AnonymousUserCantExit => Error.Unprocessable(
            $"{nameof(BulkInsertOrders)}.{nameof(AnonymousUserCantExit)}",
            "Anonymous user can't enter the queue.",
            ErrorArea.Application);

        public static Error UnableToRemoveWithExceededQuantity => Error.Unprocessable(
            $"{nameof(BulkRemoveOrders)}.{nameof(UnableToRemoveWithExceededQuantity)}",
            "Provided order quantity exceeds user order quantity.",
            ErrorArea.Application);
    }

    public static class Subscription
    {
        public static Error AnonymousUserCantSubscribe => Error.Unprocessable(
            $"{nameof(Subscription)}.{nameof(AnonymousUserCantSubscribe)}",
            "Authenticate against the system to subscribe",
            ErrorArea.Application);

        public static Error UserHasNoSubscriptions(Guid id)
        {
            return Error.NotFound(
                $"{nameof(Subscription)}.{nameof(UserHasNoSubscriptions)}",
                $"User with Id = {id} has no subscriptions",
                ErrorArea.Application);
        }
    }
}