using Domain.Core.User;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Mapping;

public static class UserMapping
{
    public static UserEntity MapTo(UserModel model)
    {
        return new UserEntity(model.Id, model.TelegramId, model.Fullname, model.RegistrationDate, model.ModifiedOn);
    }

    public static UserModel MapFrom(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            Fullname = entity.Fullname,
            TelegramId = entity.TelegramId,
            RegistrationDate = entity.CreationDate,
            ModifiedOn = entity.ModifiedOn
        };
    }
}