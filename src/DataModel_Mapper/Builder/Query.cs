using DataModel_Mapper.Extensions;
using DataModel_Mapper.SqlClauses;
using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.Builder;

internal class Query : IInternalQuery, IExternalQuery
{
    private readonly ISqlTranslator _translator;

    private readonly string _fromTable;

    private readonly IDictionary<string, List<IClause>> _clauses;

    public string Text { get; private set; }

    public Query(ISqlTranslator translator, string fromTable)
    {
        _translator = translator ?? throw new ArgumentNullException(nameof(translator));

        _fromTable = fromTable;
        _clauses = new Dictionary<string, List<IClause>>();
    }

    public void AddSelectClause(DataModelInfo propertyColumnMember, string? alias)
        => _clauses.AddOnListValue("SELECT", new SelectClause(propertyColumnMember, alias ?? propertyColumnMember.ColumnName));
    
    public void AddWhereClause(DataModelInfo propertyColumnMember, object param, string orientation)
        => _clauses.AddOnListValue("WHERE", new WhereClause(propertyColumnMember, param, orientation));

    public void AddJoinClause(
        DataModelInfo propertyColumnMember, 
        DataModelInfo joinedPropertyColumnMember, 
        string orientation)
        => _clauses.AddOnListValue("JOIN", new JoinClause(propertyColumnMember, joinedPropertyColumnMember, orientation));

    public void AddGroupByReference(DataModelInfo propertyColumnMember)
        => _clauses.AddOnListValue("GROUP_BY", new GroupByClause(propertyColumnMember));

    public void AddOrderByReference(DataModelInfo propertyColumnMember, string orientation)
        => _clauses.AddOnListValue("ORDER_BY", new OrderByClause(propertyColumnMember, orientation));

    public void ClearSelectClauses()
        => _clauses["SELECT"] = new List<IClause>();

    public void Build()
    {
        Text = _translator.Translate(_fromTable, _clauses);
    }
}