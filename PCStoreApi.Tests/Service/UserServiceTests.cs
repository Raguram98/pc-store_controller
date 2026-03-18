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
using System.Xml;

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
            var ids = new List<Guid>()
            { 
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var users = new List<UserInfo>
            {
                new() { UserId = ids[0], FullName = "Conner", Address = "Detroit" },
                new() { UserId = ids[1], FullName = "Markus", Address = "Detroit" },
                new() { UserId = ids[2], FullName = "Kara", Address = "Detroit" },
            };

            var mappedUsers = new List<UserReadDto>
            {
                new() { UserId = ids[0], FullName = "Conner", Address = "Detroit" },
                new() { UserId = ids[1], FullName = "Markus", Address = "Detroit" },
                new() { UserId = ids[2], FullName = "Kara", Address = "Detroit" },
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
            var id = Guid.NewGuid();
            var user = new UserInfo { UserId = id, FullName = "Connor", Address = "Detroit" };
            var dto = new UserReadDto { UserId = id, FullName = "Connor", Address = "Detroit" };

            _repoMock.Setup(r => r.GetUserByIdAsync(dto.UserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserReadDto>(user)).Returns(dto);

            var result = await _service.GetUserByIdAsync(dto.UserId);

            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetUserByIdAsync(id)).ReturnsAsync((UserInfo?)null);

            var result = await _service.GetUserByIdAsync(id);

            result.Should().BeNull();
        }


        [Fact]
        public async Task CreateUserAsync_ShouldAddUserAndReturnMappedDto()
        {
            var id = Guid.NewGuid();
            var createDto = new UserCreateDto
            {
                FullName = "Markus",
                Address = "Detroit"
            };
            var user = new UserInfo { UserId = id, FullName = "Markus", Address = "Detroit" };
            var readDto = new UserReadDto { UserId = id, FullName = "Markus", Address = "Detroit" };

            _mapperMock.Setup(m => m.Map<UserInfo>(createDto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserReadDto>(user)).Returns(readDto);

            _repoMock.Setup(r => r.AddUserAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateUserAsync(id, createDto);

            result.Should().BeEquivalentTo(readDto);
            _repoMock.Verify(r => r.AddUserAsync(user), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var id = Guid.NewGuid();
            var user = new UserInfo
            {
                UserId = id,
                FullName = "OldKara",
                Address = "Detroit"
            };
            var dto = new UserUpdateDto
            {
                FullName = "NewKara",
                Address = "Detroit"
            };

            _repoMock.Setup(r => r.GetUserByIdAsync(id)).ReturnsAsync(user);
            _repoMock.Setup(r => r.UpdateUserAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateUserAsync(id, dto);

            result.Should().BeTrue();
            _repoMock.Verify(r => r.GetUserByIdAsync(id), Times.Once);

        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetUserByIdAsync(id)).ReturnsAsync((UserInfo?)null);

            var result = await _service.UpdateUserAsync(id, new UserUpdateDto());
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            var id = Guid.NewGuid();
            var user = new UserInfo
            {
                UserId = id,
                FullName = "Connor",
                Address = "Detroit"
            };

            _repoMock.Setup(r => r.GetUserByIdAsync(user.UserId)).ReturnsAsync(user);
            _repoMock.Setup(r => r.DeleteUserAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteUserAsync(id);

            result.Should().BeTrue();

            _repoMock.Verify(r => r.DeleteUserAsync(user), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetUserByIdAsync(id)).ReturnsAsync((UserInfo?)null);

            var result = await _service.DeleteUserAsync(id);

            result.Should().BeFalse();
        }
    }
}
