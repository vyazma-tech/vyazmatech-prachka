using Domain.Common.Abstractions;
using Infrastructure.Tools;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public sealed class SpbDateTimeValueConverter : ValueConverter<SpbDateTime, DateTime>
{
    public SpbDateTimeValueConverter()
        : base(
        spb => SpbDateTimeProvider.ToUtc(spb),
        dateTime => SpbDateTimeProvider.FromUtc(dateTime))
    {
    }
}