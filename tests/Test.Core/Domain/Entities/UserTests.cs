using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.User.Events;
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
    public void RegisterUser_ShouldReturnNotNullUser_AndRaiseDomainEvent()
    {
        var registrationDate = DateOnly.FromDateTime(DateTime.UtcNow);
        UserEntity user = UserClassData.Create();

        user.Should().NotBeNull();
        user.TelegramId.Value.Should().Be("1");
        user.CreationDate.Should().Be(registrationDate);
        user.ModifiedOn.Should().BeNull();
        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserRegisteredDomainEvent>();
    }
}