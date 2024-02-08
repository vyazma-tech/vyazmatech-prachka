using Application.Dto.Identity;
using Infrastructure.Authentication.Models;

namespace Infrastructure.Authentication.Mapping;

internal static class IdentityUserMapping
{
    public static IdentityUserDto ToDto(this VyazmaTechIdentityUser user)
    {
        return new IdentityUserDto(
            Id: user.Id,
            Fullname: user.Fullname,
            TelegramUsername: user.UserName ?? string.Empty,
            TelegramImageUrl: user.TelegramImageUrl);
    }
}