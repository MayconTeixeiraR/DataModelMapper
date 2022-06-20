using DataModel_Mapper.SqlServer.Translate;
using Microsoft.Extensions.DependencyInjection;

namespace DataModel_Mapper.SqlServer.Extensions;

public static class ServicesCollectionExtensions
{
    public static void AddDatabaseContext<TContext>(this IServiceCollection services)
        where TContext : SqlBuilderDatabaseContext
    {
        services.AddSingleton(s =>
        {
            var contextOptions = new SqlBuilderContextOptions
            {
                Translator = new SqlServerTranslator()
            };

            if (Activator.CreateInstance(typeof(TContext), contextOptions) is not TContext instance) throw new Exception("");

            instance.ConfigureMapping();

            return instance;
        });
    }
}