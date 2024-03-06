using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Application.Core.Errors;

public static class ValidationErrors
{
    public static class ProlongOrder
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(ProlongOrder)}.{nameof(OrderIdIsRequired)}",
            "The order identifier is required.",
            ErrorArea.Application);

        public static Error TargetQueueIdIdIsRequired => Error.Validation(
            $"{nameof(ProlongOrder)}.{nameof(TargetQueueIdIdIsRequired)}",
            "The order identifier is required.",
            ErrorArea.Application);
    }

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

    public static class CreateUser
    {
        public static Error UserIdIsRequired => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(UserIdIsRequired)}",
            "The user identifier is required.",
            ErrorArea.Application);

        public static Error InvalidCredentialsFormat => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(InvalidCredentialsFormat)}",
            "Provide not empty user credentials.",
            ErrorArea.Application);

        public static Error InvalidRoleFormat => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(InvalidRoleFormat)}",
            "Provide not empty and not null user role.",
            ErrorArea.Application);
    }

    public static class ChangeRole
    {
        public static Error UsernameIsRequired => Error.Validation(
            $"{nameof(ChangeRole)}.{nameof(UsernameIsRequired)}",
            "Username should not be null or empty.",
            ErrorArea.Application);

        public static Error NewRoleNameIsRequired => Error.Validation(
            $"{nameof(ChangeRole)}.{nameof(NewRoleNameIsRequired)}",
            "Role name should not be null or empty.",
            ErrorArea.Application);
    }

    public static class RevokeToken
    {
        public static Error UsernameIsRequired => Error.Validation(
            $"{nameof(RevokeToken)}.{nameof(UsernameIsRequired)}",
            "Username should not be null or empty.",
            ErrorArea.Application);
    }
}