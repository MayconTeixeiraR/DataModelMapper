using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.SqlClauses;

public record SelectClause(
    DataModelInfo PropertyColumnMember, 
    string Alias) : IClause;