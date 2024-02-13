namespace VyazmaTech.Prachka.Application.Core.Querying.Common;

public interface IQueryProcessor<in TQuery, TQueryBuilder>
{
    TQueryBuilder Process(TQuery query, TQueryBuilder builder);
}