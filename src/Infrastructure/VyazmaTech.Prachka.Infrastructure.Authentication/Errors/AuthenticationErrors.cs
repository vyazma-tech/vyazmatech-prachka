using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Errors;

public static class AuthenticationErrors
{
    public static class IdentityUser
    {
        public static Error NotFoundFor(string searchInfo)
        {
            return Error.NotFound(
                $"{nameof(IdentityUser)}.{nameof(NotFoundFor)}",
                $"Не удалось найти identity по '{searchInfo}'");
        }

        public static Error Creation(string details)
        {
            return Error.Unauthorized(
                $"{nameof(IdentityUser)}.{nameof(Creation)}",
                details);
        }

        public static Error NotInRole()
        {
            return Error.Forbidden(
                $"{nameof(IdentityUser)}.{nameof(NotInRole)}",
                "Тебе так нельзя");
        }
    }

    public static class IdentityToken
    {
        public static Error Refresh()
        {
            return Error.Unauthorized(
                $"{nameof(IdentityToken)}.{nameof(Refresh)}",
                "Не авторизован");
        }
    }
}