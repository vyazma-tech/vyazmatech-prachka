using System.Collections;

namespace Test.Core.Domain.Entities.ClassData;

public sealed class TelegramIdClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "awesome id666", "Telegram ID should be a number." };
        yield return new object[] { "", "Telegram ID should not be null or empty." };
        yield return new object[] { "id12345689", "Telegram ID should be a number." };
        yield return new object[] { "939C07BB-4773-41C8-9237-E71DE6F48A95", "Telegram ID should be a number." };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}