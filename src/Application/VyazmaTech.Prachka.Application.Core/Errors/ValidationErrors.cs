using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Application.Core.Errors;

public static class ValidationErrors
{
    public static class ProlongOrder
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(ProlongOrder)}.{nameof(OrderIdIsRequired)}",
            "OrderId обязательное поле");

        public static Error TargetQueueIdIdIsRequired => Error.Validation(
            $"{nameof(ProlongOrder)}.{nameof(TargetQueueIdIdIsRequired)}",
            "OrderId обязательное поле");
    }

    public static class MarkOrderAsReady
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(MarkOrderAsReady)}.{nameof(OrderIdIsRequired)}",
            "OrderId обязательное поле");
    }

    public static class MarkOrderAsPaid
    {
        public static Error OrderIdIsRequired => Error.Validation(
            $"{nameof(MarkOrderAsPaid)}.{nameof(OrderIdIsRequired)}",
            "OrderId обязательное поле");

        public static Error NegativePrice => Error.Validation(
            $"{nameof(MarkOrderAsPaid)}.{nameof(NegativePrice)}",
            "Стоимость заказа не должна быть отрицательной");
    }

    public static class CreateUser
    {
        public static Error UserIdIsRequired => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(UserIdIsRequired)}",
            "UserId обязательное поле");

        public static Error InvalidCredentialsFormat => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(InvalidCredentialsFormat)}",
            "Данные пользователя в системе авторизации не должны быть пустыми");

        public static Error InvalidRoleFormat => Error.Validation(
            $"{nameof(CreateUser)}.{nameof(InvalidRoleFormat)}",
            "Укажи роль пользователя");
    }

    public static class ChangeRole
    {
        public static Error UsernameIsRequired => Error.Validation(
            $"{nameof(ChangeRole)}.{nameof(UsernameIsRequired)}",
            "Username обязательное поле");

        public static Error NewRoleNameIsRequired => Error.Validation(
            $"{nameof(ChangeRole)}.{nameof(NewRoleNameIsRequired)}",
            "RoleName обязательное поле");
    }

    public static class RevokeToken
    {
        public static Error UsernameIsRequired => Error.Validation(
            $"{nameof(RevokeToken)}.{nameof(UsernameIsRequired)}",
            "Username обязательное поле");
    }

    public static class BulkInsertOrders
    {
        public static Error OrderQuantityShouldBePositive => Error.Validation(
            $"{nameof(BulkInsertOrders)}.{nameof(OrderQuantityShouldBePositive)}",
            "Чтобы встать в очередь укажи количество пакетов");
    }

    public static class BulkRemoveOrders
    {
        public static Error OrderQuantityShouldBePositive => Error.Validation(
            $"{nameof(BulkRemoveOrders)}.{nameof(OrderQuantityShouldBePositive)}",
            "Чтобы выйти из очереди укажи количество пакетов");
    }

    public static class BanUser
    {
        public static Error InvalidUsername => Error.Validation(
            $"{nameof(BanUser)}.{nameof(InvalidUsername)}",
            "Юзернейм должен начинаться с '@' и не быть пустой строкой");
    }
}