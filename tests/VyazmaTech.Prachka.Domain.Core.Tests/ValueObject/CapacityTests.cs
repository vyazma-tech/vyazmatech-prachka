using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.ValueObject;

public sealed class CapacityTests
{
    [Fact]
    public void Create_ShouldThrow_WhenCapacityIsNegative()
    {
        Func<Capacity> action = () => Capacity.Create(-1);

        action.Should().Throw<UserInvalidInputException>();
    }

    [Fact]
    public void Create_ShouldNotThrow_WhenCapacityIsNonNegative()
    {
        Func<Capacity> action = () => Capacity.Create(0);

        action.Should().NotThrow<UserInvalidInputException>();
    }
}