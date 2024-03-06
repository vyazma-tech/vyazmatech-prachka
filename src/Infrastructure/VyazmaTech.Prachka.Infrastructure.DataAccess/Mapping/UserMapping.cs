using VyazmaTech.Prachka.Domain.Core.User;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;

public static class UserMapping
{
    public static UserEntity MapTo(UserModel model)
    {
        return new UserEntity(model.Id, model.TelegramUsername, model.Fullname, model.RegistrationDate, model.ModifiedOn);
    }

    public static UserModel MapFrom(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            Fullname = entity.Fullname,
            TelegramUsername = entity.TelegramUsername,
            RegistrationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOn
        };
    }
}