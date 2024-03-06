using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Tests.Entities.ClassData;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Tests.Entities;

public class UserTests
{
    [Theory]
    [ClassData(typeof(TelegramIdClassData))]
    public void CreateUserTelegramId_ShouldReturnFailureResult_WhenTelegramIdIsInvalid(string id, Error error)
    {
        Result<TelegramId> creationResult = TelegramId.Create(id);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Should().Be(error);
    }

    [Fact]
    public void CreateUserTelegramId_ShouldReturnSuccessResult_WhenTelegramIdIsNumber()
    {
        const string id = "123456789";
        Result<TelegramId> creationResult = TelegramId.Create(id);

        creationResult.IsSuccess.Should().BeTrue();
        creationResult.Value.Value.Should().Be(id);
    }
}