using DENTISTA360_BACKEND.Models;

namespace DENTISTA360_BACKEND.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string email, string password);
        string GenerateJwtToken(User user);
        bool VerifyPassword(string password, string hashedPassword);
        string HashPassword(string password);
    }
}
