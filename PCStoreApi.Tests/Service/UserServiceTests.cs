using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using PCStoreApi.Application.DTOs.User;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Application.Services;
using PCStoreApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCStoreApi.Tests.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new UserService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllUserAsync_ShouldReturnMappedUserList()
        {
            var users = new List<UserInfo>
            {
                new() { UserID = 1, FullName = "Conner", Email = "conner@detroit.org", Address = "Detroit" },
                new() { UserID = 2, FullName = "Markus", Email = "markus@detroit.org", Address = "Detroit" },
                new() { UserID = 3, FullName = "Kara", Email = "kara@detroit.org", Address = "Detroit" },
            };

            var mappedUsers = new List<UserReadDto>
            {
                new() { UserId = 1, FullName = "Conner", Email = "conner@detroit.org", Address = "Detroit" },
                new() { UserId = 2, FullName = "Markus", Email = "markus@detroit.org", Address = "Detroit" },
                new() { UserId = 3, FullName = "Kara", Email = "kara@detroit.org", Address = "Detroit" },
            };

            _repoMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<List<UserReadDto>>(users)).Returns(mappedUsers);

            var result = await _service.GetAllUsersAsync();

            result.Should().BeEquivalentTo(mappedUsers);
            _repoMock.Verify(r => r.GetAllUsersAsync(), Times.Once);
        }


        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
        {
            var user = new UserInfo { UserID = 1, FullName = "Connor", Email = "conner@detroit.org", Address = "Detroit" };
            var dto = new UserReadDto { UserId = 1, FullName = "Connor", Email = "conner@detroit.org", Address = "Detroit" };

            _repoMock.Setup(r => r.GetUserByIDAsync(1)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserReadDto>(user)).Returns(dto);

            var result = await _service.GetUserByIdAsync(1);

            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            _repoMock.Setup(r => r.GetUserByIDAsync(1)).ReturnsAsync((UserInfo?)null);

            var result = await _service.GetUserByIdAsync(1);

            result.Should().BeNull();
        }


        [Fact]
        public async Task CreateUserAsync_ShouldAddUserAndReturnMappedDto()
        {
            var createDto = new UserCreateDto
            {
                FullName = "Markus",
                Email = "markus@detroit.org",
                Address = "Detroit"
            };
            var user = new UserInfo { UserID = 1, FullName = "Markus", Email = "markus@detroit.org", Address = "Detroit" };
            var readDto = new UserReadDto { UserId = 1, FullName = "Markus", Email = "markus@detroit.org", Address = "Detroit" };

            _mapperMock.Setup(m => m.Map<UserInfo>(createDto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserReadDto>(user)).Returns(readDto);

            _repoMock.Setup(r => r.AddUserAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateUserAsync(createDto);

            result.Should().BeEquivalentTo(readDto);
            _repoMock.Verify(r => r.AddUserAsync(user), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var user = new UserInfo
            {
                UserID = 1,
                FullName = "OldKara",
                Email = "kara@detroit.org",
                Address = "Detroit"
            };
            var dto = new UserUpdateDto
            {
                FullName = "NewKara",
                Email = "kara@detroit.org",
                Address = "Detroit"
            };

            _repoMock.Setup(r => r.GetUserByIDAsync(1)).ReturnsAsync(user);
            _repoMock.Setup(r => r.UpdateUserAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateUserAsync(1, dto);

            result.Should().BeTrue();
            _repoMock.Verify(r => r.GetUserByIDAsync(1), Times.Once);

        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            _repoMock.Setup(r => r.GetUserByIDAsync(1)).ReturnsAsync((UserInfo?)null);

            var result = await _service.UpdateUserAsync(1, new UserUpdateDto());
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var user = new UserInfo
            {
                UserID = 1,
                FullName = "Connor",
                Email = "connor@detroit.org",
                Address = "Detroit"
            };

            _repoMock.Setup(r => r.GetUserByIDAsync(1)).ReturnsAsync(user);
            _repoMock.Setup(r => r.DeleteUserAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteUserAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            _repoMock.Setup(r => r.GetUserByIDAsync(1)).ReturnsAsync((UserInfo?)null);

            var result = await _service.DeleteUserAsync(1);

            result.Should().BeFalse();
        }
    }
}
