using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.User.Events;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Moq;
using Test.Core.Domain.ClassData;
using Xunit;

namespace Test.Core.Domain;

public class UserTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;

    public UserTests()
    {
        _dateTimeProvider = new Mock<IDateTimeProvider>();
    }

    [Fact]
    public void CreateUserRegistrationDate_ShouldReturnFailureResult_WhenRegistrationDateIsInPast()
    {
        var dateTimeOffset = new DateTime(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 30,
            second: 0);
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(dateTimeOffset);

        var registrationDate = new DateTime(year: 2022, month: 12, day: 31);
        Result<UserRegistrationDate> creationResult =
            UserRegistrationDate.Create(registrationDate, _dateTimeProvider.Object);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(DomainErrors.UserRegistrationDate.InThePast.Message);
    }

    [Fact]
    public void CreateUserRegistrationDate_ShouldReturnSuccessResult_WhenRegistrationDateIsNotInPast()
    {
        var dateTime = new DateTime(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 30,
            second: 0);
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(dateTime);

        Result<UserRegistrationDate> creationResult = UserRegistrationDate.Create(
            dateTime,
            _dateTimeProvider.Object);

        creationResult.IsSuccess.Should().BeTrue();
        creationResult.Value.Value.Should().Be(dateTime);
    }

    [Theory]
    [ClassData(typeof(TelegramIdClassData))]
    public void CreateUserTelegramId_ShouldReturnFailureResult_WhenTelegramIdIsInvalid(string id, string errorMessage)
    {
        Result<TelegramId> creationResult = TelegramId.Create(id);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void CreateUserTelegramId_ShouldReturnSuccessResult_WhenTelegramIdIsNumber()
    {
        const string id = "123456789";
        Result<TelegramId> creationResult = TelegramId.Create(id);

        creationResult.IsSuccess.Should().BeTrue();
        creationResult.Value.Value.Should().Be(id);
    }

    [Fact]
    public void RegisterUser_ShouldReturnSuccessResult_AndRaiseDomainEvent()
    {
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var user = new UserEntity(
            TelegramId.Create("1").Value,
            UserRegistrationDate.Create(DateTime.UtcNow, _dateTimeProvider.Object).Value);

        user.Should().NotBeNull();
        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserRegisteredDomainEvent>();
    }
}