using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnWheels.Sql.Models;

[Table("RaceLike")]
public class SqlRaceLike
{
    [Column("RaceLikeId"), Key]
    public int Id { get; set; }

    public int RaceId { get; set; }

    public int UserId { get; set; }
}