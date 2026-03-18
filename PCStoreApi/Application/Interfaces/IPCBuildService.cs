using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PCStoreApi.Application.DTOs.PCBuild;

namespace PCStoreApi.Application.Interfaces
{
    public interface IPCBuildService
    {
        Task<List<PCBuildReadDto>> GetAllBuildsAsync();
        Task<PCBuildReadDto?> GetBuildByIdAsync(Guid id);
        Task<PCBuildReadDto> GetBuildByUserIdAsync(Guid userId);
        Task<PCBuildReadDto> CreateBuildAsync(PCBuildCreateDto dto);
        Task<bool> UpdateBuildAsync(Guid id, PCBuildUpdateDto dto);
        Task<bool> DeleteBuildAsync(Guid id);
    }
}
