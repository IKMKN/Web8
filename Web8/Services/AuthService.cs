using Microsoft.EntityFrameworkCore;
using Web8.Data;
using Web8.Interfaces;
using Web8.Models.Entities;

namespace Web8.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext context;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtService jwtProvider;

    public AuthService(AppDbContext context, IPasswordHasher passwordHasher, IJwtService jwtProvider)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
        this.jwtProvider = jwtProvider;
    }

    public async Task<string> LoginAsync(string login, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login) 
            ?? throw new KeyNotFoundException("This login not exist!");

        if (user.UserStateId == (int)UserStateCode.Blocked)
            throw new ArgumentException($"Account {user.Login} is blocked!");

        var result = passwordHasher.Verify(password, user.PasswordHash);

        if (result is false)
            throw new ArgumentException("Incorrect password!");

        var token = jwtProvider.GenerateToken(user);
        return token;
    }
}
