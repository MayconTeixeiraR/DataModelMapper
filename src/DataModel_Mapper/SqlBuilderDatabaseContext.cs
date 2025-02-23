using System.Collections.Concurrent;
using DataModel_Mapper.Builder;
using DataModel_Mapper.Configuration;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper;

public abstract class SqlBuilderDatabaseContext
{
    private readonly ConcurrentDictionary<Type, TableConfiguration> _tableConfiguration;
    private readonly ConcurrentDictionary<int, DataModelInfo> _columnConfiguration;

    internal SqlBuilderContextOptions Options { get; }

    protected SqlBuilderDatabaseContext(SqlBuilderContextOptions options)
    {
        _tableConfiguration = new ConcurrentDictionary<Type, TableConfiguration>();
        _columnConfiguration = new ConcurrentDictionary<int, DataModelInfo>();

        Options = options;
    }
    
    public ICreatedQuery<TEntity> Set<TEntity>() where TEntity : class
    {
        return QueryBuilder<TEntity>.BuildQuery(this);
    }

    public void AddTableConfiguration(TableConfiguration tableConfiguration)
    {
        var hasConfiguration = _tableConfiguration.TryGetValue(tableConfiguration.EntityType, out _);
        if (hasConfiguration) throw new Exception("There is already a table configuration set up for this entity.");

        _tableConfiguration.AddOrUpdate(
            tableConfiguration.EntityType, 
            tableConfiguration,
            (_, tc) =>
            {
                if (tc.TableName != tableConfiguration.TableName) 
                    throw new ArgumentException(nameof(tc.TableName));
                
                return tableConfiguration;
            });
    }

    public TableConfiguration GetTableConfiguration<TTableEntity>()
    {
        var hasConfiguration = _tableConfiguration.TryGetValue(typeof(TTableEntity), out var tableConfiguration);
        if (hasConfiguration == false) throw new Exception("There is no table configuration set up for this entity.");

        return tableConfiguration;
    }

    public void AddColumnConfiguration(int propertyMetadataToken, DataModelInfo member)
    {
        if (_columnConfiguration.ContainsKey(propertyMetadataToken)) return;

        _columnConfiguration.AddOrUpdate(
            propertyMetadataToken,
            member,
            (_, mb) =>
            {
                if (mb.Property.MetadataToken != member.Property.MetadataToken)
                    throw new ArgumentException(nameof(mb.Property.MetadataToken));

                return member;
            });

    }

    public DataModelInfo GetColumnConfiguration(int propertyToken)
    {
        if (propertyToken == default)
            throw new Exception("The specified token is empty.");

        var columnConfigurationFound = _columnConfiguration.TryGetValue(propertyToken, out var columnConfiguration);

        if (columnConfigurationFound == false)
            throw new Exception($"The configuration for the token column {propertyToken} was not found.");

        return columnConfiguration;
    }

    public abstract void ConfigureMapping();

    public void RegisterMap<TEntity, TEntityMapping>()
        where TEntityMapping : DataModelMapper<TEntity>, new() => new TEntityMapping().ConfigureMapping(this);
}