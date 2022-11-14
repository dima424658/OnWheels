using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnWheels.Sql.Models;

[Table("RaceView")]
public class SqlRaceView
{
    [Column("RaceViewId"), Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RaceId { get; set; }
}