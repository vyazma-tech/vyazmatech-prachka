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
                $"Identity user with search info \"{searchInfo}\" was not found.");
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
                "Identity user has no granted access");
        }
    }

    public static class IdentityToken
    {
        public static Error Refresh()
        {
            return Error.Unauthorized(
                $"{nameof(IdentityToken)}.{nameof(Refresh)}",
                "Unauthorized.");
        }
    }
}