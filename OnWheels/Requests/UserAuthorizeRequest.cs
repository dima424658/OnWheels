using System.ComponentModel.DataAnnotations;

namespace OnWheels.Requests;

public class UserAuthorizeRequest
{
    //[Required]
    public string Email { get; set; } = "vasya@pupkin.ru";

    //[Required]
    public string Password { get; set; } = "strongPassword";
}