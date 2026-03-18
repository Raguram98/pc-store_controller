using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserInfo>> GetAllUsersAsync();
        Task<UserInfo?> GetUserByIdAsync(Guid id);
        Task AddUserAsync(UserInfo user);
        Task UpdateUserAsync(UserInfo user);
        Task DeleteUserAsync(UserInfo user);
        Task<bool> SaveChangesAsync();
    }
}
