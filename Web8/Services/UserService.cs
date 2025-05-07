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
    private readonly IHttpContextAccessor httpContextAccessor;
    private static readonly ConcurrentDictionary<string, bool> pendingLogins = new();
    public UserService(AppDbContext context, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {


        if (!pendingLogins.TryAdd(request.Login, true))
            throw new Exception("This Login is already being register");

        if (request.UserGroupId is (int)UserGroupCode.Admin)
        {
           bool adminExist =  await context.Users
                .AnyAsync(u=> u.UserGroup.UserGroupCode == UserGroupCode.Admin);

            if (adminExist)
                throw new Exception("Admin Exists");
        }

        await Task.Delay(5000);

        if (await context.Users.AnyAsync(u=>u.Login == request.Login))
            throw new Exception("This Login exists");

        var hashedPassword = passwordHasher.Generate(request.Password);

        await context.Users.AddAsync(new User
        {
            Id = new Guid(),
            Login = request.Login,
            PasswordHash = hashedPassword,
            CreatedDate = DateTime.UtcNow,
            UserGroupId = request.UserGroupId,
            UserStateId = (int)UserStateCode.Active,
        });

        await context.SaveChangesAsync();

        pendingLogins.TryRemove(request.Login, out _);
    }
    public async Task BlockUserAsync(Guid id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            throw new KeyNotFoundException($"User {id} not found");
 
        user.UserStateId = (int)UserStateCode.Blocked;
        await context.SaveChangesAsync();
    }
    public async Task<UserResponse> GetUserAsync(Guid id)
    {
        var user = await context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponse
            {
                UserId = u.Id,
                Login = u.Login,
                UserGroup = new UserGroupResponce
                {
                    UserGroupId = u.UserGroup.UserGroupId,
                    UserGroupCode = u.UserGroup.UserGroupCode.ToString(),
                    Description = u.UserGroup.Description
                },
                UserState = new UserStateResponse
                {
                    UserStateId = u.UserState.UserStateId,
                    UserStateCode = u.UserState.UserStateCode.ToString(),
                    Description= u.UserState.Description
                },
                CreatedDate = u.CreatedDate
            })
            .FirstOrDefaultAsync();

        return user 
            ?? throw new KeyNotFoundException($"User {id} not found");   
    }
    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        return await context.Users
            .Select(u => new UserResponse
            {
                UserId = u.Id,
                Login = u.Login,
                UserGroup = new UserGroupResponce
                {
                    UserGroupId = u.UserGroup.UserGroupId,
                    UserGroupCode = u.UserGroup.UserGroupCode.ToString(),
                    Description = u.UserGroup.Description
                },
                UserState = new UserStateResponse
                {
                    UserStateId = u.UserState.UserStateId,
                    UserStateCode = u.UserState.UserStateCode.ToString(),
                    Description = u.UserState.Description
                },
                CreatedDate = u.CreatedDate
            })
            .ToListAsync();
    }
}
