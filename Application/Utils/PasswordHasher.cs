using Application.Interfaces;

namespace Application.Utils;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password) =>
         BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string passwordHash, string password) =>
             BCrypt.Net.BCrypt.EnhancedVerify(passwordHash, password);
}
