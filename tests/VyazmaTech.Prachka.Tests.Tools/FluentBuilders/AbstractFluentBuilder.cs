using System.Linq.Expressions;
using System.Reflection;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

internal abstract class AbstractFluentBuilder<T>
{
    protected readonly T? EntityWithRandomProperties;
    protected T Entity;

    protected AbstractFluentBuilder(bool initWithRandomProperties = false)
    {
        Entity = (T)Activator.CreateInstance(typeof(T), true)!;

        if (initWithRandomProperties)
            EntityWithRandomProperties = (T)GenFu.GenFu.New(typeof(T));
    }

    public AbstractFluentBuilder<T> WithProperty<TProp>(Expression<Func<T, TProp>> propertyExpression, TProp value)
    {
        if ((propertyExpression.Body as MemberExpression)?.Member is not PropertyInfo propertyInfo)
            throw new InvalidOperationException();

        SetValueInternal(propertyInfo, Entity, value);

        if (EntityWithRandomProperties is not null)
            SetValueInternal(propertyInfo, EntityWithRandomProperties, value);

        return this;
    }

    public TBuilder? CastBuilder<TBuilder>()
        where TBuilder : class
    {
        return this as TBuilder;
    }

    public virtual T Build()
    {
        return Entity;
    }

    public virtual T? BuildWithRandomProperties() => EntityWithRandomProperties;

    public AbstractFluentBuilder<T> WithCollection<TProp>(Expression<Func<T, TProp>> propertyExpression, TProp value)
    {
        if ((propertyExpression.Body as MemberExpression)?.Member is not PropertyInfo propertyInfo)
            throw new InvalidOperationException();

        SetCollectionInternal(propertyInfo, Entity, value);

        if (EntityWithRandomProperties is not null)
            SetCollectionInternal(propertyInfo, EntityWithRandomProperties, value);

        return this;
    }

    private void SetValueInternal<TProp>(PropertyInfo propertyInfo, T? entity, TProp value)
    {
        FieldInfo? backingField = propertyInfo.DeclaringType?.GetField(
            $"<{propertyInfo.Name}>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingField is null)
            throw new InvalidOperationException($"{propertyInfo.Name} backing field not found");

        backingField.SetValue(Entity, value);
    }

    private void SetCollectionInternal<TProp>(PropertyInfo propertyInfo, T? entity, TProp value)
    {
        FieldInfo? backingField = propertyInfo.DeclaringType?.GetField(
            $"_{propertyInfo.Name.ToLower()}",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingField is null)
            throw new InvalidOperationException($"{propertyInfo.Name} backing field not found");

        backingField.SetValue(Entity, value);
    }
}