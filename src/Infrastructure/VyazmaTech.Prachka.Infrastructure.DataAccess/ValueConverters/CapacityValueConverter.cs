using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.ValueConverters;

public sealed class CapacityValueConverter : ValueConverter<Capacity, int>
{
    public CapacityValueConverter()
        : base(
            capacity => capacity.Value,
            value => Capacity.Create(value).Value)
    {
    }
}