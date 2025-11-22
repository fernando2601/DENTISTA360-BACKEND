using DENTISTA360_BACKEND.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<Clinica>> GetUserClinicsAsync(int userId);
        Task<string?> GetUserPermissionInClinicAsync(int userId, int clinicId);
    }
}
