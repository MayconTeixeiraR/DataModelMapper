using System.Linq.Expressions;

namespace DataModel_Mapper.Configuration.Abstractions;

public interface IDataModelConfiguration<TDataModel>
{
    public void OfTable(string tableName);
        
    public void MapColumn<TProperty>(
        Expression<Func<TDataModel, TProperty>> propertyExpression, string columnName);
}