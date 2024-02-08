using Domain.Common.Errors;

namespace Infrastructure.Authentication.Errors;

public static class AuthenticationErrors
{
    public static class IdentityUser
    {
        public static Error NotFoundFor(string searchInfo)
            => Error.NotFound(
                $"{nameof(IdentityUser)}.{nameof(NotFoundFor)}",
                $"Identity user with search info \"{searchInfo}\" was not found.",
                ErrorArea.Infrastructure);

        public static Error Creation(string details)
            => Error.Unauthorized(
                $"{nameof(IdentityUser)}.{nameof(Creation)}",
                details,
                ErrorArea.Infrastructure);
    }

    public static class IdentityToken
    {
        public static Error Refresh()
            => Error.Unauthorized(
                $"{nameof(IdentityToken)}.{nameof(Refresh)}",
                "Unauthorized.",
                ErrorArea.Infrastructure);
    }
}