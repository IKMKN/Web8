using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Web8.Data;
using Web8.Interfaces;
using Web8.Models.Entities;
using Web8.Models.Requests;
using Web8.Models.Responses;

namespace Web8.Services;

public class UserService : IUserService
{
    private readonly AppDbContext context;
    private readonly IPasswordHasher passwordHasher;
    private static readonly ConcurrentDictionary<string, bool> pendingLogins = new();
    public UserService(AppDbContext context, IPasswordHasher passwordHasher)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {
        if (!pendingLogins.TryAdd(request.Login, true))
            throw new ArgumentException("This Login is already being register");

        try
        {
            if (await context.Users.AnyAsync(u => u.Login == request.Login))
                throw new ArgumentException("This Login exists");

            if (request.UserGroupId is (int)UserGroupCode.Admin)
            {
                bool adminExist = await context.Users
                     .AnyAsync(u => u.UserGroup.UserGroupCode == UserGroupCode.Admin);

                if (adminExist)
                    throw new ArgumentException("Admin exists");
            }

            await Task.Delay(5000);

            var user = new User 
            {
                Login = request.Login,
                PasswordHash = passwordHasher.Generate(request.Password),
                CreatedDate = DateTime.UtcNow,
                UserGroupId = request.UserGroupId,
                UserStateId = (int)UserStateCode.Active
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        finally 
        {
            pendingLogins.TryRemove(request.Login, out _);
        }
    }
    public async Task BlockUserAsync(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            throw new KeyNotFoundException($"User {id} not found");

        if (user.UserStateId is (int)UserStateCode.Blocked)
            throw new KeyNotFoundException($"User {id} already blocked");

        if (user.UserGroupId is (int)UserGroupCode.Admin)
            throw new ArgumentException($"Admin cannot be blocked!");

        user.UserStateId = (int)UserStateCode.Blocked;
        await context.SaveChangesAsync();
    }
    public async Task<UserResponse> GetUserAsync(int id)
    {
        var user = await context.Users
            .Include(u => u.UserGroup)
            .Include(u => u.UserState)
            .FirstOrDefaultAsync(x=> x.Id == id);

        if (user is null)
            throw new KeyNotFoundException($"User {id} not found!");

        return MapToResponce(user);
    }
    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var result = await context.Users
            .Include(u => u.UserGroup)
            .Include(u => u.UserState)
            .ToListAsync();

        return result
            .Select(MapToResponce)
            .ToList();
    }

    private static UserResponse MapToResponce(User user) => new()
    {
        UserId = user.Id,
        Login = user.Login,
        UserGroup = new UserGroupResponce
        {
            UserGroupId = user.UserGroup.UserGroupId,
            UserGroupCode = user.UserGroup.UserGroupCode,
            Description = user.UserGroup.Description
        },
        UserState = new UserStateResponse
        {
            UserStateId = user.UserState.UserStateId,
            UserStateCode = user.UserState.UserStateCode,
            Description = user.UserState.Description
        },
        CreatedDate = user.CreatedDate
    };
}

//public record OperationResult (bool IsSuccess, string? Error = null, int? ErrorCode = null)
//{
//    public static OperationResult Success() 
//        => new(true);
//    public static OperationResult Fail(string error, int errorCode) 
//        => new(false, error, errorCode);
//}