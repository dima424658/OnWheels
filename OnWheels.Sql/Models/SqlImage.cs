using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnWheels.Sql.Models;

[Table("Image")]
public class SqlImage
{
    [Column("ImageId"), Key]
    public int Id { get; set; }

    public string MimeType { get; set; } = string.Empty;

    public byte[] RawData { get; set; } = Array.Empty<byte>();
}