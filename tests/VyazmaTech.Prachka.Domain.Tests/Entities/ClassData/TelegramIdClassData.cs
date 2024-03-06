using System.Collections;
using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Domain.Tests.Entities.ClassData;

public sealed class TelegramIdClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "awesome id666", DomainErrors.TelegramId.InvalidFormat };
        yield return new object[] { string.Empty, DomainErrors.TelegramId.NullOrEmpty };
        yield return new object[] { "id12345689", DomainErrors.TelegramId.InvalidFormat };
        yield return new object[] { "939C07BB-4773-41C8-9237-E71DE6F48A95", DomainErrors.TelegramId.InvalidFormat };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}