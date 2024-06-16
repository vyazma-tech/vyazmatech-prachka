using Mediator;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Application.Core.PreProcessors;

public sealed class RequestLoggingPreprocessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly ILogger<RequestLoggingPreprocessor<TRequest, TResponse>> _logger;

    public RequestLoggingPreprocessor(ILogger<RequestLoggingPreprocessor<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(
        TRequest message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, TResponse> next)
    {
        string? requestName = typeof(TRequest).FullName;

        _logger.LogInformation("Processing request RequestName = {RequestName}", requestName);
        TResponse response = default!;

        try
        {
            response = await next(message, cancellationToken);
            _logger.LogInformation("Completed request RequestName = {RequestName}", requestName);

            return response;
        }
        catch (DomainException e)
        {
            using (LogContext.PushProperty("Error", e.Error, true))
            {
                _logger.LogError("Completed request RequestName = {RequestName} with error", requestName);
            }

            throw;
        }
    }
}