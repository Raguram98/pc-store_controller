using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PCStoreApi.Application.DTOs.PCBuild;

namespace PCStoreApi.Application.Interfaces
{
    public interface IPCBuildService
    {
        Task<List<PCBuildReadDto>> GetAllBuildsAsync();
        Task<PCBuildReadDto?> GetBuildByIdAsync(int id);
        Task<PCBuildReadDto> GetBuildByUserIdAsync(int userId);
        Task<PCBuildReadDto> CreateBuildAsync(PCBuildCreateDto dto);
        Task<bool> UpdateBuildAsync(int id, PCBuildUpdateDto dto);
        Task<bool> DeleteBuildAsync(int id);
    }
}
