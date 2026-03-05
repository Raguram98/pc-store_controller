using AutoMapper;
using PCStoreApi.Application.DTOs.PCBuild;
using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Application.Mapping
{
    public class PCBuildMappingProfile : Profile
    {
        public PCBuildMappingProfile()
        {
            CreateMap<PCBuild, PCBuildReadDto>();
            CreateMap<PCBuildCreateDto, PCBuild>();
            CreateMap<PCBuildUpdateDto, PCBuild>();
            CreateMap<PCBuild, PCBuildUpdateDto>();
        }
    }
}
