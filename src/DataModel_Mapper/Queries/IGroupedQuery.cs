using DataModel_Mapper.Queryables;

namespace DataModel_Mapper.Queries;

public interface IGroupedQuery<TEntity> : 
    IOrderableQuery<TEntity>
{

}