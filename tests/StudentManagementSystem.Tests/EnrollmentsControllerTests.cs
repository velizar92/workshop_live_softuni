using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Controllers;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Tests;

public class EnrollmentsControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static (AppDbContext Context, Student Student, Course Course) CreateContextWithStudentAndCourse()
    {
        var context = CreateContext();

        var student = new Student { FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" };
        var course = new Course { Name = "Mathematics", MaxCapacity = 30 };

        context.Students.Add(student);
        context.Courses.Add(course);
        context.SaveChanges();

        return (context, student, course);
    }

    [Fact]
    public void Enroll_NewStudentAndCourse_ReturnsCreated()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var controller = new EnrollmentsController(context);

        var result = controller.Enroll(new EnrollmentRequest { StudentId = student.Id, CourseId = course.Id });

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Single(context.Enrollments);
    }

    [Fact]
    public void Enroll_DuplicateStudentAndCourse_ReturnsConflict()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var controller = new EnrollmentsController(context);

        controller.Enroll(new EnrollmentRequest { StudentId = student.Id, CourseId = course.Id });
        var secondResult = controller.Enroll(new EnrollmentRequest { StudentId = student.Id, CourseId = course.Id });

        Assert.IsType<ConflictObjectResult>(secondResult.Result);
        Assert.Single(context.Enrollments);
    }

    [Fact]
    public void Enroll_SameStudentDifferentCourse_ReturnsCreated()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var otherCourse = new Course { Name = "Computer Science", MaxCapacity = 30 };
        context.Courses.Add(otherCourse);
        context.SaveChanges();

        var controller = new EnrollmentsController(context);

        controller.Enroll(new EnrollmentRequest { StudentId = student.Id, CourseId = course.Id });
        var secondResult = controller.Enroll(new EnrollmentRequest { StudentId = student.Id, CourseId = otherCourse.Id });

        var objectResult = Assert.IsType<ObjectResult>(secondResult.Result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Equal(2, context.Enrollments.Count());
    }

    [Fact]
    public void Enroll_DifferentStudentSameCourse_ReturnsCreated()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var otherStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };
        context.Students.Add(otherStudent);
        context.SaveChanges();

        var controller = new EnrollmentsController(context);

        controller.Enroll(new EnrollmentRequest { StudentId = student.Id, CourseId = course.Id });
        var secondResult = controller.Enroll(new EnrollmentRequest { StudentId = otherStudent.Id, CourseId = course.Id });

        var objectResult = Assert.IsType<ObjectResult>(secondResult.Result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Equal(2, context.Enrollments.Count());
    }

    [Fact]
    public void Enroll_CourseAtMaxCapacity_ReturnsConflict()
    {
        var context = CreateContext();
        var course = new Course { Name = "Mathematics", MaxCapacity = 1 };
        var firstStudent = new Student { FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" };
        var secondStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };

        context.Courses.Add(course);
        context.Students.AddRange(firstStudent, secondStudent);
        context.SaveChanges();

        var controller = new EnrollmentsController(context);

        controller.Enroll(new EnrollmentRequest { StudentId = firstStudent.Id, CourseId = course.Id });
        var secondResult = controller.Enroll(new EnrollmentRequest { StudentId = secondStudent.Id, CourseId = course.Id });

        Assert.IsType<ConflictObjectResult>(secondResult.Result);
        Assert.Single(context.Enrollments);
    }

    [Fact]
    public void Enroll_CourseWithAvailableCapacity_ReturnsCreated()
    {
        var context = CreateContext();
        var course = new Course { Name = "Mathematics", MaxCapacity = 2 };
        var firstStudent = new Student { FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" };
        var secondStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };

        context.Courses.Add(course);
        context.Students.AddRange(firstStudent, secondStudent);
        context.SaveChanges();

        var controller = new EnrollmentsController(context);

        controller.Enroll(new EnrollmentRequest { StudentId = firstStudent.Id, CourseId = course.Id });
        var secondResult = controller.Enroll(new EnrollmentRequest { StudentId = secondStudent.Id, CourseId = course.Id });

        var objectResult = Assert.IsType<ObjectResult>(secondResult.Result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Equal(2, context.Enrollments.Count());
    }
}
