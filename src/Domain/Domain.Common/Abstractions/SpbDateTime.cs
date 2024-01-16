namespace Domain.Common.Abstractions;

public readonly record struct SpbDateTime(DateTime Value)
{
    public static SpbDateTime FromDateOnly(DateOnly dateOnly)
    {
        return new SpbDateTime(dateOnly.ToDateTime(TimeOnly.MinValue));
    }

    public DateOnly AsDateOnly()
    {
        return new DateOnly(Value.Year, Value.Month, Value.Day);
    }

    public TimeOnly AsTimeOnly()
    {
        return new TimeOnly(Value.Hour, Value.Minute, Value.Second, Value.Millisecond, Value.Microsecond);
    }

    public override string ToString()
    {
        return Value.ToString("dd.MM.yyyy");
    }
}