using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using wedev.Infrastructure;
using wedev.Service.Services.Global;
using wedev.Shared.Global;
using wedev.Domain.Global;

using Xunit;

public class AppServiceTests
{
    private readonly Mock<IDbContextFactory<TestGlobalDbContext>> _dbContextFactoryMock;
    private readonly Mock<TestGlobalDbContext> _dbContextMock;
    private readonly AppService _appService;

    public AppServiceTests()
    {
        _dbContextMock = new Mock<TestGlobalDbContext>(new DbContextOptions<GlobalDbContext>());
        _dbContextFactoryMock = new Mock<IDbContextFactory<TestGlobalDbContext>>();
        _dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(_dbContextMock.Object);

     //   _appService = new AppService(_dbContextFactoryMock.Object);
    }

    [Fact]
    public async Task GetAllAppsAsync_ShouldReturnAllApps()
    {
        // Arrange
        var apps = new List<App> { new App { AppId = Guid.NewGuid(), Name = "TestApp" } };
        _dbContextMock.Setup(db => db.Apps).Returns(apps.ReturnsDbSet());

        // Act
        var result = await _appService.GetAllAppsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("TestApp");
    }

    [Fact]
    public async Task GetAppByIdAsync_ShouldReturnApp_WhenAppExists()
    {
        // Arrange
        var appId = Guid.NewGuid();
        var app = new App { AppId = appId, Name = "TestApp" };
        _dbContextMock.Setup(db => db.Apps).Returns(new List<App> { app }.ReturnsDbSet());

        // Act
        var result = await _appService.GetAppByIdAsync(appId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestApp");
    }

    [Fact]
    public async Task GetAppByIdAsync_ShouldReturnNull_WhenAppDoesNotExist()
    {
        // Arrange
        var appId = Guid.NewGuid();
        _dbContextMock.Setup(db => db.Apps).Returns(new List<App>().ReturnsDbSet());

        // Act
        var result = await _appService.GetAppByIdAsync(appId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAppAsync_ShouldAddApp()
    {
        // Arrange
        var appDto = new AppDto { AppId = Guid.NewGuid(), Name = "NewApp" };
        var apps = new List<App>();
        _dbContextMock.Setup(db => db.Apps).Returns(apps.ReturnsDbSet());

        // Act
        await _appService.CreateAppAsync(appDto, "TestUser");

        // Assert
        _dbContextMock.Verify(db => db.Apps.Add(It.Is<App>(a => a.Name == "NewApp")), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAppAsync_ShouldUpdateApp_WhenAppExists()
    {
        // Arrange
        var appId = Guid.NewGuid();
        var app = new App { AppId = appId, Name = "OldName" };
        _dbContextMock.Setup(db => db.Apps).Returns(new List<App> { app }.ReturnsDbSet());

        var appDto = new AppDto { AppId = appId, Name = "UpdatedName" };

        // Act
        await _appService.UpdateAppAsync(appDto, "TestUser");

        // Assert
        app.Name.Should().Be("UpdatedName");
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAppAsync_ShouldNotUpdate_WhenAppDoesNotExist()
    {
        // Arrange
        var appDto = new AppDto { AppId = Guid.NewGuid(), Name = "NonExistentApp" };
        _dbContextMock.Setup(db => db.Apps).Returns(new List<App>().ReturnsDbSet());

        // Act
        Func<Task> act = async () => await _appService.UpdateAppAsync(appDto, "TestUser");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>("because the app does not exist");
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAppAsync_ShouldRemoveApp_WhenAppExists()
    {
        // Arrange
        var appId = Guid.NewGuid();
        var app = new App { AppId = appId, Name = "AppToDelete" };
        _dbContextMock.Setup(db => db.Apps).Returns(new List<App> { app }.ReturnsDbSet());

        // Act
        await _appService.DeleteAppAsync(appId);

        // Assert
        _dbContextMock.Verify(m => m.Apps.Remove(It.Is<App>(a => a.AppId == appId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAppAsync_ShouldNotRemoveApp_WhenAppDoesNotExist()
    {
        // Arrange
        var appId = Guid.NewGuid();
        _dbContextMock.Setup(db => db.Apps).Returns(new List<App>().ReturnsDbSet());

        // Act
        await _appService.DeleteAppAsync(appId);

        // Assert
        _dbContextMock.Verify(m => m.Apps.Remove(It.IsAny<App>()), Times.Never);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

public static class DbSetMockExtensions
{
    public static Mock<DbSet<T>> CreateMockDbSet<T>(this IEnumerable<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

        dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.ToList().Add);
        dbSetMock.Setup(d => d.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(sourceList.ToList().AddRange);

        return dbSetMock;
    }
    
    public static DbSet<T> ReturnsDbSet<T>(this IEnumerable<T> sourceList) where T : class
    {
        return sourceList.CreateMockDbSet().Object;
    }
}
