using PCStoreApi.Application.DTOs.User;

namespace PCStoreApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserReadDto?> GetUserByIdAsync(Guid id);
        Task<UserReadDto?> CreateUserAsync(Guid userId, UserCreateDto dto);
        Task<bool> UpdateUserAsync(Guid id,UserUpdateDto dto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
