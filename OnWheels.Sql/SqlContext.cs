using Microsoft.EntityFrameworkCore;
using OnWheels.Sql.Models;

namespace OnWheels.Sql;

public abstract class SqlContext : DbContext
{
    public SqlContext(DbContextOptions options)
        : base(options) { }

    public DbSet<SqlImage> Images { get; set; } = null!;

    public DbSet<SqlRace> Races { get; set; } = null!;

    public DbSet<SqlRaceImage> RaceImages { get; set; } = null!;

    public DbSet<SqlRaceLike> RaceLikes { get; set; } = null!;

    public DbSet<SqlRaceTag> RaceTags { get; set; } = null!;

    public DbSet<SqlRaceView> RaceViews { get; set; } = null!;

    public DbSet<SqlUser> Users { get; set; } = null!;

    protected abstract override void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
}