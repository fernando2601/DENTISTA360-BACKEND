using Application.DTOs;

namespace DENTISTA360_BACKEND.Services
{
    public interface ISignupService
    {
        Task<SignupResponse> SignupAsync(SignupRequest request);
    }
}

