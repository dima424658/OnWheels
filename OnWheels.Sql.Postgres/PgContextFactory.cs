using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace OnWheels.Sql.Postgres;

public class PgContextFactory : IDesignTimeDbContextFactory<PgContext>
{
    public PgContext CreateDbContext(string[] args)
    {
        var services = new ServiceCollection();

        services.AddPostgresContext(options =>
        {

        });

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<SqlContext>() as PgContext ?? throw new Exception("PgContext is null!");
    }
}