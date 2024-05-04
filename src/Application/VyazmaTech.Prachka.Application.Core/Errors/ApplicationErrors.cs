using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Application.Core.Errors;

public static class ApplicationErrors
{
    public static class BulkInsertOrders
    {
        public static Error AnonymousUserCantEnter => Error.Unprocessable(
            code: $"{nameof(BulkInsertOrders)}.{nameof(AnonymousUserCantEnter)}",
            description: "Anonymous user can't enter the queue.",
            area: ErrorArea.Application);
    }

    public static class BulkRemoveOrders
    {
        public static Error AnonymousUserCantExit => Error.Unprocessable(
            code: $"{nameof(BulkInsertOrders)}.{nameof(AnonymousUserCantExit)}",
            description: "Anonymous user can't enter the queue.",
            area: ErrorArea.Application);

        public static Error UnableToRemoveWithExceededQuantity => Error.Unprocessable(
            code: $"{nameof(BulkRemoveOrders)}.{nameof(UnableToRemoveWithExceededQuantity)}",
            description: "Provided order quantity exceeds user order quantity.",
            area: ErrorArea.Application);
    }

    public static class Subscription
    {
        public static Error AnonymousUserCantSubscribe => Error.Unprocessable(
            code: $"{nameof(Subscription)}.{nameof(AnonymousUserCantSubscribe)}",
            description: "Authenticate against the system to subscribe",
            area: ErrorArea.Application);

        public static Error UserHasNoSubscriptions(Guid id) => Error.NotFound(
            code: $"{nameof(Subscription)}.{nameof(UserHasNoSubscriptions)}",
            description: $"User with Id = {id} has no subscriptions",
            area: ErrorArea.Application);
    }
}