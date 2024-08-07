#pragma warning disable IDE0008
using FluentAssertions;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.DomainEvents;

public sealed class QueueExpiredDomainEventTests
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

        var @event = new QueueExpiredDomainEvent(id);
        string json = JsonConvert.SerializeObject(@event, Settings);

        // Act
        IDomainEvent? result = JsonConvert.DeserializeObject<IDomainEvent>(json, Settings);

        // Assert
        result.Should().NotBeNull();
        var domainEvent = result.Should().BeOfType<QueueExpiredDomainEvent>();

        domainEvent.Which.Id.Should().Be(id);
    }
}