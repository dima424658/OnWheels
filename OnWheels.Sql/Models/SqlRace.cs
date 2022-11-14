using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnWheels.Sql.Models;

[Table("Race")]
public class SqlRace
{
    [Column("RaceId"), Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    [Column("LocLat")]
    public decimal LocationLatitude { get; set; }

    [Column("LocLong")]
    public decimal LocationLongitude { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public string Description { get; set; } = string.Empty;
}