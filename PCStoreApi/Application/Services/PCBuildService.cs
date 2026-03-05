using AutoMapper;
using PCStoreApi.Application.DTOs.PCBuild;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Application.Services
{
    public class PCBuildService : IPCBuildService
    {
        private readonly IPCBuildRepository _repo;
        private readonly IMapper _mapper;

        public PCBuildService(IPCBuildRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PCBuildReadDto> CreateBuildAsync(PCBuildCreateDto dto)
        {
            var build = _mapper.Map<PCBuild>(dto);
            await _repo.AddBuildAsync(build);
            await _repo.SaveChangesAsync();
            return _mapper.Map<PCBuildReadDto>(build);
        }

        public async Task<bool> DeleteBuildAsync(int id)
        {
            var build = await _repo.GetBuildByIdAsync(id);
            if (build == null) return false;

            await _repo.DeleteBuildAsync(build);
            return await _repo.SaveChangesAsync();
        }

        public async Task<List<PCBuildReadDto>> GetAllBuildsAsync()
        {
            var builds = await _repo.GetAllBuildsAsync();
            return _mapper.Map<List<PCBuildReadDto>>(builds);
        }

        public async Task<PCBuildReadDto?> GetBuildByIdAsync(int id)
        {
            var build = await _repo.GetBuildByIdAsync(id);
            return _mapper.Map<PCBuildReadDto>(build);
        }

        public async Task<PCBuildReadDto> GetBuildByUserIdAsync(int userId)
        {

            var builds = await _repo.GetBuildByUserIdAsync(userId);
            return _mapper.Map<PCBuildReadDto>(builds);
        }

        public async Task<bool> UpdateBuildAsync(int id, PCBuildUpdateDto dto)
        {
            var build = await _repo.GetBuildByIdAsync(id);
            if (build == null) return false;
            if (dto.GraphicsCard == null)
                dto.GraphicsCard = build.GraphicsCard;

            _mapper.Map(dto, build);
            await _repo.UpdateBuildAsync(build);
            return await _repo.SaveChangesAsync();
        }
    }
}
