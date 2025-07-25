using Domain.Models.Responses;
using Domain.Models.Requests;

namespace Application.Interfaces;

public interface IUserService
{
    Task SoftDeleteUserAsync(long id);
    Task CreateUserAsync(CreateUserRequest request);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserAsync(long id);
}