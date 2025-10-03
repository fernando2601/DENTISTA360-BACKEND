using DENTISTA360_BACKEND.Models;

namespace DENTISTA360_BACKEND.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<Clinica>> GetUserClinicsAsync(int userId);
        Task<string?> GetUserPermissionInClinicAsync(int userId, int clinicId);
    }
}
