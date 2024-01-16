using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class UserMapping
{
    public static UserEntity MapTo(UserModel model)
    {
        TelegramId telegramId = TelegramId.Create(model.TelegramId).Value;
        Fullname fullname = Fullname.Create(model.Fullname).Value;

        return new UserEntity(model.Id, telegramId, fullname, model.RegistrationDate, model.ModifiedOn);
    }

    public static UserModel MapFrom(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            Fullname = entity.Fullname.Value,
            TelegramId = entity.TelegramId.Value,
            RegistrationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOn
        };
    }
}