using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Queryables;

public interface IWhereableQuery<TEntity> : IQuery
{
    IWheredQuery<TEntity> Where<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression,
        TResult result);
}