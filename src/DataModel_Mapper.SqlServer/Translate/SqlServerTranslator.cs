using System.Text;
using DataModel_Mapper.Builder;
using DataModel_Mapper.SqlClauses;
using DataModel_Mapper.SqlClauses.Abstractions;

namespace DataModel_Mapper.SqlServer.Translate
{
    internal class SqlServerTranslator : ISqlTranslator
    {
        public string Translate(string fromTable, IDictionary<string, List<IClause>> clauses)
        {
            var query = new StringBuilder();
            
            Select(query, clauses);
            FromOf(query, fromTable);
            Join(query, clauses);
            Where(query, clauses);
            GroupBy(query, clauses);
            OrderBy(query, clauses);

            return query.ToString();
        }

        private static void Append(StringBuilder query, string @string) => query.Append(@string);        

        // ReSharper disable once MethodOverloadWithOptionalParameter
        private static void Append(StringBuilder query, string @string, bool onNewLine = false, bool withComma = false, short tabSize = 0)
        {
            if (onNewLine) query.AppendLine();
            
            if (tabSize > 0) query.Append(" ".PadRight(tabSize));
            if (withComma) query.Append(",");

            query.Append(@string);
        }
        
        private static void AppendTable(
            StringBuilder query, DataModelInfo propertyColumnMember)
        {
            Append(query, "[");
            Append(query, propertyColumnMember.TableConfiguration.TableName);
            Append(query, "]");
        }

        private static void AppendColumn(
            StringBuilder query, DataModelInfo propertyColumnMember)
        {
            Append(query, "[");
            Append(query, propertyColumnMember.ColumnName);
            Append(query, "]");
        }

        private static void AppendTableColumn(
            StringBuilder query, DataModelInfo propertyColumnMember, bool onNewLine = false, bool withComma = false, short tabSize = 0)
        {
            if (onNewLine) query.AppendLine();
            if (tabSize > 0) query.Append(" ".PadRight(tabSize));
            if (withComma) query.Append(",");

            AppendTable(query, propertyColumnMember);
            Append(query, ".");
            AppendColumn(query, propertyColumnMember);
        }

        private static void AppendParam(StringBuilder query, DataModelInfo propertyColumnMember)
        {
            Append(query, "@");
            Append(query, propertyColumnMember.TableConfiguration.TableName);
            Append(query, propertyColumnMember.ColumnName);
        }

        private static void Select(
            StringBuilder query, IDictionary<string, List<IClause>> clauses)
        {
            var hasSelectClauses = clauses.TryGetValue("SELECT", out var selectClauses);
            if (hasSelectClauses == false || selectClauses == null || selectClauses.Count == 0) return;

            Append(query, "SELECT");

            var needComma = false;
            foreach (var clause in selectClauses)
            {
                if (clause is not SelectClause selectClause) continue;

                AppendTableColumn(query, selectClause.PropertyColumnMember, onNewLine: true, withComma: needComma, tabSize: 3);
                
                Append(query, " AS [");
                Append(query, selectClause.Alias);
                Append(query, "]");

                needComma = true;
            }
        }

        private static void FromOf(StringBuilder query, string fromTable)
        {
            Append(query, "FROM", onNewLine: true);
            Append(query, " [");
            Append(query, fromTable);
            Append(query, "]");
        }

        private static void Where(
            StringBuilder query, IDictionary<string, List<IClause>> clauses)
        {
            var hasWhereClauses = clauses.TryGetValue("WHERE", out var whereClauses);
            if (hasWhereClauses == false || whereClauses == null || whereClauses.Count == 0) return;

            foreach (var clause in whereClauses)
            {
                if (clause is not WhereClause whereClause) continue;

                Append(query, whereClause.Orientation, onNewLine: true);
                Append(query, " ");
                AppendTableColumn(query, whereClause.PropertyColumnMember);
                Append(query, " = ");
                AppendParam(query, whereClause.PropertyColumnMember);
            }
        }

        private static void Join(
            StringBuilder query, IDictionary<string, List<IClause>> clauses)
        {
            var hasJoinClauses = clauses.TryGetValue("JOIN", out var joinClauses);
            if (hasJoinClauses == false || joinClauses == null || joinClauses.Count == 0) return;

            foreach (var clause in joinClauses)
            {
                if (clause is not JoinClause joinClause) continue;

                Append(query, joinClause.Orientation, onNewLine: true);
                Append(query, " ");
                AppendTable(query, joinClause.JoinedPropertyColumnMember);
                Append(query, " ON ");
                AppendTableColumn(query, joinClause.JoinedPropertyColumnMember);
                Append(query, " = ");
                AppendTableColumn(query, joinClause.PropertyColumnMember);
            }
        }

        private static void GroupBy(
            StringBuilder query, IDictionary<string, List<IClause>> clauses)
        {
            var hasGroupByClauses = clauses.TryGetValue("GROUP_BY", out var groupByClauses);
            if (hasGroupByClauses == false || groupByClauses == null || groupByClauses.Count == 0) return;

            Append(query, "GROUP BY", onNewLine: true);

            var needComma = false;
            foreach (var clause in groupByClauses)
            {
                if (clause is not GroupByClause groupByClause) continue;

                Append(query, " ", withComma: needComma);
                AppendTableColumn(query, groupByClause.PropertyColumnMember);

                needComma = true;
            }
        }

        private static void OrderBy(
            StringBuilder query, IDictionary<string, List<IClause>> clauses)
        {
            var hasOrderByClauses = clauses.TryGetValue("ORDER_BY", out var orderByClauses);
            if (hasOrderByClauses == false || orderByClauses == null || orderByClauses.Count == 0) return;

            Append(query, "ORDER BY", onNewLine: true);

            var needComma = false;
            foreach (var clause in orderByClauses)
            {
                if (clause is not OrderByClause orderByClause) continue;

                Append(query, " ", withComma: needComma);
                AppendTableColumn(query, orderByClause.PropertyColumnMember);

                Append(query, " ");
                Append(query, orderByClause.Orientation);

                needComma = true;
            }
        }
    }
}
