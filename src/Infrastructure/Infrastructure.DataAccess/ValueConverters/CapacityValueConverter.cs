using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public sealed class CapacityValueConverter : ValueConverter<Capacity, int>
{
    public CapacityValueConverter()
        : base(
            capacity => capacity.Value,
            value => Capacity.Create(value).Value)
    {
    }
}