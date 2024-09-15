using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class IdentityMapping
{
    public static IdentityUserDto ToDto(this IdentityUserModel model)
    {
        return new IdentityUserDto(
            model.Id,
            model.Fullname,
            model.TelegramUsername,
            model.TelegramImageUrl);
    }

    public static IdentityTokenDto ToDto(this IdentityTokenModel model, string role)
    {
        return new IdentityTokenDto(model.AccessToken, model.RefreshToken, role);
    }
}