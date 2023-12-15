using Application.Handlers.Queue.Queries;
using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Queries.Links;

public class QueueIdQueryLink : QueryLinkBase<QueueQuery.QueryBuilder, QueueQueryParameter>
{
    private readonly ILogger<QueueIdQueryLink> _logger;

    public QueueIdQueryLink(ILogger<QueueIdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override QueueQuery.QueryBuilder? TryApply(
        QueueQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<QueueQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not QueueQueryParameter.QueueId)
            return null;

        try
        {
            return requestQueryBuilder.WithQueueId(Guid.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to guid", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to guid", e);
        }
    }
}