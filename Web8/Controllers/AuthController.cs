using Application.Interfaces;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

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
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var token = await authService.LoginAsync(request.Login, request.Password);

        HttpContext.Response.Cookies.Append("secret-cookie", token);

        return Ok("Successful login!");
    }
}
