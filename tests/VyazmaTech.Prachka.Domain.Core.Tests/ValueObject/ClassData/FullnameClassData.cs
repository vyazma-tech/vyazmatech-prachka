using System.Collections;

namespace VyazmaTech.Prachka.Domain.Core.Tests.ValueObject.ClassData;

internal sealed class FullnameClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["Bogdan Archangelsk 29"];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}