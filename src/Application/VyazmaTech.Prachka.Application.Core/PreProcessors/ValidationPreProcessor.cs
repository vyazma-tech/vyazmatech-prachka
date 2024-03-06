using FluentValidation;
using FluentValidation.Results;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Core.PreProcessors;

public sealed class ValidationPreProcessor<TRequest, TResponse> : MessagePreProcessor<TRequest, TResponse>
    where TRequest : IValidatableRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPreProcessor(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    protected override ValueTask Handle(TRequest message, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(message);

        List<ValidationFailure> failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return ValueTask.CompletedTask;
    }
}