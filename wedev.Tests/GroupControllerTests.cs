using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using wedev.Service.Services;
using wedev.Shared.Global;
using wedev.WebApi.Controllers.Global;

public class GroupControllerTests
{
    private readonly Mock<GlobalServices> _globalServicesMock;
    private readonly GroupController _groupController;

    public GroupControllerTests()
    {
        _globalServicesMock = new Mock<GlobalServices>();
        _groupController = new GroupController(_globalServicesMock.Object);
    }

    [Fact]
    public async Task GetAllGroups_ShouldReturnOkWithGroups()
    {
        var tenantId = Guid.NewGuid();
        var groups = new List<GroupDto> { new GroupDto { GroupId = Guid.NewGuid(), Name = "TestGroup", TenantId = tenantId } };
        _globalServicesMock.Setup(service => service.GetAllGroupsAsync(tenantId)).ReturnsAsync(groups);

        var result = await _groupController.GetAllGroups(tenantId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(groups);
    }

    [Fact]
    public async Task CreateGroup_ShouldReturnCreatedAtAction()
    {
        var tenantId = Guid.NewGuid();
        var groupDto = new GroupDto { GroupId = Guid.NewGuid(), Name = "NewGroup", TenantId = tenantId };
        _globalServicesMock.Setup(service => service.CreateGroupAsync(groupDto));

        var result = await _groupController.CreateGroup(tenantId, groupDto);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be(nameof(_groupController.GetGroupById));
    }

    [Fact]
    public async Task UpdateGroup_ShouldReturnNoContent()
    {
        var tenantId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var groupDto = new GroupDto { GroupId = groupId, Name = "UpdatedGroup", TenantId = tenantId };
        _globalServicesMock.Setup(service => service.UpdateGroupAsync(groupId, groupDto));

        var result = await _groupController.UpdateGroup(tenantId, groupId, groupDto);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteGroup_ShouldReturnNoContent()
    {
        var tenantId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        _globalServicesMock.Setup(service => service.DeleteGroupAsync(groupId)).Returns((Task<bool>)Task.CompletedTask);

        var result = await _groupController.DeleteGroup(tenantId, groupId);

        result.Should().BeOfType<NoContentResult>();
    }
}
