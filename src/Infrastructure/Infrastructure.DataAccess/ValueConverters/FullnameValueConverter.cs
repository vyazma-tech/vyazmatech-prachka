using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public sealed class FullnameValueConverter : ValueConverter<Fullname, string>
{
    public FullnameValueConverter()
        : base(
            name => name.Value,
            value => Fullname.Create(value).Value)
    {
    }
}