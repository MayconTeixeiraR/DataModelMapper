using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Queryables;

public interface ISelectableQuery<TEntity>
{
    ISelectedQuery<TResult> Select<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression) where TResult : class;
}