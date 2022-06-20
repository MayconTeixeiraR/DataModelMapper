using DataModel_Mapper.Queryables;

namespace DataModel_Mapper.Queries;

public interface ISelectedQuery<TEntity> : 
    IWhereableQuery<TEntity>, 
    IOrderableQuery<TEntity>
{

}