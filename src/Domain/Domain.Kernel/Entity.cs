namespace Domain.Kernel;

/// <summary>
/// Base entity class. All entities should be inherited from it.
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents = new ();

    protected Entity(Guid id)
    {
        Id = id;
    }

#pragma warning disable CS8618
    protected Entity() { }
#pragma warning restore CS8618

    /// <summary>
    /// Gets unique identifier of an entity.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets domain event list for a current entity.
    /// </summary>
    public virtual IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

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

    /// <summary>
    /// Raises domain event for an entity.
    /// </summary>
    /// <param name="domainEvent">domain event object.</param>
    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears domain events. Should be called <b>only</b>,
    /// when domain events are about to publish.
    /// </summary>
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