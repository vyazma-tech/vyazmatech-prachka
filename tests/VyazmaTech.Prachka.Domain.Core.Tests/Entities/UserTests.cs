using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Tests.Entities.ClassData;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.Entities;

public class UserTests
{
    [Theory]
    [ClassData(typeof(TelegramIdClassData))]
    public void CreateUserTelegramId_ShouldThrow_WhenTelegramIdIsInvalid(string id)
    {
        Func<TelegramUsername> action = () => TelegramUsername.Create(id);

        action.Should().Throw<UserInvalidInputException>();
    }

    [Fact]
    public void CreateUserTelegramId_ShouldReturnSuccessResult_WhenTelegramIdIsNumber()
    {
        const string id = "123456789";
        var telegramId = TelegramUsername.Create(id);

        telegramId.Value.Should().Be(id);
    }
}