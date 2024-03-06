using Mediator;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Core.PreProcessors;

public sealed class RequestLoggingPreprocessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : Result
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

        TResponse response = await next(message, cancellationToken);

        if (response.IsSuccess)
        {
            _logger.LogInformation("Completed request RequestName = {RequestName}", requestName);
        }
        else
        {
            using (LogContext.PushProperty("Error", response.Error, true))
            {
                _logger.LogError("Completed request RequestName = {RequestName} with error", requestName);
            }
        }

        return response;
    }
}