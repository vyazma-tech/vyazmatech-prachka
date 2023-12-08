namespace Infrastructure.DataAccess.Quering.Abstractions;

public record QueryConfiguration<T>(IReadOnlyCollection<QueryParameter<T>> parameters);
