using PCStoreApi.Application.DTOs.User;

namespace PCStoreApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserReadDto?> GetUserByIdAsync(int id);
        Task<UserReadDto?> CreateUserAsync(UserCreateDto dto);
        Task<bool> UpdateUserAsync(int id,UserUpdateDto dto);
        Task<bool> DeleteUserAsync(int id);
    }
}
