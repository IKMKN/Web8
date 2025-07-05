using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web8.Interfaces;
using Web8.Models.Requests;

namespace Web8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("Users/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(long id)
        {
            var result = await userService.GetUserAsync(id);
            return Ok(result);
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await userService.GetAllUsersAsync();
            return Ok(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync(CreateUserRequest request)
        {
            await userService.CreateUserAsync(request);
            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> SoftDeleteUserAsync(int id)
        {
            await userService.SoftDeleteUserAsync(id);
            return Ok();
        }
    }
}
