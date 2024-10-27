using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using wedev.Service.Services;
using wedev.Shared.Global;
using wedev.WebApi.Controllers.Global;

public class RoleControllerTests
{
    private readonly Mock<GlobalServices> _globalServicesMock;
    private readonly RoleController _roleController;

    public RoleControllerTests()
    {
        _globalServicesMock = new Mock<GlobalServices>();
        _roleController = new RoleController(_globalServicesMock.Object);
    }

    [Fact]
    public async Task GetAllRoles_ShouldReturnOkWithRoles()
    {
        var tenantId = Guid.NewGuid();
        var roles = new List<RoleDto> { new RoleDto { RoleId = Guid.NewGuid(), Name = "TestRole", TenantId = tenantId } };
        _globalServicesMock.Setup(service => service.GetAllRolesAsync(tenantId)).ReturnsAsync(roles);

        var result = await _roleController.GetAllRoles(tenantId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(roles);
    }

    [Fact]
    public async Task CreateRole_ShouldReturnCreatedAtAction()
    {
        var tenantId = Guid.NewGuid();
        var roleDto = new RoleDto { RoleId = Guid.NewGuid(), Name = "NewRole", TenantId = tenantId };
        _globalServicesMock.Setup(service => service.CreateRoleAsync(roleDto));

        var result = await _roleController.CreateRole(tenantId, roleDto);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be(nameof(_roleController.GetRoleById));
    }

    [Fact]
    public async Task DeleteRole_ShouldReturnNoContent()
    {
        var tenantId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        _globalServicesMock.Setup(service => service.DeleteRoleAsync(roleId)).Returns((Task<bool>)Task.CompletedTask);

        var result = await _roleController.DeleteRole(tenantId, roleId);

        result.Should().BeOfType<NoContentResult>();
    }
}