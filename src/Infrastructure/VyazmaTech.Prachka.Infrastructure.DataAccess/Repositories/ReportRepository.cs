using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Application.Dto.Reports;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal sealed class ReportRepository : IReportRepository
{
    private readonly DatabaseContext _context;

    public ReportRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ReportModel>> GetReports(DateOnly from, DateOnly to)
    {
        var dtFrom = from.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.Zero), DateTimeKind.Utc);
        var dtTo = to.ToDateTime(TimeOnly.Parse("23:59:59"), DateTimeKind.Utc);

        var reportGroups = await _context.Orders
            .Where(x => x.Status != OrderStatus.New)
            .Where(x => x.CreationDateTime >= dtFrom && x.CreationDateTime <= dtTo)
            .Select(x => new
            {
                x.User.Id,
                x.User.Fullname,
                x.Price,
                x.CreationDateTime,
                x.Status
            })
            .GroupBy(x => x.CreationDateTime.Month)
            .ToDictionaryAsync(
                x => x.Key,
                x => x.Select(li => new ReportLineItem(li.Id, li.Fullname, li.Price, li.CreationDateTime)));

        List<ReportModel> models = [];

        foreach (KeyValuePair<int, IEnumerable<ReportLineItem>> reportGroup in reportGroups)
        {
            var groupedByUser = reportGroup.Value
                .GroupBy(x => x.UserId)
                .Select(group => new ReportLineItem(
                    UserId: group.Key,
                    Fullname: group.First().Fullname,
                    OrderPrice: group.Sum(order => order.OrderPrice),
                    CreatedAt: group.First().CreatedAt))
                .ToList();

            var model = new ReportModel(groupedByUser);
            models.Add(model);
        }

        return models;
    }
}