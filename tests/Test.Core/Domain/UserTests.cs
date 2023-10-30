using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Core.ValueObjects;
using FluentAssertions;
using LanguageExt.Common;
using Moq;
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
    public void RegisterUser_ShouldReturnFailureResult_WhenRegistrationDateIsInPast()
    {
        var dateTimeOffset = new DateTimeOffset(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 30,
            second: 0,
            offset: TimeSpan.FromHours(3));
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(dateTimeOffset);

        var registrationDate = new DateTime(year: 2022, month: 12, day: 31);
        Result<UserRegistrationDate> creationResult = UserRegistrationDate.Create(registrationDate,_dateTimeProvider.Object);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Match(
            success => success.Value.ToString().Should().Be(_dateTimeProvider.Object.UtcNow.ToString()),
            fail => fail.Message.Should().Be(DomainErrors.UserRegistrationDate.InThePast.Message));
    }
    
    [Fact]
    public void RegisterUser_ShouldReturnSuccessResult_WhenRegistrationDateIsNotInPast()
    {
        var dateTimeOffset = new DateTimeOffset(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 30,
            second: 0,
            offset: TimeSpan.FromHours(1.5));
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(dateTimeOffset);

        Result<UserRegistrationDate> creationResult = UserRegistrationDate.Create(
            DateTime.SpecifyKind(dateTimeOffset.Date, DateTimeKind.Utc),
            _dateTimeProvider.Object);

        creationResult.IsSuccess.Should().BeTrue();
        creationResult.Match(
            success => success.Value.ToString().Should().Be(DateTime.SpecifyKind(dateTimeOffset.Date, DateTimeKind.Utc).ToString()),
            fail => fail.Message.Should().Be(DomainErrors.UserRegistrationDate.InThePast.Message));
    }
}