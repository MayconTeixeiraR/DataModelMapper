using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Queryables;

public interface IOrderableQuery<TEntity> : IQuery
{
    IOrderedQuery<TEntity> OrderBy<TOrderedProperties>(
        Expression<Func<TEntity, TOrderedProperties>> propertyExpression);

    IOrderedQuery<TEntity> OrderByDescending<TOrderedProperties>(
        Expression<Func<TEntity, TOrderedProperties>> propertyExpression);
}