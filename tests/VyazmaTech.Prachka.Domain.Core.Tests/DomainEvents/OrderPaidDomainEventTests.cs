#pragma warning disable IDE0008
using FluentAssertions;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.DomainEvents;

public sealed class OrderPaidDomainEventTests
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
    };

    [Fact]
    public void DeserializeObject_Should_DeserializeAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        const double price = 180.0;
        const string fullName = "Bobby Shmurda";

        var @event = new OrderPaidDomainEvent(id, price, fullName);
        string json = JsonConvert.SerializeObject(@event, Settings);

        // Act
        IDomainEvent? result = JsonConvert.DeserializeObject<IDomainEvent>(json, Settings);

        // Assert
        result.Should().NotBeNull();
        var domainEvent = result.Should().BeOfType<OrderPaidDomainEvent>();

        domainEvent.Which.Id.Should().Be(id);
        domainEvent.Which.Price.Should().Be(price);
        domainEvent.Which.Fullname.Should().Be(fullName);
    }
}