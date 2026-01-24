using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;

namespace Movies.Application;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application Services Registration
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDBConnectionFactory>(_ => new SqlServerConnectionFactory(connectionString));
        // initialise dbinitializer class for tables creation
        services.AddSingleton<DBInitializer>();
        return services;
    }
}