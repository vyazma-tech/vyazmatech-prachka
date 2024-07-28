using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Application.Core.Errors;

public static class ValidationErrors
{
    public static class ProlongOrder
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(ProlongOrder)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.");

        public static Error TargetQueueIdIdIsRequired => Error.Validation(
            $"{nameof(ProlongOrder)}.{nameof(TargetQueueIdIdIsRequired)}",
            "The order identifier is required.");
    }

    public static class MarkOrderAsReady
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(MarkOrderAsReady)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.");
    }

    public static class MarkOrderAsPaid
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(MarkOrderAsPaid)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.");

        public static Error NegativePrice => Error.Validation(
            $"{nameof(MarkOrderAsPaid)}.{nameof(NegativePrice)}",
            "The order price should not be negative.");
    }

    public static class CreateUser
    {
        public static Error UserIdIsRequired => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(UserIdIsRequired)}",
            "The user identifier is required.");

        public static Error InvalidCredentialsFormat => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(InvalidCredentialsFormat)}",
            "Provide not empty user credentials.");

        public static Error InvalidRoleFormat => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(InvalidRoleFormat)}",
            "Provide not empty and not null user role.");
    }

    public static class ChangeRole
    {
        public static Error UsernameIsRequired => Error.Validation(
            $"{nameof(ChangeRole)}.{nameof(UsernameIsRequired)}",
            "Username should not be null or empty.");

        public static Error NewRoleNameIsRequired => Error.Validation(
            $"{nameof(ChangeRole)}.{nameof(NewRoleNameIsRequired)}",
            "Role name should not be null or empty.");
    }

    public static class RevokeToken
    {
        public static Error UsernameIsRequired => Error.Validation(
            $"{nameof(RevokeToken)}.{nameof(UsernameIsRequired)}",
            "Username should not be null or empty.");
    }

    public static class BulkInsertOrders
    {
        public static Error OrderQuantityShouldBePositive => Error.Validation(
            $"{nameof(BulkInsertOrders)}.{nameof(OrderQuantityShouldBePositive)}",
            "You should provide positive order quantity to enter queue");
    }

    public static class BulkRemoveOrders
    {
        public static Error OrderQuantityShouldBePositive => Error.Validation(
            $"{nameof(BulkRemoveOrders)}.{nameof(OrderQuantityShouldBePositive)}",
            "You should provide positive order quantity to exit queue.");
    }
}