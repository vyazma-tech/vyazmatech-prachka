using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Queue;

public class QueuePageSpecification : Specification<QueueModel>
{
    public QueuePageSpecification(int page, int recordPerPage)
        : base(null!)
    {
        AddPaging(page, recordPerPage);
        AsNoTracking();
    }

    public override string ToString()
        => string.Empty;
}