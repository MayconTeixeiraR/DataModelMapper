using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.SqlClauses;

public record OrderByClause(
    DataModelInfo PropertyColumnMember, 
    string Orientation) : IClause;