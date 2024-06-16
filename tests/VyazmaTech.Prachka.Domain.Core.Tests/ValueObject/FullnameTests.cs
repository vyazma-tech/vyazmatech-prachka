using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Tests.ValueObject.ClassData;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.ValueObject;

public sealed class FullnameTests
{
    [Theory]
    [ClassData(typeof(FullnameClassData))]
    public void Create_ShouldThrow_WhenFullnameIsInvalid(string fullname)
    {
        Func<Fullname> action = () => Fullname.Create(fullname);

        action.Should().Throw<UserInvalidInputException>();
    }

    [Fact]
    public void Create_ShouldNotThrow_WhenFullnameStartWithUppercaseAndContainOnlyLetters()
    {
        string fullname = "Kostyuk Ivan Aleksandorvich";
        var result = Fullname.Create(fullname);

        result.Value.Should().Be(fullname);
    }
}