using Domain.Common.Errors;

namespace Application.Core.Errors;

public static class ValidationErrors
{
    public static class MarkOrderAsReady
    {
        public static Error OrderIdIsRequired => new (
            $"{nameof(MarkOrderAsReady)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.");
    }

    public static class MarkOrderAsPaid
    {
        public static Error OrderIdIsRequired => new (
            $"{nameof(MarkOrderAsPaid)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.");
    }
}