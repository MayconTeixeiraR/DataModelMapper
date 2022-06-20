using DataModel_Mapper.Builder;

namespace DataModel_Mapper.Queries;

public interface IQuery
{
    IExternalQuery ToQuery();
}