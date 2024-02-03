using Domain.Common.Errors;
using Infrastructure.Authentication.Errors;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.Extensions;

public static class IdentityResultExtensions
{
    public static Error ToError(this IdentityResult result)
    {
        string errors = string.Join(' ', result.Errors.Select(x => x.Description));

        return AuthenticationErrors.IdentityUser.Creation(errors);
    }
}