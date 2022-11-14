using Microsoft.AspNetCore.Mvc;
using OnWheels.Models;
using OnWheels.Requests;

namespace OnWheels.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger Logger;
    private readonly UserModel UserModel;

    public UserController(ILogger<UserController> logger, UserModel userModel)
    {
        Logger = logger;
        UserModel = userModel;
    }

    [HttpPost("Create")]
    public async Task CreateAsync([FromBody] UserCreateRequest user, CancellationToken cancellationToken)
    {
        await UserModel.CreateUserAsync(user, cancellationToken);
    }

    [HttpPost("Authorize")]
    public async Task AuthorizeAsync([FromBody] UserAuthorizeRequest authorizeRequest, CancellationToken cancellationToken)
    {
        var userId = UserModel.GetUserIdByEmail(authorizeRequest.Email);

        await UserModel.AuthorizeAsync(userId, authorizeRequest.Password, cancellationToken);
    }
}