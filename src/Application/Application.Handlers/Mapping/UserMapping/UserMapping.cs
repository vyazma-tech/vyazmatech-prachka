using Application.Handlers.User.CreateUser;
using Application.Handlers.User.Queries;
using Domain.Common.Errors;
using Domain.Core.User;
using Domain.Core.ValueObjects;

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
    
    public static CreateUserResponseModel ToCreationDto(this UserEntity userEntity)
    {
        return new CreateUserResponseModel
        {
            Id = userEntity.Id,
            TelegramId = userEntity.TelegramId.Value,
            Fullname = userEntity.Fullname.Value,
            ModifiedOn = userEntity.ModifiedOn,
            RegistrationDate = userEntity.CreationDate,
        };
    }
}