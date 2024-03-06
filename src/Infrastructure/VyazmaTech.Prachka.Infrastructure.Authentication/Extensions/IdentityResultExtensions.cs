using Microsoft.AspNetCore.Identity;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Infrastructure.Authentication.Errors;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;

public static class IdentityResultExtensions
{
    public static Error ToError(this IdentityResult result)
    {
        string errors = string.Join(' ', result.Errors.Select(x => x.Description));

        return AuthenticationErrors.IdentityUser.Creation(errors);
    }
}