using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web8.Interfaces;
using Web8.Models.Requests;

namespace Web8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginUserRequest request)
    {
        var token = await authService.LoginAsync(request.Login, request.Password);

        HttpContext.Response.Cookies.Append("secret-cookie", token);

        return Results.Ok("Successful login!");
    }
}
