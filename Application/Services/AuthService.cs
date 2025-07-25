using DataAccess;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext context;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtService jwtProvider;
    private readonly ILogger<AuthService> logger;

    public AuthService(AppDbContext context, 
        IPasswordHasher passwordHasher, 
        IJwtService jwtProvider, 
        ILogger<AuthService> logger)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
        this.jwtProvider = jwtProvider;
        this.logger = logger;
    }

    public async Task<string> LoginAsync(string login, string password)
    {
        logger.LogInformation("Attempting login for user - {login}", login);

        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);

        if (user is null) 
        {
            logger.LogWarning("User not exist. Login - {login}",login);
            throw new KeyNotFoundException("This login not exist!");
        }
             
        if (user.UserStateId is (int)UserStateCode.Blocked)
        {
            logger.LogWarning("User is blocked. Login - {login}, userId - {userId}", login, user.Id);
            throw new ArgumentException($"Account {user.Login} is blocked!");
        }
            
        var result = passwordHasher.Verify(password, user.PasswordHash);

        if (result is false)
        {
            logger.LogWarning("Incorrect password entered. Login - {login}, userId - {userId}", login, user.Id);
            throw new ArgumentException("Incorrect password!");
        }
            
        var token = jwtProvider.GenerateToken(user);
        return token;
    }
}
