using AutoMapper;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Application.DTOs.User;

namespace PCStoreApi.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserInfo, UserReadDto>();
            CreateMap<UserCreateDto, UserInfo>();
            CreateMap<UserUpdateDto, UserInfo>();
            CreateMap<UserInfo, UserUpdateDto>();
        }
    }
}
