using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using wedev.Domain.Global;
using wedev.Infrastructure;
using wedev.Service.Services.Global;
using wedev.Shared.Global;

public class TenantServiceTests
{
    private readonly Mock<IDbContextFactory<GlobalDbContext>> _dbContextFactoryMock;
    private readonly Mock<GlobalDbContext> _dbContextMock;
    private readonly TenantService _tenantService;

    public TenantServiceTests()
    {
        _dbContextMock = new Mock<GlobalDbContext>(new DbContextOptions<GlobalDbContext>());
        _dbContextFactoryMock = new Mock<IDbContextFactory<GlobalDbContext>>();
        _dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(_dbContextMock.Object);

        _tenantService = new TenantService(_dbContextFactoryMock.Object);
    }

    [Fact]
    public async Task GetAllTenantsAsync_ShouldReturnAllTenants()
    {
        var tenants = new List<Tenant> { new Tenant { TenantId = Guid.NewGuid(), Name = "TestTenant" } };
        _dbContextMock.Setup(db => db.Tenants).Returns(tenants.ReturnsDbSet());

        var result = await _tenantService.GetAllTenantsAsync();

        result.Should().HaveCount(1);
        result.First().Name.Should().Be("TestTenant");
    }

    [Fact]
    public async Task GetTenantByIdAsync_ShouldReturnTenant_WhenTenantExists()
    {
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { TenantId = tenantId, Name = "TestTenant" };
        _dbContextMock.Setup(db => db.Tenants).Returns(new List<Tenant> { tenant }.ReturnsDbSet());

        var result = await _tenantService.GetTenantByIdAsync(tenantId);

        result.Should().NotBeNull();
        result.Name.Should().Be("TestTenant");
    }

    [Fact]
    public async Task CreateTenantAsync_ShouldAddTenant()
    {
        var tenantDto = new TenantDto { TenantId = Guid.NewGuid(), Name = "NewTenant" };
        var tenantSetMock = new Mock<DbSet<Tenant>>();
        _dbContextMock.Setup(db => db.Tenants).Returns(tenantSetMock.Object);

        await _tenantService.CreateTenantAsync(tenantDto);

        tenantSetMock.Verify(db => db.Add(It.Is<Tenant>(t => t.Name == "NewTenant")), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTenantAsync_ShouldRemoveTenant_WhenTenantExists()
    {
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { TenantId = tenantId, Name = "TenantToDelete" };
        var tenantSetMock = new Mock<DbSet<Tenant>>();
        tenantSetMock.Setup(m => m.FindAsync(tenantId)).ReturnsAsync(tenant);
        _dbContextMock.Setup(db => db.Tenants).Returns(tenantSetMock.Object);

        await _tenantService.DeleteTenantAsync(tenantId);

        tenantSetMock.Verify(m => m.Remove(It.Is<Tenant>(t => t.TenantId == tenantId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
