using DataModel_Mapper.Configuration.Abstractions;

namespace DataModel_Mapper.Configuration;

public abstract class DataModelMapper<TDataModel>
{
    public void ConfigureMapping(SqlBuilderDatabaseContext databaseContext)
    {
        var contextEntityConfiguration = new DataModelConfiguration<TDataModel>(databaseContext);
        Configure(contextEntityConfiguration);
    }
        
    protected abstract void Configure(IDataModelConfiguration<TDataModel> configuration);
}