using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.Models;

internal class VyazmaTechIdentityRole : IdentityRole<Guid>
{
    public VyazmaTechIdentityRole(string roleName)
        : base(roleName)
    {
    }

    protected VyazmaTechIdentityRole()
    {
    }
}