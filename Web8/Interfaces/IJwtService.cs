using Web8.Models.Entities;

namespace Web8.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}