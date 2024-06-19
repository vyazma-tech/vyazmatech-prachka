using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

namespace VyazmaTech.Prachka.Application.Core.Users;

internal class CurrentUserProxy : ICurrentUser, ICurrentUserManager
{
    private ICurrentUser _user = new AnonymousUser();

    public Guid? Id => _user.Id;

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
    {
        return _user.CanChangeUserRole(currentRoleName, newRoleName);
    }

    public void Authenticate(ClaimsPrincipal principal)
    {
        var roles = principal.Claims
            .Where(x => x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        string nameIdentifier = principal.Claims
            .Single(x => x.Type.Equals(JwtRegisteredClaimNames.NameId, StringComparison.OrdinalIgnoreCase))
            .Value;

        if (!Guid.TryParse(nameIdentifier, out Guid id))
        {
            return;
        }

        if (roles.Contains(VyazmaTechRoleNames.AdminRoleName))
        {
            _user = new AdminUser(id);
        }
        else if (roles.Contains(VyazmaTechRoleNames.ModeratorRoleName))
        {
            _user = new ModeratorUser(id);
        }
        else if (roles.Contains(VyazmaTechRoleNames.EmployeeRoleName))
        {
            _user = new EmployeeUser(id);
        }
        else if (roles.Contains(VyazmaTechRoleNames.UserRoleName))
        {
            _user = new User(id);
        }
    }
}