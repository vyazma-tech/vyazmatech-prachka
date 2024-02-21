using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models;
using VyazmaTech.Prachka.Infrastructure.Authentication.Outbox;

namespace VyazmaTech.Prachka.Infrastructure.Authentication;

internal sealed class VyazmaTechIdentityContext
    : IdentityDbContext<VyazmaTechIdentityUser, VyazmaTechIdentityRole, Guid>
{
    public VyazmaTechIdentityContext(DbContextOptions<VyazmaTechIdentityContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<OutboxMessage> OutboxMessages { get; init; } = null!;
}