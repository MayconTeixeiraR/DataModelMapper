using System.Linq.Expressions;
using DataModel_Mapper.Configuration.Abstractions;

namespace DataModel_Mapper.Configuration;

public class DataModelConfiguration<TEntity> : IDataModelConfiguration<TEntity>
{
    private TableConfiguration _tableConfiguration;
    private readonly SqlBuilderDatabaseContext _context;

    public DataModelConfiguration(SqlBuilderDatabaseContext context)
    {
        _context = context;
    }

    public void OfTable(string tableName)
    {
        _tableConfiguration = new TableConfiguration(typeof(TEntity), tableName);
        _context.AddTableConfiguration(_tableConfiguration);
    }

    public void MapColumn<TProperty>(
        Expression<Func<TEntity, TProperty>> propertyExpression, string columnName)
    {
        if (_tableConfiguration == null)
            throw new Exception("");
            
        var propertyMemberExpression = (MemberExpression) propertyExpression.Body;

        if (propertyMemberExpression == null)
            throw new Exception("Propriedade de entidade interna nula!");

        var member = new DataModelInfo(propertyMemberExpression.Member, columnName, _tableConfiguration);
        _context.AddColumnConfiguration(propertyMemberExpression.Member.MetadataToken, member);
    }
}