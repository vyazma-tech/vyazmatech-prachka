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
}