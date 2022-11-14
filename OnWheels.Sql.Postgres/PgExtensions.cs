using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OnWheels.Sql.Postgres;

public static class PgExtensions
{
    public static IServiceCollection AddPostgresContext(this IServiceCollection services)
    {
        services.AddDbContext<SqlContext, PgContext>();

        return services;
    }

    public static IServiceCollection AddPostgresContext(this IServiceCollection services, Action<PgOptions> options)
    {
        services.Configure(options);
        services.AddDbContext<SqlContext, PgContext>();

        return services;
    }

    public static async Task<IServiceProvider> ApplyPostgresMigrationsAsync(this IServiceProvider services, CancellationToken cancellationToken)
    {
        await using var scope = services.CreateAsyncScope();
        var pgContext = scope.ServiceProvider.GetRequiredService<PgContext>();

        await pgContext.Database.MigrateAsync(cancellationToken);

        return services;
    }

    public static IServiceProvider ApplyPostgresMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var pgContext = scope.ServiceProvider.GetRequiredService<PgContext>();

        pgContext.Database.Migrate();

        return services;
    }
}