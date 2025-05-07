namespace Web8.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string login, string password);
    }
}