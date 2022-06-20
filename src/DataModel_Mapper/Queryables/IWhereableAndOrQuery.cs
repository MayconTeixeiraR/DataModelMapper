using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Queryables;

public interface IWhereableAndOrQuery<TEntity> : IQuery
{
    IWheredQuery<TEntity> And<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression,
        TResult result);

    IWheredQuery<TEntity> Or<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression,
        TResult result);
}