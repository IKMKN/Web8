namespace API.Interfaces;

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string passwordHash, string password);
}