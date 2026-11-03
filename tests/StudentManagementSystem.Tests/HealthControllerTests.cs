using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Controllers;

namespace StudentManagementSystem.Tests;

public class HealthControllerTests
{
    [Fact]
    public void Get_ReturnsOkWithStatusOk()
    {
        var controller = new HealthController();

        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var statusProperty = okResult.Value!.GetType().GetProperty("status");
        Assert.NotNull(statusProperty);
        Assert.Equal("ok", statusProperty.GetValue(okResult.Value));
    }
}
