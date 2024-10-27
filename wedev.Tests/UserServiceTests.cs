using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using wedev.Domain.Global;
using wedev.Infrastructure;
using wedev.Service.Services.Global;
using wedev.Shared.Global;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IDbContextFactory<GlobalDbContext>> _dbContextFactoryMock;
    private readonly Mock<GlobalDbContext> _dbContextMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _dbContextMock = new Mock<GlobalDbContext>(new DbContextOptions<GlobalDbContext>());
        _dbContextFactoryMock = new Mock<IDbContextFactory<GlobalDbContext>>();
        _dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(_dbContextMock.Object);

        _userService = new UserService(_dbContextFactoryMock.Object);
    }

    [Fact]
    public async Task GetAllUsersByTenantAsync_ShouldReturnAllUsersForTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var users = new List<User>
        {
            new User { UserId = Guid.NewGuid(), UserName = "User1", UserTenants = new List<UserTenant> { new UserTenant { TenantId = tenantId } } },
            new User { UserId = Guid.NewGuid(), UserName = "User2", UserTenants = new List<UserTenant> { new UserTenant { TenantId = tenantId } } }
        };
        _dbContextMock.Setup(db => db.Users).Returns(users.ReturnsDbSet());

        // Act
        var result = await _userService.GetAllUsersByTenantAsync(tenantId);

        // Assert
        result.Should().HaveCount(2);
        result.Select(u => u.UserName).Should().Contain(new[] { "User1", "User2" });
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExistsForTenant()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();
        var user = new User
        {
            UserId = userId,
            UserName = "TestUser",
            UserTenants = new List<UserTenant> { new UserTenant { TenantId = tenantId } }
        };
        _dbContextMock.Setup(db => db.Users).Returns(new List<User> { user }.ReturnsDbSet());

        // Act
        var result = await _userService.GetUserByIdAsync(userId, tenantId);

        // Assert
        result.Should().NotBeNull();
        result.UserName.Should().Be("TestUser");
    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddUser()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "NewUser", Email = "newuser@example.com" };
        var userSetMock = new Mock<DbSet<User>>();
        _dbContextMock.Setup(db => db.Users).Returns(userSetMock.Object);

        // Act
        await _userService.CreateUserAsync(userDto, tenantId);

        // Assert
        userSetMock.Verify(db => db.Add(It.Is<User>(u => u.UserName == "NewUser" && u.Email == "newuser@example.com")), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { UserId = userId, UserName = "OldName", Email = "old@example.com" };
        _dbContextMock.Setup(db => db.Users).Returns(new List<User> { user }.ReturnsDbSet());

        var userDto = new UserDto { UserId = userId, UserName = "UpdatedName", Email = "updated@example.com" };

        // Act
        await _userService.UpdateUserAsync(userId, userDto);

        // Assert
        user.UserName.Should().Be("UpdatedName");
        user.Email.Should().Be("updated@example.com");
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldNotUpdate_WhenUserDoesNotExist()
    {
        // Arrange
        var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "NonExistentUser" };
        _dbContextMock.Setup(db => db.Users).Returns(new List<User>().ReturnsDbSet());

        // Act
        Func<Task> act = async () => await _userService.UpdateUserAsync(userDto.UserId, userDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>("because the user does not exist");
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldRemoveUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { UserId = userId, UserName = "UserToDelete" };
        var userSetMock = new Mock<DbSet<User>>();
        userSetMock.Setup(m => m.FindAsync(userId)).ReturnsAsync(user);
        _dbContextMock.Setup(db => db.Users).Returns(userSetMock.Object);

        // Act
        await _userService.DeleteUserAsync(userId);

        // Assert
        userSetMock.Verify(m => m.Remove(It.Is<User>(u => u.UserId == userId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldNotRemoveUser_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userSetMock = new Mock<DbSet<User>>();
        userSetMock.Setup(m => m.FindAsync(userId)).ReturnsAsync((User)null);
        _dbContextMock.Setup(db => db.Users).Returns(userSetMock.Object);

        // Act
        await _userService.DeleteUserAsync(userId);

        // Assert
        userSetMock.Verify(m => m.Remove(It.IsAny<User>()), Times.Never);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
