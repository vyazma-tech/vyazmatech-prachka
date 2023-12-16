using Application.Handlers.User.Queries;
using Domain.Core.User;

namespace Application.Handlers.Mapping.UserMapping;

public static class UserMapping
{
    public static UserResponseModel ToDto(this UserEntity userEntity)
    {
        return new UserResponseModel
        {
            Id = userEntity.Id,
            TelegramId = userEntity.TelegramId.Value,
            Fullname = userEntity.Fullname.Value,
            ModifiedOn = userEntity.ModifiedOn,
            CreationDate = userEntity.CreationDate,
        };
    }
}