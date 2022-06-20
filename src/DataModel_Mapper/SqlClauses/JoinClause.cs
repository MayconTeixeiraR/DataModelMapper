using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.SqlClauses;

public record JoinClause(
    DataModelInfo PropertyColumnMember, 
    DataModelInfo JoinedPropertyColumnMember, 
    string Orientation) : IClause;