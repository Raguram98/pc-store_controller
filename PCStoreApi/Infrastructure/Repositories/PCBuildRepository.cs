using Microsoft.EntityFrameworkCore;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Data;

namespace PCStoreApi.Infrastructure.Repositories
{
    public class PCBuildRepository : IPCBuildRepository
    {
        private readonly AppDbContext _context;

        public PCBuildRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBuildAsync(PCBuild build)
        {
            await _context.PCBuilds.AddAsync(build);
        }

        public Task DeleteBuildAsync(PCBuild build)
        {
            _context.PCBuilds.Remove(build);
            return Task.CompletedTask;
        }

        public async Task<List<PCBuild>> GetAllBuildsAsync()
        {
            return await _context.PCBuilds.Include(b=> b.User).ToListAsync();
        }

        public async Task<PCBuild?> GetBuildByIdAsync(int id)
        {
            return await _context.PCBuilds.Include(b=> b.User).FirstOrDefaultAsync(b => b.PCBuildId == id);
        }

        public async Task<PCBuild?> GetBuildByUserIdAsync(int userId)
        {
            return await _context.PCBuilds
                .FirstOrDefaultAsync(b => b.UserID == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task UpdateBuildAsync(PCBuild build)
        {
            _context.PCBuilds.Update(build);
            return Task.CompletedTask;
        }
    }
}
