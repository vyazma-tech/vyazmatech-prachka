using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Subscriptions;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.Entities;

public class SubscriptionTests
{
    [Fact]
    public void Subscribe_ShouldNotThrow_WhenOrderIsNotInSubscription()
    {
        var id = Guid.NewGuid();
        var subscription = new OrderSubscriptionEntity(
            Guid.Empty,
            Guid.Empty,
            default,
            Array.Empty<Guid>().ToHashSet());

        subscription.Subscribe(id);

        subscription.SubscribedOrders.Should().Contain(id);
    }

    [Fact]
    public void Unsubscribe_ShouldThrow_WhenUserOrderIsNotInSubscription()
    {
        var id = Guid.NewGuid();

        var subscription = new OrderSubscriptionEntity(
            Guid.Empty,
            Guid.Empty,
            default,
            Array.Empty<Guid>().ToHashSet());

        Action action = () => subscription.Unsubscribe(id);

        action.Should().Throw<DomainInvalidOperationException>();
    }
}