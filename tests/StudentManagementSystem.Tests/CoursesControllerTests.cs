using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Controllers;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Tests;

public class CoursesControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static (AppDbContext Context, Course Course) CreateContextWithCourse()
    {
        var context = CreateContext();

        var course = new Course { Name = "Mathematics", MaxCapacity = 30 };
        context.Courses.Add(course);
        context.SaveChanges();

        return (context, course);
    }

    [Fact]
    public void GetAll_ReturnsAllCourses()
    {
        var (context, _) = CreateContextWithCourse();
        var controller = new CoursesController(context);

        var result = controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Single(Assert.IsAssignableFrom<IEnumerable<Course>>(okResult.Value));
    }

    [Fact]
    public void GetById_ExistingCourse_ReturnsCourse()
    {
        var (context, course) = CreateContextWithCourse();
        var controller = new CoursesController(context);

        var result = controller.GetById(course.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(course.Id, Assert.IsType<Course>(okResult.Value).Id);
    }

    [Fact]
    public void GetById_NonExistentCourse_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = new CoursesController(context);

        var result = controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_ValidCourse_ReturnsCreated()
    {
        var context = CreateContext();
        var controller = new CoursesController(context);

        var result = controller.Create(new Course { Name = "Physics", MaxCapacity = 20 });

        Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Single(context.Courses);
    }

    [Fact]
    public void Update_ExistingCourse_ReturnsNoContent()
    {
        var (context, course) = CreateContextWithCourse();
        var controller = new CoursesController(context);

        var result = controller.Update(course.Id, new Course { Name = "Updated Course", MaxCapacity = 50 });

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Course", context.Courses.Find(course.Id)!.Name);
    }

    [Fact]
    public void Update_NonExistentCourse_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = new CoursesController(context);

        var result = controller.Update(999, new Course { Name = "Ghost Course", MaxCapacity = 10 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ExistingCourse_ReturnsNoContent()
    {
        var (context, course) = CreateContextWithCourse();
        var controller = new CoursesController(context);

        var result = controller.Delete(course.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Courses);
    }

    [Fact]
    public void Delete_NonExistentCourse_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = new CoursesController(context);

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
