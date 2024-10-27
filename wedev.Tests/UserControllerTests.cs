using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using wedev.Service.Services;
using wedev.Shared.Global;
using wedev.WebApi.Controllers.Global;

public class UserControllerTests
{
    private readonly Mock<GlobalServices> _globalServicesMock;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _globalServicesMock = new Mock<GlobalServices>();
        _userController = new UserController(_globalServicesMock.Object);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnOkWithUsers()
    {
        var tenantId = Guid.NewGuid();
        var users = new List<UserDto> { new UserDto { UserId = Guid.NewGuid(), UserName = "TestUser" } };
        _globalServicesMock.Setup(service => service.GetAllUsersByTenantAsync(tenantId)).ReturnsAsync(users);

        var result = await _userController.GetAllUsers(tenantId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedAtAction()
    {
        var tenantId = Guid.NewGuid();
        var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "NewUser" };
        _globalServicesMock.Setup(service => service.CreateUserAsync(userDto, tenantId));

        var result = await _userController.CreateUser(tenantId, userDto);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be(nameof(_userController.GetUserById));
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnNoContent()
    {
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _globalServicesMock.Setup(service => service.DeleteUserAsync(userId)).Returns((Task<bool>)Task.CompletedTask);

        var result = await _userController.DeleteUser(tenantId, userId);

        result.Should().BeOfType<NoContentResult>();
    }
}