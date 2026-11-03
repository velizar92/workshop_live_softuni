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
    public void GetAll_AvailableOnlyFalse_IncludesFullCourses()
    {
        var context = CreateContext();
        var fullCourse = new Course { Name = "Full Course", MaxCapacity = 1 };
        context.Courses.Add(fullCourse);
        context.SaveChanges();
        context.Students.Add(new Student { FirstName = "A", LastName = "B", Email = "a@b.com" });
        context.SaveChanges();
        context.Enrollments.Add(new Enrollment { StudentId = context.Students.First().Id, CourseId = fullCourse.Id, EnrollmentDate = DateTime.UtcNow });
        context.SaveChanges();
        var controller = new CoursesController(context);

        var result = controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Single(Assert.IsAssignableFrom<IEnumerable<Course>>(okResult.Value));
    }

    [Fact]
    public void GetAll_AvailableOnlyTrue_ExcludesCoursesAtCapacity()
    {
        var context = CreateContext();
        var fullCourse = new Course { Name = "Full Course", MaxCapacity = 1 };
        var availableCourse = new Course { Name = "Available Course", MaxCapacity = 2 };
        context.Courses.AddRange(fullCourse, availableCourse);
        context.SaveChanges();

        var student = new Student { FirstName = "A", LastName = "B", Email = "a@b.com" };
        context.Students.Add(student);
        context.SaveChanges();

        context.Enrollments.Add(new Enrollment { StudentId = student.Id, CourseId = fullCourse.Id, EnrollmentDate = DateTime.UtcNow });
        context.Enrollments.Add(new Enrollment { StudentId = student.Id, CourseId = availableCourse.Id, EnrollmentDate = DateTime.UtcNow });
        context.SaveChanges();

        var controller = new CoursesController(context);

        var result = controller.GetAll(availableOnly: true);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var courses = Assert.IsAssignableFrom<IEnumerable<Course>>(okResult.Value);
        Assert.Equal(new[] { availableCourse.Id }, courses.Select(c => c.Id));
    }

    [Fact]
    public void GetAll_AvailableOnlyTrue_IncludesCoursesWithNoEnrollments()
    {
        var (context, course) = CreateContextWithCourse();
        var controller = new CoursesController(context);

        var result = controller.GetAll(availableOnly: true);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var courses = Assert.IsAssignableFrom<IEnumerable<Course>>(okResult.Value);
        Assert.Equal(new[] { course.Id }, courses.Select(c => c.Id));
    }

    [Fact]
    public void GetById_ExistingCourse_ReturnsCourse()
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
