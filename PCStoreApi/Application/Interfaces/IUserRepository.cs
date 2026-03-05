using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserInfo>> GetAllUsersAsync();
        Task<UserInfo?> GetUserByIDAsync(int id);
        Task AddUserAsync(UserInfo user);
        Task UpdateUserAsync(UserInfo user);
        Task DeleteUserAsync(UserInfo user);
        Task<bool> SaveChangesAsync();
    }
}
