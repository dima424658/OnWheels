using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnWheels.Sql.Models;

[Table("RaceTag")]
public class SqlRaceTag
{
    [Column("RaceTagId"), Key]
    public int Id { get; set; }

    public int RaceId { get; set; }

    public string Name { get; set; } = string.Empty;
}