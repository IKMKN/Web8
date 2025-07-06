using API.Models.Requests;
using API.Models.Responses;

namespace API.Interfaces;

public interface IUserService
{
    Task SoftDeleteUserAsync(long id);
    Task CreateUserAsync(CreateUserRequest request);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserAsync(long id);
}