using System.Security.Claims;

namespace VyazmaTech.Prachka.Application.Abstractions.Identity;

public interface ICurrentUserManager
{
    void Authenticate(ClaimsPrincipal principal);
}