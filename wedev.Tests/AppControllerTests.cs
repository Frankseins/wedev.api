using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using wedev.WebApi.Controllers.Global;
using wedev.Service.Services;
using wedev.Shared.Global;
using wedev.WebApi.Controllers.Global;
using Xunit;

public class AppControllerTests
{
    private readonly Mock<GlobalServices> _globalServicesMock;
    private readonly AppController _appController;

    public AppControllerTests()
    {
        _globalServicesMock = new Mock<GlobalServices>();
        _appController = new AppController(_globalServicesMock.Object);
    }

    [Fact]
    public async Task GetAllApps_ShouldReturnOkWithApps()
    {
        // Arrange
        var apps = new List<AppDto> { new AppDto { AppId = Guid.NewGuid(), Name = "TestApp" } };
        _globalServicesMock.Setup(service => service.GetAllAppsAsync()).ReturnsAsync(apps);

        // Act
        var result = await _appController.GetAllApps();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(apps);
    }

    [Fact]
    public async Task GetAppById_ShouldReturnOkWithApp_WhenAppExists()
    {
        var appId = Guid.NewGuid();
        var appDto = new AppDto { AppId = appId, Name = "TestApp" };
        _globalServicesMock.Setup(service => service.GetAppByIdAsync(appId)).ReturnsAsync(appDto);

        var result = await _appController.GetAppById(appId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(appDto);
    }

    [Fact]
    public async Task GetAppById_ShouldReturnNotFound_WhenAppDoesNotExist()
    {
        var appId = Guid.NewGuid();
        _globalServicesMock.Setup(service => service.GetAppByIdAsync(appId)).ReturnsAsync((AppDto)null);

        var result = await _appController.GetAppById(appId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateApp_ShouldReturnCreatedAtAction()
    {
        var appDto = new AppDto { AppId = Guid.NewGuid(), Name = "NewApp" };
        _globalServicesMock.Setup(service => service.CreateAppAsync(appDto, It.IsAny<string>()));

        var result = await _appController.CreateApp(appDto);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be(nameof(_appController.GetAppById));
    }

    [Fact]
    public async Task DeleteApp_ShouldReturnNoContent()
    {
        var appId = Guid.NewGuid();
        _globalServicesMock.Setup(service => service.DeleteAppAsync(appId)).Returns(Task.CompletedTask);

        var result = await _appController.DeleteApp(appId);

        result.Should().BeOfType<NoContentResult>();
    }
}
