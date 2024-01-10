using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Contracts;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByTelegramIdSpecification : Specification<UserEntity>
{
    private readonly TelegramId _telegramId;

    public UserByTelegramIdSpecification(TelegramId telegramId)
        : base(user => user.TelegramId == telegramId)
    {
        _telegramId = telegramId;
    }

    public override string ToString()
        => $"UserTelegramId = {_telegramId.Value}";
}