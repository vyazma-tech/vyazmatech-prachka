namespace Domain.Common.Abstractions;

public abstract class ValueObject : IEquatable<ValueObject>
{
#pragma warning disable CS8618
    protected ValueObject() { }
#pragma warning restore CS8618
    public bool Equals(ValueObject? other)
    {
        return other is not null
               && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        if (!(obj is ValueObject valueObject))
            return false;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        HashCode hashCode = default;

        foreach (object obj in GetEqualityComponents())
        {
            hashCode.Add(obj);
        }

        return hashCode.ToHashCode();
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}