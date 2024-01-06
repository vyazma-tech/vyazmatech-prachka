using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Core.Extensions;
using Application.DataAccess.Contracts;
using Domain.Core.Queue;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Application.Handlers.Queue.Queries.QueueByQuery.QueueQuery;

namespace Application.Handlers.Queue.Queries.QueueByQuery;

internal sealed class QueueQueryHandler : IQueryHandler<Query, PagedResponse<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;

    public QueueQueryHandler(
        IPersistenceContext persistenceContext,
        IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _persistenceContext = persistenceContext;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<PagedResponse<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryable<QueueEntity> query = _persistenceContext.Entities<QueueEntity>()
            .Skip(request.Page * _recordsPerPage)
            .Take(_recordsPerPage);

        List<QueueEntity> queues = await query.ToListAsync(cancellationToken);

        Response[] result = queues
            .FilterBy(request.AssignmentDate)
            .Select(x => x.ToDto())
            .ToArray();

        long totalPages = await _persistenceContext.Queues.CountAsync(cancellationToken);
        return new PagedResponse<Response>
        {
            Bunch = result,
            CurrentPage = request.Page + 1,
            RecordPerPage = _recordsPerPage,
            TotalPages = (totalPages / _recordsPerPage) + 1
        };
    }
}