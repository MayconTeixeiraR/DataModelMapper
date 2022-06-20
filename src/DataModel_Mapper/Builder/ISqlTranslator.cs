using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.Builder;

public interface ISqlTranslator
{
    string Translate(string fromTable, IDictionary<string, List<IClause>> clauses);
}