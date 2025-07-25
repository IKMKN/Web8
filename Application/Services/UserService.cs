using Domain.Models.Responses;
using DataAccess;
using Domain.Entities;
using Domain.Enums;
using Domain.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext context;
    private readonly IPasswordHasher passwordHasher;
    private readonly ILogger<UserService> logger;
    private static readonly ConcurrentDictionary<string, bool> pendingLogins = new();
    
    public UserService(AppDbContext context, IPasswordHasher passwordHasher, ILogger<UserService> logger)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
        this.logger = logger;
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {
        logger.LogInformation("Attempting to create user with Login - {Login}", request.Login);

        if (!pendingLogins.TryAdd(request.Login, true)) 
        {
            logger.LogWarning("Dublicate registration attempt for Login - {Login}", request.Login);
            throw new ArgumentException("This Login is already being register"); 
        }
            

        try
        {
            if (await context.Users.AnyAsync(u => u.Login == request.Login))
            {
                logger.LogWarning("User creation failed. Login already exsists - {Login}", request.Login);
                throw new ArgumentException("This Login exists");
            }
                
            if (request.UserGroupId is (int)UserGroupCode.Admin)
            {
                bool adminExist = await context.Users
                     .AnyAsync(u => u.UserGroup.UserGroupCode == UserGroupCode.Admin);

                if (adminExist)
                {
                    logger.LogWarning("Attempt to create second admin");
                    throw new ArgumentException("Admin exists");
                }
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

            logger.LogInformation("User create successfully. UserId - {UserId}", user.Id);
        }

        finally 
        {
            pendingLogins.TryRemove(request.Login, out _);
        }
    }
    public async Task SoftDeleteUserAsync(long id)
    {
        logger.LogInformation("Attempting to soft delete user - {UserId}", id);
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null) 
        {
            logger.LogWarning("User not found for deletion. UserId - {UserId}", id);
            throw new KeyNotFoundException($"User {id} not found!");
        }
            

        if (user.UserStateId is (int)UserStateCode.Blocked) 
        {
            logger.LogWarning("User already blocked. UserId - {UserId}", id);
            throw new KeyNotFoundException($"User {id} already blocked");
        }

        if (user.UserGroupId is (int)UserGroupCode.Admin) 
        {
            logger.LogWarning("Attempt to block Admin.");
            throw new ArgumentException($"Admin cannot be blocked!"); 
        }
            

        user.UserStateId = (int)UserStateCode.Blocked;
        await context.SaveChangesAsync();

        logger.LogInformation("User with UserId - {UserId} successfully blocked.", id);
    }
    public async Task<UserResponse> GetUserAsync(long id)
    {
        logger.LogInformation("Getting user with id - {id}", id);

        var user = await context.Users
            .Include(u => u.UserGroup)
            .Include(u => u.UserState)
            .FirstOrDefaultAsync(x=> x.Id == id);

        if (user is null) 
        {
            logger.LogWarning("User not found. UserId - {id}", id);
            throw new KeyNotFoundException($"User {id} not found!");
        }
            

        if (user.UserStateId == (int)UserStateCode.Blocked)
        {
            logger.LogWarning("Attempt to get blocked user. UserId - {id}", id);
            throw new KeyNotFoundException($"User {id} is blocked!");
        }

        logger.LogInformation("Successfully getting user. UserId - {id}", id);
        return MapToResponce(user);
    }
    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        logger.LogInformation("Getting all users");

        var result = await context.Users
            .Include(u => u.UserGroup)
            .Include(u => u.UserState)
            .Where(u => u.UserStateId == (int)UserStateCode.Active)
            .ToListAsync();

        logger.LogInformation("Users found - {count}", result.Count);
        return [.. result.Select(MapToResponce)];
    }

    private static UserResponse MapToResponce(User user) => new()
    {
        UserId = user.Id,
        Login = user.Login,
        UserGroup = new UserGroupResponce
        {
            //UserGroupId = user.UserGroup.UserGroupId,
            UserGroupCode = user.UserGroup.UserGroupCode.ToString(),
            Description = user.UserGroup.Description
        },
        UserState = new UserStateResponse
        {
            //UserStateId = user.UserState.UserStateId,
            UserStateCode = user.UserState.UserStateCode.ToString(),
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