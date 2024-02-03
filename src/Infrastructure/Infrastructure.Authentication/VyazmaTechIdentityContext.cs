using Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication;

internal sealed class VyazmaTechIdentityContext : IdentityDbContext<VyazmaTechIdentityUser, VyazmaTechIdentityRole, Guid>
{
    public VyazmaTechIdentityContext(DbContextOptions<VyazmaTechIdentityContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}