using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Data;
using PCStoreApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PCStoreApi.Tests.Repositories
{
    public class PCBuildRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly PCBuildRepository _repo;

        public PCBuildRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new AppDbContext(options);
            _repo = new PCBuildRepository(_context);
        }

        [Fact]
        public async Task AddBuildAsync_ShouldAddNewPCBuild()
        {
            var user = new UserInfo
            {
                FullName = "John Wick",
                Email = "john@continental.com",
                Address = "New York"
            };
            await _context.UserInfo.AddAsync(user);
            await _context.SaveChangesAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 5 5600",
                RamInGB = 16,
                GraphicsCard = "RTX 3060",
                Storage = "1TB SSD",
                UserID = user.UserID
            };

            await _repo.AddBuildAsync(build);
            await _context.SaveChangesAsync();

            var result = await _context.PCBuilds.FirstOrDefaultAsync(p => p.PCBuildId == build.PCBuildId);
            result.Should().NotBeNull();
            result.Processor.Should().Be("Ryzen 5 5600");
        }

        [Fact]
        public async Task GetBuildByIdAsync_ShouldReturnPCBuild_WhenExists()
        {
            var user = new UserInfo
            {
                FullName = "John Wick",
                Email = "john@continental.com",
                Address = "New York"
            };
            await _context.UserInfo.AddAsync(user);
            await _context.SaveChangesAsync();
            // Arrange
            var build = new PCBuild
            {
                Processor = "i7-13700K",
                RamInGB = 32,
                GraphicsCard = "RTX 4080",
                Storage = "2TB NVMe SSD",
                UserID = user.UserID
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
            var user = new UserInfo
            {
                FullName = "Arthur Morgan",
                Email = "arthur@rdr2.com",
                Address = "Valentine"
            };

            await _context.UserInfo.AddAsync(user);
            await _context.SaveChangesAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 9 7900X",
                RamInGB = 32,
                GraphicsCard = "RTX 4090",
                Storage = "2TB SSD",
                UserID = user.UserID
            };
            await _context.PCBuilds.AddAsync(build);
            await _context.SaveChangesAsync();


            var result = await _repo.GetBuildByUserIdAsync(user.UserID);


            result.Should().NotBeNull();
            result!.UserID.Should().Be(user.UserID);
        }

        [Fact]
        public async Task UpdateBuildAsync_ShouldModifyExistingBuild()
        {
            var user = new UserInfo
            {
                FullName = "Arthur Morgan",
                Email = "arthur@rdr2.com",
                Address = "Valentine"
            };

            await _context.UserInfo.AddAsync(user);
            await _context.SaveChangesAsync();
            var build = new PCBuild
            {
                Processor = "Ryzen 3 3200G",
                RamInGB = 8,
                GraphicsCard = "GTX 1650",
                Storage = "500GB HDD",
                UserID = 1
            };
            await _context.PCBuilds.AddAsync(build);
            await _context.SaveChangesAsync();

            build.RamInGB = 16;
            build.Storage = "1TB SSD";

            // Act
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
            var user = new UserInfo
            {
                FullName = "Arthur Morgan",
                Email = "arthur@rdr2.com",
                Address = "Valentine"
            };

            await _context.UserInfo.AddAsync(user);
            await _context.SaveChangesAsync();

            var build = new PCBuild
            {
                Processor = "Ryzen 9 7900X",
                RamInGB = 32,
                GraphicsCard = "RTX 4090",
                Storage = "2TB SSD",
                UserID = user.UserID
            };
            await _context.PCBuilds.AddAsync(build);
            await _context.SaveChangesAsync();

            await _repo.DeleteBuildAsync(build);
            await _repo.SaveChangesAsync();

            var exists = await _context.PCBuilds.AnyAsync(b => b.PCBuildId == build.PCBuildId);
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllBuildsAsync_ShouldReturnAllBuilds()
        {
            var users = new List<UserInfo>
            {
                new UserInfo
                {
                FullName = "Arthur Morgan",
                Email = "arthur@rdr2.com",
                Address = "Valentine"
            },
                new UserInfo
            {
                FullName = "John Marston",
                Email = "john@rdr2.com",
                Address = "Old West Street"
            }
            };

            await _context.UserInfo.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var builds = new List<PCBuild>
            {
                new PCBuild
                {
                    Processor = "i5-12400F",
                    RamInGB = 16,
                    GraphicsCard = "RTX 3060",
                    Storage = "1TB SSD",
                    UserID = 1
                },
                new PCBuild
                {
                    Processor = "Ryzen 7 5800X",
                    RamInGB = 32,
                    GraphicsCard = "RTX 3070",
                    Storage = "2TB NVMe SSD",
                    UserID = 2
                }
            };
            await _context.PCBuilds.AddRangeAsync(builds);
            await _context.SaveChangesAsync();

            var result = await _repo.GetAllBuildsAsync();

            result.Should().HaveCount(2);
        }
    }
}
