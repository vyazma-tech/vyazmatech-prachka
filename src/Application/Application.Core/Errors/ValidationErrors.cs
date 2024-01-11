using Domain.Common.Errors;

namespace Application.Core.Errors;

public static class ValidationErrors
{
    public static class MarkOrderAsReady
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(MarkOrderAsReady)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.",
            ErrorArea.Application);
    }

    public static class MarkOrderAsPaid
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(MarkOrderAsPaid)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.",
            ErrorArea.Application);
    }
}