using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Controllers;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Tests.Fakes;

namespace StudentManagementSystem.Tests;

public class CoursesControllerTests
{
    [Fact]
    public void GetAll_ReturnsOkWithServiceResult()
    {
        var courses = new List<Course> { new() { Id = 1, Name = "Mathematics", MaxCapacity = 30 } };
        var fake = new FakeCourseService { CoursesToReturn = courses };
        var controller = new CoursesController(fake);

        var result = controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(courses, okResult.Value);
    }

    [Fact]
    public void GetById_ServiceReturnsCourse_ReturnsOk()
    {
        var course = new Course { Id = 1, Name = "Mathematics", MaxCapacity = 30 };
        var fake = new FakeCourseService { CourseToReturn = course };
        var controller = new CoursesController(fake);

        var result = controller.GetById(course.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(course, okResult.Value);
    }

    [Fact]
    public void GetById_ServiceReturnsNull_ReturnsNotFound()
    {
        var fake = new FakeCourseService { CourseToReturn = null };
        var controller = new CoursesController(fake);

        var result = controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_DelegatesToServiceAndReturnsCreated()
    {
        var created = new Course { Id = 5, Name = "Physics", MaxCapacity = 20 };
        var fake = new FakeCourseService { CreateResultToReturn = created };
        var controller = new CoursesController(fake);
        var newCourse = new Course { Name = "Physics", MaxCapacity = 20 };

        var result = controller.Create(newCourse);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Same(created, createdResult.Value);
        Assert.Same(newCourse, fake.LastCreateCall);
    }

    [Fact]
    public void Update_ServiceReturnsTrue_ReturnsNoContent()
    {
        var fake = new FakeCourseService { UpdateResultToReturn = true };
        var controller = new CoursesController(fake);
        var updatedCourse = new Course { Name = "Updated Course", MaxCapacity = 50 };

        var result = controller.Update(1, updatedCourse);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal((1, updatedCourse), fake.LastUpdateCall);
    }

    [Fact]
    public void Update_ServiceReturnsFalse_ReturnsNotFound()
    {
        var fake = new FakeCourseService { UpdateResultToReturn = false };
        var controller = new CoursesController(fake);

        var result = controller.Update(999, new Course { Name = "Ghost Course", MaxCapacity = 10 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ServiceReturnsTrue_ReturnsNoContent()
    {
        var fake = new FakeCourseService { DeleteResultToReturn = true };
        var controller = new CoursesController(fake);

        var result = controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, fake.LastDeleteCall);
    }

    [Fact]
    public void Delete_ServiceReturnsFalse_ReturnsNotFound()
    {
        var fake = new FakeCourseService { DeleteResultToReturn = false };
        var controller = new CoursesController(fake);

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
