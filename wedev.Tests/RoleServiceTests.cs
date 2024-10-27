using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using wedev.Domain.Global;
using wedev.Infrastructure;
using wedev.Service.Services.Global;
using wedev.Shared.Global;

public class RoleServiceTests
{
    private readonly Mock<IDbContextFactory<GlobalDbContext>> _dbContextFactoryMock;
    private readonly Mock<GlobalDbContext> _dbContextMock;
    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        _dbContextMock = new Mock<GlobalDbContext>(new DbContextOptions<GlobalDbContext>());
        _dbContextFactoryMock = new Mock<IDbContextFactory<GlobalDbContext>>();
        _dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(_dbContextMock.Object);

        _roleService = new RoleService(_dbContextFactoryMock.Object);
    }

    [Fact]
    public async Task GetAllRolesAsync_ShouldReturnAllRoles()
    {
        var tenantId = Guid.NewGuid();
        var roles = new List<Role> { new Role { RoleId = Guid.NewGuid(), Name = "TestRole", TenantId = tenantId } };
        _dbContextMock.Setup(db => db.Roles).Returns(roles.ReturnsDbSet());

        var result = await _roleService.GetAllRolesAsync(tenantId);

        result.Should().HaveCount(1);
        result.First().Name.Should().Be("TestRole");
    }

    [Fact]
    public async Task GetRoleByIdAsync_ShouldReturnRole_WhenRoleExists()
    {
        var roleId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();
        var role = new Role { RoleId = roleId, Name = "TestRole", TenantId = tenantId };
        _dbContextMock.Setup(db => db.Roles).Returns(new List<Role> { role }.ReturnsDbSet());

        var result = await _roleService.GetRoleByIdAsync(roleId, tenantId);

        result.Should().NotBeNull();
        result.Name.Should().Be("TestRole");
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldAddRole()
    {
        var roleDto = new RoleDto { RoleId = Guid.NewGuid(), Name = "NewRole", TenantId = Guid.NewGuid() };
        var roleSetMock = new Mock<DbSet<Role>>();
        _dbContextMock.Setup(db => db.Roles).Returns(roleSetMock.Object);

        await _roleService.CreateRoleAsync(roleDto);

        roleSetMock.Verify(db => db.Add(It.Is<Role>(r => r.Name == "NewRole")), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleAsync_ShouldUpdateRole_WhenRoleExists()
    {
        var roleId = Guid.NewGuid();
        var role = new Role { RoleId = roleId, Name = "OldName" };
        _dbContextMock.Setup(db => db.Roles).Returns(new List<Role> { role }.ReturnsDbSet());

        var roleDto = new RoleDto { RoleId = roleId, Name = "UpdatedName" };

        await _roleService.UpdateRoleAsync(roleId, roleDto);

        role.Name.Should().Be("UpdatedName");
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleAsync_ShouldRemoveRole_WhenRoleExists()
    {
        var roleId = Guid.NewGuid();
        var role = new Role { RoleId = roleId, Name = "RoleToDelete" };
        var roleSetMock = new Mock<DbSet<Role>>();
        roleSetMock.Setup(m => m.FindAsync(roleId)).ReturnsAsync(role);
        _dbContextMock.Setup(db => db.Roles).Returns(roleSetMock.Object);

        await _roleService.DeleteRoleAsync(roleId);

        roleSetMock.Verify(m => m.Remove(It.Is<Role>(r => r.RoleId == roleId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
