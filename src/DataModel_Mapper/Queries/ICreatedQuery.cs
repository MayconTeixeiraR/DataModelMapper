using DataModel_Mapper.Queryables;

namespace DataModel_Mapper.Queries;

public interface ICreatedQuery<TEntity> : 
    ISelectableQuery<TEntity>, 
    IJoinableQuery<TEntity>,
    IGroupableQuery<TEntity>,
    IOrderedQuery<TEntity>
{
        
}