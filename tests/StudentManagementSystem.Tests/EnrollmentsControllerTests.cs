using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Controllers;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;
using StudentManagementSystem.Tests.Fakes;

namespace StudentManagementSystem.Tests;

public class EnrollmentsControllerTests
{
    [Fact]
    public void Enroll_ServiceReturnsCreated_Returns201WithEnrollmentBody()
    {
        var enrollment = new Enrollment { StudentId = 1, CourseId = 2, EnrollmentDate = DateTime.UtcNow };
        var fake = new FakeEnrollmentService { EnrollResultToReturn = EnrollResult.Created(enrollment) };
        var controller = new EnrollmentsController(fake);

        var result = controller.Enroll(new EnrollmentRequest { StudentId = 1, CourseId = 2 });

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Same(enrollment, objectResult.Value);
    }

    [Fact]
    public void Enroll_ServiceReturnsStudentNotFound_Returns404WithStudentMessage()
    {
        var fake = new FakeEnrollmentService { EnrollResultToReturn = EnrollResult.StudentNotFound() };
        var controller = new EnrollmentsController(fake);

        var result = controller.Enroll(new EnrollmentRequest { StudentId = 999, CourseId = 2 });

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Student 999 does not exist.", notFoundResult.Value);
    }

    [Fact]
    public void Enroll_ServiceReturnsCourseNotFound_Returns404WithCourseMessage()
    {
        var fake = new FakeEnrollmentService { EnrollResultToReturn = EnrollResult.CourseNotFound() };
        var controller = new EnrollmentsController(fake);

        var result = controller.Enroll(new EnrollmentRequest { StudentId = 1, CourseId = 999 });

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Course 999 does not exist.", notFoundResult.Value);
    }

    [Fact]
    public void Enroll_ServiceReturnsAlreadyEnrolled_Returns409WithDuplicateMessage()
    {
        var fake = new FakeEnrollmentService { EnrollResultToReturn = EnrollResult.AlreadyEnrolled() };
        var controller = new EnrollmentsController(fake);

        var result = controller.Enroll(new EnrollmentRequest { StudentId = 1, CourseId = 2 });

        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("Student 1 is already enrolled in course 2.", conflictResult.Value);
    }

    [Fact]
    public void Enroll_ServiceReturnsCourseAtCapacity_Returns409WithCapacityMessage()
    {
        var fake = new FakeEnrollmentService { EnrollResultToReturn = EnrollResult.CourseAtCapacity() };
        var controller = new EnrollmentsController(fake);

        var result = controller.Enroll(new EnrollmentRequest { StudentId = 1, CourseId = 2 });

        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("Course 2 has reached its maximum capacity.", conflictResult.Value);
    }

    [Fact]
    public void Enroll_PassesRequestIdsToService()
    {
        var fake = new FakeEnrollmentService();
        var controller = new EnrollmentsController(fake);

        controller.Enroll(new EnrollmentRequest { StudentId = 7, CourseId = 9 });

        Assert.Equal((7, 9), fake.LastEnrollCall);
    }

    [Fact]
    public void GetCoursesForStudent_ReturnsOkWithServiceResult()
    {
        var courses = new List<Course> { new() { Id = 1, Name = "Mathematics", MaxCapacity = 30 } };
        var fake = new FakeEnrollmentService { CoursesToReturn = courses };
        var controller = new EnrollmentsController(fake);

        var result = controller.GetCoursesForStudent(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(courses, okResult.Value);
    }

    [Fact]
    public void GetStudentsForCourse_ReturnsOkWithServiceResult()
    {
        var students = new List<Student> { new() { Id = 1, FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" } };
        var fake = new FakeEnrollmentService { StudentsToReturn = students };
        var controller = new EnrollmentsController(fake);

        var result = controller.GetStudentsForCourse(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(students, okResult.Value);
    }

    [Fact]
    public void RemoveEnrollment_ServiceReturnsTrue_Returns204NoContent()
    {
        var fake = new FakeEnrollmentService { RemoveResultToReturn = true };
        var controller = new EnrollmentsController(fake);

        var result = controller.RemoveEnrollment(1, 2);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal((1, 2), fake.LastRemoveCall);
    }

    [Fact]
    public void RemoveEnrollment_ServiceReturnsFalse_Returns404NotFound()
    {
        var fake = new FakeEnrollmentService { RemoveResultToReturn = false };
        var controller = new EnrollmentsController(fake);

        var result = controller.RemoveEnrollment(1, 2);

        Assert.IsType<NotFoundResult>(result);
    }
}
