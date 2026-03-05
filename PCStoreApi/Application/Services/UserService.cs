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

        public async Task<UserReadDto?> CreateUserAsync(UserCreateDto dto)
        {
            var user = _mapper.Map<UserInfo>(dto);
            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();
            if (string.IsNullOrEmpty(dto.Address))
            {
                Console.WriteLine("⚠️ WARNING: Address is null - validation likely skipped");
            }
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repo.GetUserByIDAsync(id);
            if (user == null) return false;

            await _repo.DeleteUserAsync(user);
            return await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _repo.GetAllUsersAsync();
            return _mapper.Map<List<UserReadDto>>(users);
        }

        public async Task<UserReadDto?> GetUserByIdAsync(int id)
        {
            var user = await _repo.GetUserByIDAsync(id);
            return user ==null? null : _mapper.Map<UserReadDto>(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var user = await _repo.GetUserByIDAsync(id);
            if (user == null) return false;

            _mapper.Map(dto,user);
            await _repo.UpdateUserAsync(user);
            return await _repo.SaveChangesAsync();
        }
    }
}
