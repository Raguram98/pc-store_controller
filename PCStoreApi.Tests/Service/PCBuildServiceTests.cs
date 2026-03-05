using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.Identity.Client;
using Moq;
using PCStoreApi.Application.DTOs.PCBuild;
using PCStoreApi.Application.Interfaces;
using PCStoreApi.Application.Services;
using PCStoreApi.Domain.Entities;
using PCStoreApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PCStoreApi.Tests.Service
{
    public class PCBuildServiceTests
    {
        private readonly Mock<IPCBuildRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PCBuildService _service;

        public PCBuildServiceTests()
        {
            _repoMock = new Mock<IPCBuildRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PCBuildService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateBuildAsync_ShouldReturnMappedDto_WhenBuildCreated()
        {
            var createDto = new PCBuildCreateDto { Processor = "Ryzen 3", RamInGB = 16, Storage = "1TB SSD", UserID = 1 };
            var build = new PCBuild { PCBuildId = 1, Processor = "Ryzen 3", RamInGB = 16, Storage = "1TB SSD", UserID = 1 };
            var readDto = new PCBuildReadDto { PCBuildId = 1, Processor = "Ryzen 3", RamInGB = 16, Storage = "1TB SSD", UserID = 1 };

            _mapperMock.Setup(m => m.Map<PCBuild>(createDto)).Returns(build);
            _repoMock.Setup(r => r.AddBuildAsync(build)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<PCBuildReadDto>(build)).Returns(readDto);

            var result = await _service.CreateBuildAsync(createDto);

            result.Should().NotBeNull();
            result.PCBuildId.Should().Be(1);
            _repoMock.Verify(r => r.AddBuildAsync(build), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllBuildAsync_ShouldReturnMappedList()
        {
            var builds = new List<PCBuild>
            {
                new PCBuild {PCBuildId = 1, Processor = "Ryzen 3", RamInGB = 16, Storage = "1TB SSD", UserID = 1 },
                new PCBuild {PCBuildId = 2, Processor = "Intel i5", RamInGB = 8, Storage = "512GB SSD", UserID = 2 }
            };

            var readDtos = new List<PCBuildReadDto>
            {
                new PCBuildReadDto {PCBuildId = 1, Processor = "Ryzen 3", RamInGB = 16, Storage = "1TB SSD", UserID = 1 },
                new PCBuildReadDto {PCBuildId = 2, Processor = "Intel i5", RamInGB = 8, Storage = "512GB SSD", UserID = 2 }
            };

            _repoMock.Setup(r => r.GetAllBuildsAsync()).ReturnsAsync(builds);
            _mapperMock.Setup(m => m.Map<List<PCBuildReadDto>>(builds)).Returns(readDtos);

            var result = await _service.GetAllBuildsAsync();

            result.Should().HaveCount(2);
            result[0].Processor.Should().Be("Ryzen 3");
            result[1].Processor.Should().Be("Intel i5");
        }

        [Fact]
        public async Task GetBuildByIdAsync_ShouldReturnMappedDto_WhenExists()
        {
            var build = new PCBuild
            {
                PCBuildId = 1,
                Processor = "Ryzen 3",
                RamInGB = 16,
                Storage = "1TB SSD",
                UserID = 1
            };

            var readDto = new PCBuildReadDto
            {
                PCBuildId = 1,
                Processor = "Ryzen 3",
                RamInGB = 16,
                Storage = "1TB SSD",
                UserID = 1
            };

            _repoMock.Setup(r => r.GetBuildByIdAsync(1)).ReturnsAsync(build);
            _mapperMock.Setup(m => m.Map<PCBuildReadDto>(build)).Returns(readDto);

            var result = await _service.GetBuildByIdAsync(1);

            result.Should().NotBeNull();
            result!.Processor.Should().Be("Ryzen 3");
        }

        [Fact]
        public async Task GetBuildByUserIdAsync_ShouldReturnPCBuild_WhenUserExists()
        {
            var userId = 10;
            var build = new PCBuild
            {
                PCBuildId = 1,
                Processor = "Ryzen 7 5800X",
                RamInGB = 32,
                GraphicsCard = "RTX 3080",
                Storage = "2TB NVMe",
                UserID = userId
            };

            _repoMock.Setup(r => r.GetBuildByUserIdAsync(userId))
                .ReturnsAsync(build);

            _mapperMock.Setup(m => m.Map<PCBuildReadDto>(build))
                .Returns(new PCBuildReadDto
                {
                    PCBuildId = build.PCBuildId,
                    Processor = build.Processor,
                    RamInGB = build.RamInGB,
                    GraphicsCard = build.GraphicsCard,
                    Storage = build.Storage,
                    UserID = build.UserID
                });

            var result = await _service.GetBuildByUserIdAsync(userId);

            result.Should().NotBeNull();
            result!.Processor.Should().Be("Ryzen 7 5800X");
            result.UserID.Should().Be(userId);
        }

        [Fact]
        public async Task GetBuildByUserIdAsync_ShouldReturnNull_WhenUserNotFound()
        {
            var userId = 99;
            _repoMock.Setup(r => r.GetBuildByUserIdAsync(userId))
                .ReturnsAsync((PCBuild?)null);

            var result = await _service.GetBuildByUserIdAsync(userId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateBuildAsync_ShouldReturnTrue_WhenBuildExists()
        {
            var existing = new PCBuild
            {
                PCBuildId = 1,
                Processor = "Ryzen 3",
                RamInGB = 16,
                Storage = "1TB SSD",
                UserID = 1
            };
            var updateDto = new PCBuildUpdateDto
            {
                Processor = "Ryzen 5",
                RamInGB = 32,
                Storage = "2TB SSD"
            };

            _repoMock.Setup(r => r.GetBuildByIdAsync(1)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.UpdateBuildAsync(existing)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map(updateDto, existing));

            var result = await _service.UpdateBuildAsync(1, updateDto);

            result.Should().BeTrue();
            _repoMock.Verify(r => r.UpdateBuildAsync(existing), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateBuildAsync_ShouldReturnFalse_WhenBuildNotFound()
        {
            _repoMock.Setup(r => r.GetBuildByIdAsync(1)).ReturnsAsync((PCBuild?)null);

            var result = await _service.UpdateBuildAsync(1, new PCBuildUpdateDto());

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteBuildAsync_ShouldReturnTrue_WhenBuildExists()
        {
            var build = new PCBuild
            {
                PCBuildId = 1,
                Processor = "Ryzen 3",
                RamInGB = 16,
                Storage = "1TB SSD",
                UserID = 1
            };
            _repoMock.Setup(r => r.GetBuildByIdAsync(1)).ReturnsAsync(build);
            _repoMock.Setup(r => r.DeleteBuildAsync(build)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteBuildAsync(1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteBuildAsync_ShouldReturnFalse_WhenBuildNotFound()
        {
            _repoMock.Setup(r => r.GetBuildByIdAsync(1)).ReturnsAsync((PCBuild?)null);
            var result = await _service.DeleteBuildAsync(1);
            result.Should().BeFalse();
        }
    }
}
