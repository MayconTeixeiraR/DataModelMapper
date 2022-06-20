using System.Linq.Expressions;
using DataModel_Mapper.Queries;

namespace DataModel_Mapper.Builder;

internal class QueryBuilder<TEntity> :
    ICreatedQuery<TEntity>, 
    ISelectedQuery<TEntity>,
    IWheredQuery<TEntity>,
    IJoinedQuery<TEntity>,
    IOrderedQuery<TEntity>,
    IGroupedQuery<TEntity> where TEntity : class
{
    private const string InnerJoinOrientation = "INNER JOIN";
    private const string LeftJoinOrientation = "LEFT JOIN";
    private const string RightJoinOrientation = "RIGHT JOIN";

    private const string WhereOrientation = "WHERE";
    private const string AndOrientation = "AND";
    private const string OrOrientation = "OR";

    private const string OrderbyOrientationAsc = "ASC";
    private const string OrderbyOrientationDesc = "DESC";

    private readonly SqlBuilderDatabaseContext _context;
    private readonly IInternalQuery _query;

    private QueryBuilder(
        SqlBuilderDatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        var tableConfiguration = _context.GetTableConfiguration<TEntity>();
        _query = new Query(context.Options.Translator, tableConfiguration.TableName);
    }

    private QueryBuilder(
        SqlBuilderDatabaseContext context,
        IInternalQuery query)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _query = query ?? throw new ArgumentNullException(nameof(query));
    }

    public static ICreatedQuery<TEntity> BuildQuery(
        SqlBuilderDatabaseContext context)
    {
        return new QueryBuilder<TEntity>(context);
    }

    private void GetSelectablesFromExpression(NewExpression expression)
    {
        for (var i = 0; i < expression.Arguments.Count; i++)
        {
            // Parameter properties
            var argumentMemberExpression = (MemberExpression) expression.Arguments[i];

            var argumentPropertyToken = argumentMemberExpression.Member.MetadataToken;
            var columnConfiguration = _context.GetColumnConfiguration(argumentPropertyToken);

            // Result type properties
            var propertyMember = expression.Members[i];
            var propertyMemberPropertyToken = propertyMember.MetadataToken;

            _context.AddColumnConfiguration(propertyMemberPropertyToken, columnConfiguration);
            _query.AddSelectClause(columnConfiguration, propertyMember.Name);
        }
    }

    public ISelectedQuery<TResult> Select<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression) where TResult : class
    {
        GetSelectablesFromExpression(propertyExpression.Body as NewExpression);
        return new QueryBuilder<TResult>(_context, _query);
    }

    public IJoinedQuery<TResult> InnerJoin<TJoinEntity, TProperty, TResult>(
        IDataModel<TJoinEntity> joinEntity,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class
    {
        return Join(
            InnerJoinOrientation,
            joinEntity,
            joinSelector,
            mainSelector,
            resultSelector);
    }

    public IJoinedQuery<TResult> LeftJoin<TJoinEntity, TProperty, TResult>(
        IDataModel<TJoinEntity> joinEntity,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class
    {
        return Join(
            LeftJoinOrientation,
            joinEntity,
            joinSelector,
            mainSelector,
            resultSelector);
    }

    public IJoinedQuery<TResult> RightJoin<TJoinEntity, TProperty, TResult>(
        IDataModel<TJoinEntity> joinEntity,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class
    {
        return Join(
            RightJoinOrientation,
            joinEntity,
            joinSelector,
            mainSelector,
            resultSelector);
    }

    private IJoinedQuery<TResult> Join<TJoinEntity, TProperty, TResult>(
        string orientation,
        IDataModel<TJoinEntity> _,
        Expression<Func<TJoinEntity, TProperty>> joinSelector,
        Expression<Func<TEntity, TProperty>> mainSelector,
        Expression<Func<TJoinEntity, TEntity, TResult>> resultSelector)
        where TJoinEntity : class
        where TResult : class
    {
        _query.ClearSelectClauses();

        var memberExpression = (MemberExpression) mainSelector.Body;
        var propertyToken = memberExpression.Member.MetadataToken;

        var propertyColumnMember = _context.GetColumnConfiguration(propertyToken);

        var joinPropertyMember = ((MemberExpression)joinSelector.Body).Member;
        var joinedPropertyToken = joinPropertyMember.MetadataToken;

        var joinedPropertyColumnMember = _context.GetColumnConfiguration(joinedPropertyToken);

        _query.AddJoinClause(
            propertyColumnMember, joinedPropertyColumnMember, orientation);

        GetSelectablesFromExpression(resultSelector.Body as NewExpression);

        return new QueryBuilder<TResult>(_context, _query);
    }

    public IWheredQuery<TEntity> Where<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression, TResult result)
        => Where(WhereOrientation, propertyExpression, result);

    public IWheredQuery<TEntity> And<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression, TResult result)
        => Where(AndOrientation, propertyExpression, result);

    public IWheredQuery<TEntity> Or<TResult>(
        Expression<Func<TEntity, TResult>> propertyExpression, TResult result) 
        => Where(OrOrientation, propertyExpression, result);

    private IWheredQuery<TEntity> Where<TResult>(
        string orientation,
        Expression<Func<TEntity, TResult>> propertyExpression,
        TResult result)
    {
        var memberExpression = (MemberExpression)propertyExpression.Body;
        var propertyToken = memberExpression.Member.MetadataToken;

        var columnConfiguration = _context.GetColumnConfiguration(propertyToken);
        _query.AddWhereClause(columnConfiguration, result, orientation);

        return this;
    }

    public IGroupedQuery<TEntity> GroupBy<TGroupedProperties, TResult>(
        Expression<Func<TEntity, TGroupedProperties>> propertyExpression,
        Expression<Func<TGroupedProperties, TResult>> resultSelector)
    {
        _query.ClearSelectClauses();

        var propertiesExpression = propertyExpression.Body as NewExpression;

        for (var i = 0; i < propertiesExpression.Arguments.Count; i++)
        {
            var argumentMemberExpression = (MemberExpression) propertiesExpression.Arguments[i];

            var argumentPropertyToken = argumentMemberExpression.Member.MetadataToken;
            var columnConfiguration = _context.GetColumnConfiguration(argumentPropertyToken);

            // Result type properties
            var propertyMember = propertiesExpression.Members[i];
            var propertyMemberPropertyToken = propertyMember.MetadataToken;

            _context.AddColumnConfiguration(propertyMemberPropertyToken, columnConfiguration);
            _query.AddGroupByReference(columnConfiguration);
        }

        GetSelectablesFromExpression(resultSelector.Body as NewExpression);

        return this;
    }

    public IOrderedQuery<TEntity> OrderBy<TOrderedProperties>(Expression<Func<TEntity, TOrderedProperties>> propertyExpression)
    {
        return OrderBy(propertyExpression, OrderbyOrientationAsc);
    }

    public IOrderedQuery<TEntity> OrderByDescending<TOrderedProperties>(Expression<Func<TEntity, TOrderedProperties>> propertyExpression)
    {
        return OrderBy(propertyExpression, OrderbyOrientationDesc);
    }

    private IOrderedQuery<TEntity> OrderBy<TOrderedProperties>(Expression<Func<TEntity, TOrderedProperties>> propertyExpression, string orientation)
    {
        var propertiesExpression = propertyExpression.Body as NewExpression;

        foreach (var propertyArgument in propertiesExpression.Arguments)
        {
            var propertyMemberExpression = (MemberExpression)propertyArgument;

            var argumentPropertyToken = propertyMemberExpression.Member.MetadataToken;
            var columnConfiguration = _context.GetColumnConfiguration(argumentPropertyToken);

            // Result type properties
            //var propertyMember = expression.Members[i];
            //var propertyMemberPropertyToken = propertyMember.MetadataToken;

            //_context.AddColumnConfiguration(propertyMemberPropertyToken, columnConfiguration);
            _query.AddOrderByReference(columnConfiguration, orientation);
        }

        return this;
    }

    public IExternalQuery ToQuery()
    {
        _query.Build();
        return (IExternalQuery) _query;
    }
}