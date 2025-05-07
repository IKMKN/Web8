using Web8.Models.Requests;
using Web8.Models.Responses;

namespace Web8.Interfaces;

public interface IUserService
{
    Task BlockUserAsync(Guid id);
    Task CreateUserAsync(CreateUserRequest request);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserAsync(Guid id);
}