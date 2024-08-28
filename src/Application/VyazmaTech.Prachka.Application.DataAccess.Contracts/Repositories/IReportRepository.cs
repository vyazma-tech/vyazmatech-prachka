using VyazmaTech.Prachka.Application.Dto.Reports;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IReportRepository
{
    Task<IReadOnlyList<ReportModel>> GetReports(DateOnly From, DateOnly To);
}