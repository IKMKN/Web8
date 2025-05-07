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

        
        [HttpGet("GetUserById/{id}")]
        public async Task<IResult> GetUserByIdAsync(Guid id)
        {
            var result = await userService.GetUserAsync(id);
            return Results.Ok(result);
        }

       
        [HttpGet("GetAllUsers")]
        public async Task<IResult> GetUsersAsync()
        {
            var result = await userService.GetAllUsersAsync();
            return Results.Ok(result);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("AddUser")]
        public async Task<IResult> AddUserAsync(CreateUserRequest request)
        {
            await userService.CreateUserAsync(request);
            return Results.Created();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteUser(Guid id)
        {
            await userService.BlockUserAsync(id);
            return Results.NoContent();
        }
    }
}
