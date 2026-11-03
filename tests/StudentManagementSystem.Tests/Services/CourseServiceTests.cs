using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Tests.Services;

public class CourseServiceTests
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
        var service = new CourseService(context);

        var courses = service.GetAll();

        Assert.Single(courses);
    }

    [Fact]
    public void GetById_ExistingCourse_ReturnsCourse()
    {
        var (context, course) = CreateContextWithCourse();
        var service = new CourseService(context);

        var result = service.GetById(course.Id);

        Assert.NotNull(result);
        Assert.Equal(course.Id, result!.Id);
    }

    [Fact]
    public void GetById_NonExistentCourse_ReturnsNull()
    {
        var context = CreateContext();
        var service = new CourseService(context);

        var result = service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public void Create_ValidCourse_PersistsAndReturnsCourse()
    {
        var context = CreateContext();
        var service = new CourseService(context);
        var course = new Course { Name = "Physics", MaxCapacity = 20 };

        var result = service.Create(course);

        Assert.Equal(course.Id, result.Id);
        Assert.Single(context.Courses);
    }

    [Fact]
    public void Update_ExistingCourse_UpdatesFieldsAndReturnsTrue()
    {
        var (context, course) = CreateContextWithCourse();
        var service = new CourseService(context);

        var result = service.Update(course.Id, new Course { Name = "Updated Course", MaxCapacity = 50 });

        Assert.True(result);
        Assert.Equal("Updated Course", context.Courses.Find(course.Id)!.Name);
    }

    [Fact]
    public void Update_NonExistentCourse_ReturnsFalse()
    {
        var context = CreateContext();
        var service = new CourseService(context);

        var result = service.Update(999, new Course { Name = "Ghost Course", MaxCapacity = 10 });

        Assert.False(result);
    }

    [Fact]
    public void Delete_ExistingCourse_RemovesCourseAndReturnsTrue()
    {
        var (context, course) = CreateContextWithCourse();
        var service = new CourseService(context);

        var result = service.Delete(course.Id);

        Assert.True(result);
        Assert.Empty(context.Courses);
    }

    [Fact]
    public void Delete_NonExistentCourse_ReturnsFalse()
    {
        var context = CreateContext();
        var service = new CourseService(context);

        var result = service.Delete(999);

        Assert.False(result);
    }
}
