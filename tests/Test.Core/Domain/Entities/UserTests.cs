using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using FluentAssertions;
using Moq;
using Test.Core.Domain.Entities.ClassData;
using Xunit;

namespace Test.Core.Domain.Entities;

public class UserTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new ();

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