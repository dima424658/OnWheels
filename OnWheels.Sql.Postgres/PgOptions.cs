namespace OnWheels.Sql.Postgres;

public class PgOptions
{
    public const string SectionName = "PostgreSQL";

    public string Host { get; set; } = "localhost";

    public int Port { get; set; } = 5432;

    public string Database { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}