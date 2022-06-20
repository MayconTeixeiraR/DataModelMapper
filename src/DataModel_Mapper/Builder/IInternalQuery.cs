namespace DataModel_Mapper.Builder;

public interface IInternalQuery
{
    void AddSelectClause(DataModelInfo propertyColumnMember, string alias = null);

    void AddWhereClause(DataModelInfo propertyColumnMember, object param, string orientation);
        
    void AddJoinClause(
        DataModelInfo propertyColumnMember,
        DataModelInfo joinedPropertyColumnMember,
        string orientation);

    void AddGroupByReference(DataModelInfo propertyColumnMember);

    void AddOrderByReference(DataModelInfo propertyColumnMember, string orientation);

    void ClearSelectClauses();

    void Build();
}