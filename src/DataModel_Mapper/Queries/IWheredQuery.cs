using DataModel_Mapper.Queryables;

namespace DataModel_Mapper.Queries;

public interface IWheredQuery<TEntity> : 
    IWhereableAndOrQuery<TEntity>,
    IGroupableQuery<TEntity>,
    IOrderableQuery<TEntity>
{

}