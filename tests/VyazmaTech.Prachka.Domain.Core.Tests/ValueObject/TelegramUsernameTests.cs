using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Tests.ValueObject.ClassData;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.ValueObject;

public sealed class UserTests
{
    [Theory]
    [ClassData(typeof(TelegramUsernameClassData))]
    public void Create_ShouldThrow_WhenTelegramUsernameIsInvalid(string id)
    {
        Func<TelegramUsername> action = () => TelegramUsername.Create(id);

        action.Should().Throw<UserInvalidInputException>();
    }

    [Fact]
    public void Create_ShouldNotThrow_WhenTelegramUsernameStartsWithAtSymbol()
    {
        const string id = "@hiThereImUsingVyazmaTech.Prachka";
        var telegramId = TelegramUsername.Create(id);

        telegramId.Value.Should().Be(id);
    }
}