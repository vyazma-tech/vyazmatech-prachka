using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Reports;

public static class GetReport
{
    public record struct Query(DateOnly From, DateOnly To) : IQuery<Response>;

    public record struct Response(byte[] binaryReport);
}