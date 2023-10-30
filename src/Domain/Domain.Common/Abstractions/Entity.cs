namespace Domain.Common.Abstractions;

public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents;

    protected Entity(Guid id)
    {
        Id = id;
        _domainEvents = new List<IDomainEvent>();
    }

#pragma warning disable CS8618
    protected Entity() { }
#pragma warning restore CS8618

    public Guid Id { get; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity)
            return false;

        return Equals(entity);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}