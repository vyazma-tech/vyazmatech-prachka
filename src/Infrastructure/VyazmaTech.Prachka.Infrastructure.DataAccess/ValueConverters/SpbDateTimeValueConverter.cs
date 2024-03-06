using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Infrastructure.Tools;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.ValueConverters;

public sealed class SpbDateTimeValueConverter : ValueConverter<SpbDateTime, DateTime>
{
    public SpbDateTimeValueConverter()
        : base(
        spb => SpbDateTimeProvider.ToUtc(spb),
        dateTime => SpbDateTimeProvider.FromUtc(dateTime))
    {
    }
}