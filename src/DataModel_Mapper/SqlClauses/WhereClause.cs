using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.SqlClauses;

public record WhereClause(
    DataModelInfo PropertyColumnMember, 
    object Param, 
    string Orientation) : IClause;