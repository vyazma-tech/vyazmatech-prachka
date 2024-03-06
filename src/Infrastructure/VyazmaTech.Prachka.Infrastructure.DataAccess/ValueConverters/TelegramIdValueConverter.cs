using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.ValueConverters;

public sealed class TelegramIdValueConverter : ValueConverter<TelegramId, string>
{
    public TelegramIdValueConverter()
        : base(
            telegramId => telegramId.Value,
            stringValue => TelegramId.Create(stringValue).Value)
    {
    }
}