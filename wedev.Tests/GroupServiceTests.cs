using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using wedev.Infrastructure;
using wedev.Service.Services.Global;
using wedev.Shared.Global;
using wedev.Domain.Global;
using Xunit;

public class GroupServiceTests
{
    private readonly Mock<IDbContextFactory<GlobalDbContext>> _dbContextFactoryMock;
    private readonly Mock<GlobalDbContext> _dbContextMock;
    private readonly GroupService _groupService;

    public GroupServiceTests()
    {
        _dbContextMock = new Mock<GlobalDbContext>(new DbContextOptions<GlobalDbContext>());
        _dbContextFactoryMock = new Mock<IDbContextFactory<GlobalDbContext>>();
        _dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(_dbContextMock.Object);

        _groupService = new GroupService(_dbContextFactoryMock.Object);
    }

    [Fact]
    public async Task GetAllGroupsAsync_ShouldReturnAllGroups()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var groups = new List<Group> { new Group { GroupId = Guid.NewGuid(), Name = "TestGroup", TenantId = tenantId } };
        _dbContextMock.Setup(db => db.Groups).Returns(groups.ReturnsDbSet());

        // Act
        var result = await _groupService.GetAllGroupsAsync(tenantId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("TestGroup");
    }

    [Fact]
    public async Task GetGroupByIdAsync_ShouldReturnGroup_WhenGroupExists()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();
        var group = new Group { GroupId = groupId, Name = "TestGroup", TenantId = tenantId };
        _dbContextMock.Setup(db => db.Groups).Returns(new List<Group> { group }.ReturnsDbSet());

        // Act
        var result = await _groupService.GetGroupByIdAsync(groupId, tenantId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestGroup");
    }

    [Fact]
    public async Task CreateGroupAsync_ShouldAddGroup()
    {
        // Arrange
        var groupDto = new GroupDto { GroupId = Guid.NewGuid(), Name = "NewGroup", TenantId = Guid.NewGuid() };
        var groupSetMock = new Mock<DbSet<Group>>();
        _dbContextMock.Setup(db => db.Groups).Returns(groupSetMock.Object);

        // Act
        await _groupService.CreateGroupAsync(groupDto);

        // Assert
        groupSetMock.Verify(db => db.Add(It.Is<Group>(g => g.Name == "NewGroup")), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateGroupAsync_ShouldUpdateGroup_WhenGroupExists()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Group { GroupId = groupId, Name = "OldName" };
        _dbContextMock.Setup(db => db.Groups).Returns(new List<Group> { group }.ReturnsDbSet());

        var groupDto = new GroupDto { GroupId = groupId, Name = "UpdatedName" };

        // Act
        await _groupService.UpdateGroupAsync(groupId, groupDto);

        // Assert
        group.Name.Should().Be("UpdatedName");
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteGroupAsync_ShouldRemoveGroup_WhenGroupExists()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Group { GroupId = groupId, Name = "GroupToDelete" };
        var groupSetMock = new Mock<DbSet<Group>>();
        groupSetMock.Setup(m => m.FindAsync(groupId)).ReturnsAsync(group);
        _dbContextMock.Setup(db => db.Groups).Returns(groupSetMock.Object);

        // Act
        await _groupService.DeleteGroupAsync(groupId);

        // Assert
        groupSetMock.Verify(m => m.Remove(It.Is<Group>(g => g.GroupId == groupId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
