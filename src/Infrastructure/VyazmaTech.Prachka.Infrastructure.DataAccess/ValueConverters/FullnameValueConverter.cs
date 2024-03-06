using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.ValueConverters;

public sealed class FullnameValueConverter : ValueConverter<Fullname, string>
{
    public FullnameValueConverter()
        : base(
            name => name.Value,
            value => Fullname.Create(value).Value)
    {
    }
}