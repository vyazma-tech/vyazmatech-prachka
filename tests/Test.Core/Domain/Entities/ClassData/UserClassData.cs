using System.Collections;
using Domain.Core.User;
using Domain.Core.ValueObjects;

namespace Test.Core.Domain.Entities.ClassData;

public static class UserClassData
{
    public static UserEntity Create()
    {
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            Fullname.Create("Test User").Value,
            DateOnly.FromDateTime(DateTime.UtcNow));
        return user;
    }
}