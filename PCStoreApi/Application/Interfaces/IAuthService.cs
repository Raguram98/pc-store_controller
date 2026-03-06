using PCStoreApi.Application.DTOs.Auth;
using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
