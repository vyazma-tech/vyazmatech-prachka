using System.Collections;

namespace VyazmaTech.Prachka.Domain.Core.Tests.ValueObject.ClassData;

public sealed class TelegramUsernameClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["awesome id666"];
        yield return [string.Empty];
        yield return ["id12345689"];
        yield return ["939C07BB-4773-41C8-9237-E71DE6F48A95"];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}