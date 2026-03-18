using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Data;
using PCStoreApi.Infrastructure.Repositories;

namespace PCStoreApi.Tests.Repositories
{
    public class PCBuildRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly PCBuildRepository _repo;

        public PCBuildRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repo = new PCBuildRepository(_context);
        }


        [Fact]
        public async Task AddBuildAsync_ShouldAddNewPCBuild()
        {
            // Arrange
            var userId = await CreateUserAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 5 5600",
                RamInGB = 16,
                GraphicsCard = "RTX 3060",
                Storage = "1TB SSD",
                UserId = userId
            };

            // Act
            await _repo.AddBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Assert
            var result = await _context.PCBuilds.FindAsync(build.PCBuildId);
            result.Should().NotBeNull();
            result!.Processor.Should().Be("Ryzen 5 5600");
        }

        [Fact]
        public async Task GetBuildByIdAsync_ShouldReturnPCBuild_WhenExists()
        {
            // Arrange
            var userId = await CreateUserAsync();

            var build = new PCBuild
            {
                Processor = "i7-13700K",
                RamInGB = 32,
                GraphicsCard = "RTX 4080",
                Storage = "2TB NVMe SSD",
                UserId = userId
            };

            await _repo.AddBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Act
            var result = await _repo.GetBuildByIdAsync(build.PCBuildId);

            // Assert
            result.Should().NotBeNull();
            result!.GraphicsCard.Should().Be("RTX 4080");
        }

        [Fact]
        public async Task GetBuildByUserIdAsync_ShouldReturnBuildLinkedToUser()
        {
            // Arrange
            var userId = await CreateUserAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 9 7900X",
                RamInGB = 32,
                GraphicsCard = "RTX 4090",
                Storage = "2TB SSD",
                UserId = userId
            };

            await _repo.AddBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Act
            var result = await _repo.GetBuildByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task UpdateBuildAsync_ShouldModifyExistingBuild()
        {
            // Arrange
            var userId = await CreateUserAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 3 3200G",
                RamInGB = 8,
                GraphicsCard = "GTX 1650",
                Storage = "500GB HDD",
                UserId = userId
            };

            await _repo.AddBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Act
            build.RamInGB = 16;
            build.Storage = "1TB SSD";

            await _repo.UpdateBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Assert
            var updated = await _context.PCBuilds.FindAsync(build.PCBuildId);
            updated!.RamInGB.Should().Be(16);
            updated.Storage.Should().Be("1TB SSD");
        }

        [Fact]
        public async Task DeleteBuildAsync_ShouldRemoveBuild()
        {
            // Arrange
            var userId = await CreateUserAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 9 7900X",
                RamInGB = 32,
                GraphicsCard = "RTX 4090",
                Storage = "2TB SSD",
                UserId = userId
            };

            await _repo.AddBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Act
            await _repo.DeleteBuildAsync(build);
            await _repo.SaveChangesAsync();

            // Assert
            var exists = await _context.PCBuilds
                .AnyAsync(b => b.PCBuildId == build.PCBuildId);

            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllBuildsAsync_ShouldReturnAllBuilds()
        {
            // Arrange
            var user1 = await CreateUserAsync();
            var user2 = await CreateUserAsync();

            var builds = new List<PCBuild>
            {
                new PCBuild
                {
                    Processor = "i5-12400F",
                    RamInGB = 16,
                    GraphicsCard = "RTX 3060",
                    Storage = "1TB SSD",
                    UserId = user1
                },
                new PCBuild
                {
                    Processor = "Ryzen 7 5800X",
                    RamInGB = 32,
                    GraphicsCard = "RTX 3070",
                    Storage = "2TB NVMe SSD",
                    UserId = user2
                }
            };

            foreach (var build in builds)
            {
                await _repo.AddBuildAsync(build);
            }

            await _repo.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllBuildsAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        private async Task<Guid> CreateUserAsync()
        {
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Email = "test@test.com",
                HashedPassword = "dummy",
                Role = "User"
            };

            var userInfo = new UserInfo
            {
                UserId = userId,
                FullName = "Test User",
                Address = "Test Address"
            };

            await _context.Users.AddAsync(user);
            await _context.UserInfo.AddAsync(userInfo);
            await _context.SaveChangesAsync();

            return userId;
        }
    }
}