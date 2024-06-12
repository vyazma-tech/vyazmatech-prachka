using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;

public static class UserMapping
{
    public static User MapTo(UserModel model)
    {
        return new User(
            model.Id,
            model.TelegramUsername,
            model.Fullname,
            model.RegistrationDate,
            model.ModifiedOn);
    }

    public static UserModel MapFrom(User entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            Fullname = entity.Fullname,
            TelegramUsername = entity.TelegramUsername,
            RegistrationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOnUtc,
        };
    }
}