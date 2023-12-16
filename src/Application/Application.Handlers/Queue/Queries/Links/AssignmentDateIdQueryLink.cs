using Application.Handlers.Queue.Queries;
using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Queries.Links;

public class AssignmentDateQueryLink : QueryLinkBase<QueueQuery.QueryBuilder, QueueQueryParameter>
{
    private readonly ILogger<AssignmentDateQueryLink> _logger;

    public AssignmentDateQueryLink(ILogger<AssignmentDateQueryLink> logger)
    {
        _logger = logger;
    }

    protected override QueueQuery.QueryBuilder? TryApply(
        QueueQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<QueueQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not QueueQueryParameter.AssignmentDate)
            return null;

        try
        {
            return requestQueryBuilder.WithAssignmentDate(
                DateOnly.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to date time", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to guid", e);
        }
    }
}