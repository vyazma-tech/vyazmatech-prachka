using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.ValueObject;

public sealed class AssignmentDateTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new();

    [Fact]
    public void Create_ShouldThrow_WhenAssignmentDateIsInThePast()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());

        Func<AssignmentDate> action = () => AssignmentDate.Create(
            _dateTimeProvider.Object.DateNow.AddDays(-1),
            _dateTimeProvider.Object.DateNow);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Create_ShouldThrow_WhenAssignmentDateIsNotNextWeek()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());

        Func<AssignmentDate> action = () => AssignmentDate.Create(
            _dateTimeProvider.Object.DateNow,
            _dateTimeProvider.Object.DateNow.AddDays(AssignmentDate.Week + 1));

        action.Should().Throw<DomainInvalidOperationException>();
    }
}