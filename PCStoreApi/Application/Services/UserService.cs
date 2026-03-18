using AutoMapper;
using PCStoreApi.Application.DTOs.User;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Domain.Entities;

namespace PCStoreApi.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserReadDto?> CreateUserAsync(Guid userId, UserCreateDto dto)
        {
            var userInfo = _mapper.Map<UserInfo>(dto);
            userInfo.UserId = userId;
            await _repo.AddUserAsync(userInfo);
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserReadDto>(userInfo);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userInfo = await _repo.GetUserByIdAsync(id);
            if (userInfo == null) return false;

            await _repo.DeleteUserAsync(userInfo);
            return await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var userInfo = await _repo.GetAllUsersAsync();
            return _mapper.Map<List<UserReadDto>>(userInfo);
        }

        public async Task<UserReadDto?> GetUserByIdAsync(Guid id)
        {
            var userInfo = await _repo.GetUserByIdAsync(id);
            return userInfo == null? null : _mapper.Map<UserReadDto>(userInfo);
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserUpdateDto dto)
        {
            var userInfo = await _repo.GetUserByIdAsync(id);
            if (userInfo == null) return false;

            _mapper.Map(dto,userInfo);
            await _repo.UpdateUserAsync(userInfo);
            return await _repo.SaveChangesAsync();
        }
    }
}
