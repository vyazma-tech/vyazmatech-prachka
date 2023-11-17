using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public sealed class TelegramIdValueConverter : ValueConverter<TelegramId, string>
{
    public TelegramIdValueConverter()
        : base(
            telegramId => telegramId.Value,
            stringValue => TelegramId.Create(stringValue).Value)
    {
    }
}