using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Data;
using PCStoreApi.Infrastructure.Repositories;


namespace PCStoreApi.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private AppDbContext? _context;
        private UserRepository? _repo;

        private async Task InitializeAsync()
        {
            _context = await GetInMemoryDbContextAsync();
            _repo = new UserRepository(_context);
        }

        private async Task<AppDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }


        [Theory]
        [InlineData("John Doe",  "123 Main Street")]
        [InlineData("Alice Wonderland",  "Wonderland Avenue")]
        [InlineData("Bob Builder", "Construction Site")]
        public async Task AddUserAsync_ShouldAddUsersToDatabase(string fullName, string address)
        {
            //Arrange
            await InitializeAsync();

            var userEntity = await CreateTestUserAsync();

            var user = new UserInfo
            {
                UserId = userEntity.Id,
                FullName = fullName,
                Address = address
            };

            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            var users = await _repo.GetAllUsersAsync();


            users.Should().HaveCount(1);
            users.Should().ContainSingle(u =>
                u.FullName == fullName &&
                u.Address == address);
        }

        [Theory]
        [InlineData("Jane Doe", "456 Park Avenue")]
        [InlineData("Alice Wonderland", "Wonderland Avenue")]
        [InlineData("Bob Builder", "Construction Site")]
        public async Task GetUserByIDAsync_ShouldReturnCorrectUser(string fullName, string address)
        {
            await InitializeAsync();

            var userEntity = await CreateTestUserAsync();

            var user = new UserInfo { UserId = userEntity.Id, FullName = fullName, Address = address };

            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            var fetchedUser = await _repo.GetUserByIdAsync(user.UserId);

            fetchedUser.Should().NotBeNull();
            fetchedUser.FullName.Should().Be(fullName);
            fetchedUser.Address.Should().Be(address);
        }

        [Theory]
        [InlineData("John Marston", "Old West Street", "Jack Marston", "New West Avenue")]
        public async Task UpdateUserAsync_ShouldUpdateUserDetails(
            string oldFullName, string oldAddress,string newFullName, string newAddress)
        {
            await InitializeAsync();

            var userEntity = await CreateTestUserAsync();

            var user = new UserInfo
            {
                UserId = userEntity.Id,
                FullName = oldFullName,
                Address = oldAddress
            };
            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            user.FullName = newFullName;
            user.Address = newAddress;

            await _repo.UpdateUserAsync(user);
            await _repo.SaveChangesAsync();

            var updatedUser = await _repo.GetUserByIdAsync(user.UserId);
            updatedUser.Should().NotBeNull();
            updatedUser.FullName.Should().Be(newFullName);
            updatedUser.Address.Should().Be(newAddress);
        }

        [Theory]
        [InlineData("Sumala", "Indonesia")]
        public async Task DeleteUserAsync_ShouldRemoveUserFromDatabase(string fullName, string address)
        {
            await InitializeAsync();

            var userEntity = await CreateTestUserAsync();

            var user = new UserInfo
            {
                UserId = userEntity.Id,
                FullName = fullName,
                Address = address
            };

            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            await _repo.DeleteUserAsync(user);
            await _repo.SaveChangesAsync();

            var users = await _repo.GetAllUsersAsync();
            users.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUSers()
        {
            await InitializeAsync();

            var user1 = await CreateTestUserAsync();
            var user2 = await CreateTestUserAsync();

            var users = new List<UserInfo>
            {
                new UserInfo
                {
                    UserId = user1.Id,FullName = "John Marston", Address = "Old West Street"
                },
                new UserInfo
                {
                    UserId = user2.Id,FullName = "Jack Marston", Address = "New West Avenue"
                }
            };

            foreach (var user in users)
            {
                await _repo!.AddUserAsync(user);
            }

            await _repo!.SaveChangesAsync();
            var result = await _repo.GetAllUsersAsync();

            result.Should().HaveCount(2);
            result.Select(u => u.FullName).Should().Contain("John Marston", "Jack Marston");
        }

        private async Task<User> CreateTestUserAsync()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                HashedPassword = "dummy",
                Role = "User"
            };

            await _context!.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
