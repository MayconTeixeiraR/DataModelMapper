using System.Linq.Expressions;
using System.Reflection;

namespace DataModel_Mapper.Extensions;

public static class ExpressionUtils
{
    public static MemberInfo GetPropertyMember(this Expression entityPropertyExpression)
    {
        var memberExpression = (entityPropertyExpression as LambdaExpression)?.Body as MemberExpression
                               ?? ((entityPropertyExpression as LambdaExpression)?.Body as UnaryExpression)?.Operand as MemberExpression;

        if (memberExpression == null)
            throw new Exception("Não foi possível obter um MemberExpression da expressão.");

        return memberExpression.Member;
    }
}

public static class DictionaryExtensions
{
    public static void AddOnListValue<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        _ = dictionary.TryGetValue(key, out var list);
        
        if (list == null)
        {
            list = new List<TValue>();
            dictionary.Add(key, list);
        }

        list.Add(value);
    }
}