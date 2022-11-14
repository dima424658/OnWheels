using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnWheels.Sql.Models;

[Table("RaceImage")]
public class SqlRaceImage
{
    [Column("RaceImageId"), Key]
    public int Id { get; set; }

    public int RaceId { get; set; }

    public int ImageId { get; set; }
}