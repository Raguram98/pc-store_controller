using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Data;
using PCStoreApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;


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
        [InlineData("John Doe", "john@example.com", "123 Main Street")]
        [InlineData("Alice Wonderland", "alice@wonderland.com", "Wonderland Avenue")]
        [InlineData("Bob Builder", "bob@builder.com", "Construction Site")]
        public async Task AddUserAsync_ShouldAddUsersToDatabase(string fullName, string email, string address)
        {
            //Arrange
            await InitializeAsync();

            var user = new UserInfo
            {
                FullName = fullName,
                Email = email,
                Address = address
            };

            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            var users = await _repo.GetAllUsersAsync();


            users.Should().HaveCount(1);
            users.Should().ContainSingle(u =>
                u.FullName == fullName &&
                u.Email == email &&
                u.Address == address);
        }

        [Theory]
        [InlineData("Jane Doe", "jane@example.com", "456 Park Avenue")]
        [InlineData("Alice Wonderland", "alice@wonderland.com", "Wonderland Avenue")]
        [InlineData("Bob Builder", "bob@builder.com", "Construction Site")]
        public async Task GetUserByIDAsync_ShouldReturnCorrectUser(string fullName, string email, string address)
        {
            await InitializeAsync();

            var user = new UserInfo { FullName = fullName, Email = email, Address = address };

            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            var fetchedUser = await _repo.GetUserByIDAsync(user.UserID);

            fetchedUser.Should().NotBeNull();
            fetchedUser.FullName.Should().Be(fullName);
            fetchedUser.Email.Should().Be(email);
            fetchedUser.Address.Should().Be(address);
        }

        [Theory]
        [InlineData("John Marston", "john@marston.com", "Old West Street", "Jack Marston", "jack@marston.com", "New West Avenue")]
        public async Task UpdateUserAsync_ShouldUpdateUserDetails(
            string oldFullName, string oldEmail, string oldAddress,string newFullName, string newEmail, string newAddress)
        {
            await InitializeAsync();

            var user = new UserInfo
            {
                FullName = oldFullName,
                Email = oldEmail,
                Address = oldAddress
            };
            await _repo!.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            user.FullName = newFullName;
            user.Email = newEmail;
            user.Address = newAddress;

            await _repo.UpdateUserAsync(user);
            await _repo.SaveChangesAsync();

            var updatedUser = await _repo.GetUserByIDAsync(user.UserID);
            updatedUser.Should().NotBeNull();
            updatedUser.FullName.Should().Be(newFullName);
            updatedUser.Email.Should().Be(newEmail);
            updatedUser.Address.Should().Be(newAddress);
        }

        [Theory]
        [InlineData("Sumala", "sumala@kumala.com", "Indonesia")]
        public async Task DeleteUserAsync_ShouldRemoveUserFromDatabase(string fullName, string email, string address)
        {
            await InitializeAsync();

            var user = new UserInfo
            {
                FullName = fullName,
                Email = email,
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

            var users = new List<UserInfo>
            {
                new UserInfo
                {
                    FullName = "John Marston", Email = "john@marston.com", Address = "Old West Street"
                },
                new UserInfo
                {
                    FullName = "Jack Marston", Email = "jack@marston.com", Address = "New West Avenue"
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
    }
}
