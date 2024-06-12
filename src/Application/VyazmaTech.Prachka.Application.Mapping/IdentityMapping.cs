using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class IdentityMapping
{
    public static IdentityUserDto ToDto(this IdentityUserModel model, string role)
    {
        return new IdentityUserDto(
            model.Id,
            model.Fullname,
            role,
            model.TelegramUsername,
            model.TelegramImageUrl);
    }

    public static IdentityTokenDto ToDto(this IdentityTokenModel model)
    {
        return new IdentityTokenDto(model.AccessToken, model.RefreshToken);
    }
}