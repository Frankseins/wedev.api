using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using wedev.Service.Services;
using wedev.Shared.Global;
using wedev.WebApi.Controllers.Global;

public class TenantControllerTests
{
    private readonly Mock<GlobalServices> _globalServicesMock;
    private readonly TenantController _tenantController;

    public TenantControllerTests()
    {
        _globalServicesMock = new Mock<GlobalServices>();
        _tenantController = new TenantController(_globalServicesMock.Object);
    }

    [Fact]
    public async Task GetAllTenants_ShouldReturnOkWithTenants()
    {
        var tenants = new List<TenantDto> { new TenantDto { TenantId = Guid.NewGuid(), Name = "TestTenant" } };
        _globalServicesMock.Setup(service => service.GetAllTenantsAsync()).ReturnsAsync(tenants);

        var result = await _tenantController.GetAllTenants();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(tenants);
    }

    [Fact]
    public async Task CreateTenant_ShouldReturnCreatedAtAction()
    {
        var tenantDto = new TenantDto { TenantId = Guid.NewGuid(), Name = "NewTenant" };
        _globalServicesMock.Setup(service => service.CreateTenantAsync(tenantDto));

        var result = await _tenantController.CreateTenant(tenantDto);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be(nameof(_tenantController.GetTenantById));
    }

    [Fact]
    public async Task DeleteTenant_ShouldReturnNoContent()
    {
        var tenantId = Guid.NewGuid();
        _globalServicesMock.Setup(service => service.DeleteTenantAsync(tenantId)).Returns((Task<bool>)Task.CompletedTask);

        var result = await _tenantController.DeleteTenant(tenantId);

        result.Should().BeOfType<NoContentResult>();
    }
}