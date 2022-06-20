using DataModel_Mapper.Queryables;

namespace DataModel_Mapper.Queries;

public interface IJoinedQuery<TEntity> : 
    IJoinableQuery<TEntity>, 
    IGroupableQuery<TEntity>,
    IWhereableQuery<TEntity>
{

}