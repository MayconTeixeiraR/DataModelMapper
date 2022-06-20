using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Queryables;

public interface IJoinableQuery<TEntity> : IQuery
{
    IJoinedQuery<TResult> InnerJoin<TJoinEntity, TProperty, TResult>(
        IDataModel<TJoinEntity> _,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class;

    IJoinedQuery<TResult> LeftJoin<TJoinEntity, TProperty, TResult>(
        IDataModel<TJoinEntity> _,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class;

    IJoinedQuery<TResult> RightJoin<TJoinEntity, TProperty, TResult>(
        IDataModel<TJoinEntity> _,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class;
}