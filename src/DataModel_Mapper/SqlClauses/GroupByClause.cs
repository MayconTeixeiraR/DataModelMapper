using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.SqlClauses;

public record GroupByClause(DataModelInfo PropertyColumnMember) : IClause;