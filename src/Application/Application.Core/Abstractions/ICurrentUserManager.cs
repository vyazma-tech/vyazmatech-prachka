using System.Security.Claims;

namespace Application.Core.Abstractions;

public interface ICurrentUserManager
{
    void Authenticate(ClaimsPrincipal principal);
}