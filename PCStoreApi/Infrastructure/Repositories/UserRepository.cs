using Microsoft.EntityFrameworkCore;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Data;

namespace PCStoreApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(UserInfo user)
        {
            await _context.UserInfo.AddAsync(user);
        }

        public Task DeleteUserAsync(UserInfo user)
        {
            _context.UserInfo.Remove(user);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsersAsync()
        {
            return await _context.UserInfo.Include(u => u.PCBuild).ToListAsync();
        }

        public async Task<UserInfo?> GetUserByIDAsync(int id)
        {
            return await _context.UserInfo.Include(u => u.PCBuild).FirstOrDefaultAsync(u => u.UserID == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task UpdateUserAsync(UserInfo user)
        {
            _context.UserInfo.Update(user);
            return Task.CompletedTask;
        }
    }
}
