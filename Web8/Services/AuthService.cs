using Microsoft.EntityFrameworkCore;
using Web8.Data;
using Web8.Interfaces;

namespace Web8.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext context;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtProvider jwtProvider;

    public AuthService(AppDbContext context, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        this.context = context;
        this.passwordHasher = passwordHasher;
        this.jwtProvider = jwtProvider;
    }

    public async Task<string> LoginAsync(string login, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u=> u.Login == login) 
            ?? throw new Exception("This login not exist!");

        var result = passwordHasher.Verify(password, user.PasswordHash);
        if (!result)
            throw new Exception("Incorrect password!");

        var token = jwtProvider.GenerateToken(user);
        return token;
    }
}
