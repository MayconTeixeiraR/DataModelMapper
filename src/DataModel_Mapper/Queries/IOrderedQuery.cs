using DataModel_Mapper.Queryables;

namespace DataModel_Mapper.Queries;

public interface IOrderedQuery<TEntity> : 
    IOrderableQuery<TEntity>
{

}