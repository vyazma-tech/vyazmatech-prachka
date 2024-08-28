using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Identity;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Services;

internal sealed class BanUserService : IBanUserService
{
    private readonly VyazmaTechIdentityContext _context;

    public BanUserService(VyazmaTechIdentityContext context)
    {
        _context = context;
    }

    public Task BanUser(string username, string initiator)
    {
        var model = new BannedUser.BannedUser
        {
            User = username,
            BannedBy = initiator,
            BannedAt = DateTime.UtcNow
        };
        _context.BannedUsers.Add(model);

        return _context.SaveChangesAsync();
    }

    public Task UnbanUser(string username)
    {
        return _context.BannedUsers
            .Where(x => x.User == username)
            .ExecuteDeleteAsync();
    }

    public bool IsBanned(string username)
    {
        return Wrapper().GetAwaiter().GetResult();

        async Task<bool> Wrapper()
            => await _context.BannedUsers.AnyAsync(x => x.User == username);
    }
}