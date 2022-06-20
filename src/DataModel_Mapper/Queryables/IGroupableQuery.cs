using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Queryables;

public interface IGroupableQuery<TEntity> : IQuery
{
    IGroupedQuery<TEntity> GroupBy<TGroupedProperties, TResult>(
        Expression<Func<TEntity, TGroupedProperties>> propertyExpression,
        Expression<Func<TGroupedProperties, TResult>> resultSelector);
}