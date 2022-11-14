using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace OnWheels.Sql.Postgres;

public class PgContext : SqlContext
{
    protected readonly PgOptions Options;

    public PgContext(DbContextOptions<PgContext> contextOptions, IOptions<PgOptions> pgOptions)
        : base(contextOptions)
    {
        Options = pgOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new Npgsql.NpgsqlConnectionStringBuilder()
        {
            Host = Options.Host,
            Port = Options.Port,
            Database = Options.Database,
            Username = Options.Username,
            Password = Options.Password
        }.ConnectionString;

        optionsBuilder.UseNpgsql(connectionString, (options) =>
        {
        });
    }
}