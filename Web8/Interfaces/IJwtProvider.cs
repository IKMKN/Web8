using Web8.Models.Entities;

namespace Web8.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}