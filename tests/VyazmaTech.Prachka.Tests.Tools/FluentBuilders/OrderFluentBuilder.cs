using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

internal sealed class OrderFluentBuilder : AbstractFluentBuilder<Order>
{
    public OrderFluentBuilder WithId(Guid Id)
    {
        WithProperty(x => x.Id, Id);
        return this;
    }

    public OrderFluentBuilder WithQueue(Queue queue)
    {
        WithProperty(x => x.Queue, queue);
        return this;
    }

    public OrderFluentBuilder WithUser(User user)
    {
        WithProperty(x => x.User, user);
        return this;
    }

    public OrderFluentBuilder WithStatus(OrderStatus status)
    {
        WithProperty(x => x.Status, status);
        return this;
    }

    public OrderFluentBuilder WithCreationDate(DateOnly creationDate)
    {
        WithProperty(x => x.CreationDate, creationDate);
        WithProperty(x => x.CreationDateTime, creationDate.ToDateTime(TimeOnly.Parse("10:00"), DateTimeKind.Utc));
        return this;
    }

    public OrderFluentBuilder WithModification(DateTime time)
    {
        WithProperty(x => x.ModifiedOnUtc, time);
        return this;
    }

    public OrderFluentBuilder WithPrice(double price)
    {
        WithProperty(x => x.Price, Price.Create(price));
        return this;
    }

    public override Order Build()
    {
        if (Entity.Price is null)
            WithProperty(x => x.Price, Price.Default);

        return base.Build();
    }
}