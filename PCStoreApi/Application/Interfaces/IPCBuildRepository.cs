using PCStoreApi.Domain.Entities;
using System.Net;

namespace PCStoreApi.Application.Interfaces
{
    public interface IPCBuildRepository
    {
        Task<List<PCBuild>> GetAllBuildsAsync();
        Task<PCBuild?> GetBuildByIdAsync(int id);
        Task<PCBuild?> GetBuildByUserIdAsync(int userId);
        Task AddBuildAsync(PCBuild build);
        Task UpdateBuildAsync(PCBuild build);
        Task DeleteBuildAsync(PCBuild build);
        Task<bool> SaveChangesAsync();
    }
}
