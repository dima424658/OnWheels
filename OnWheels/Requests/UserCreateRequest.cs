using System.ComponentModel.DataAnnotations;

namespace OnWheels.Requests;

public class UserCreateRequest
{
    //[Required, MinLength(3), MaxLength(20)]
    public string Firstname { get; set; } = string.Empty;

    //[Required, MinLength(3), MaxLength(20)]
    public string Lastname { get; set; } = string.Empty;

    //[Required]
    public string Email { get; set; } = string.Empty;

    //[Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }
}