using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Tests.Services;

public class EnrollmentServiceTests
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
        var service = new EnrollmentService(context);

        var result = service.Enroll(student.Id, course.Id);

        Assert.Equal(EnrollOutcome.Created, result.Outcome);
        Assert.NotNull(result.Enrollment);
        Assert.Equal(student.Id, result.Enrollment!.StudentId);
        Assert.Equal(course.Id, result.Enrollment.CourseId);
        Assert.Single(context.Enrollments);
    }

    [Fact]
    public void Enroll_NonExistentStudentId_ReturnsStudentNotFound()
    {
        var (context, _, course) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);

        var result = service.Enroll(999, course.Id);

        Assert.Equal(EnrollOutcome.StudentNotFound, result.Outcome);
        Assert.Empty(context.Enrollments);
    }

    [Fact]
    public void Enroll_NonExistentCourseId_ReturnsCourseNotFound()
    {
        var (context, student, _) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);

        var result = service.Enroll(student.Id, 999);

        Assert.Equal(EnrollOutcome.CourseNotFound, result.Outcome);
        Assert.Empty(context.Enrollments);
    }

    [Fact]
    public void Enroll_DuplicateStudentAndCourse_ReturnsAlreadyEnrolled()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);

        service.Enroll(student.Id, course.Id);
        var secondResult = service.Enroll(student.Id, course.Id);

        Assert.Equal(EnrollOutcome.AlreadyEnrolled, secondResult.Outcome);
        Assert.Single(context.Enrollments);
    }

    [Fact]
    public void Enroll_SameStudentDifferentCourse_ReturnsCreated()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var otherCourse = new Course { Name = "Computer Science", MaxCapacity = 30 };
        context.Courses.Add(otherCourse);
        context.SaveChanges();

        var service = new EnrollmentService(context);

        service.Enroll(student.Id, course.Id);
        var secondResult = service.Enroll(student.Id, otherCourse.Id);

        Assert.Equal(EnrollOutcome.Created, secondResult.Outcome);
        Assert.Equal(2, context.Enrollments.Count());
    }

    [Fact]
    public void Enroll_DifferentStudentSameCourse_ReturnsCreated()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var otherStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };
        context.Students.Add(otherStudent);
        context.SaveChanges();

        var service = new EnrollmentService(context);

        service.Enroll(student.Id, course.Id);
        var secondResult = service.Enroll(otherStudent.Id, course.Id);

        Assert.Equal(EnrollOutcome.Created, secondResult.Outcome);
        Assert.Equal(2, context.Enrollments.Count());
    }

    [Fact]
    public void Enroll_CourseAtMaxCapacity_ReturnsCourseAtCapacity()
    {
        var context = CreateContext();
        var course = new Course { Name = "Mathematics", MaxCapacity = 1 };
        var firstStudent = new Student { FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" };
        var secondStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };

        context.Courses.Add(course);
        context.Students.AddRange(firstStudent, secondStudent);
        context.SaveChanges();

        var service = new EnrollmentService(context);

        service.Enroll(firstStudent.Id, course.Id);
        var secondResult = service.Enroll(secondStudent.Id, course.Id);

        Assert.Equal(EnrollOutcome.CourseAtCapacity, secondResult.Outcome);
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

        var service = new EnrollmentService(context);

        service.Enroll(firstStudent.Id, course.Id);
        var secondResult = service.Enroll(secondStudent.Id, course.Id);

        Assert.Equal(EnrollOutcome.Created, secondResult.Outcome);
        Assert.Equal(2, context.Enrollments.Count());
    }

    [Fact]
    public void GetCoursesForStudent_StudentWithEnrollments_ReturnsEnrolledCourses()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var otherCourse = new Course { Name = "Computer Science", MaxCapacity = 30 };
        context.Courses.Add(otherCourse);
        context.SaveChanges();

        var service = new EnrollmentService(context);
        service.Enroll(student.Id, course.Id);
        service.Enroll(student.Id, otherCourse.Id);

        var courses = service.GetCoursesForStudent(student.Id).ToList();

        Assert.Equal(2, courses.Count);
        Assert.Contains(courses, c => c.Id == course.Id);
        Assert.Contains(courses, c => c.Id == otherCourse.Id);
    }

    [Fact]
    public void GetCoursesForStudent_StudentWithNoEnrollments_ReturnsEmpty()
    {
        var (context, student, _) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);

        var courses = service.GetCoursesForStudent(student.Id);

        Assert.Empty(courses);
    }

    [Fact]
    public void GetStudentsForCourse_CourseWithEnrollments_ReturnsEnrolledStudents()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var otherStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };
        context.Students.Add(otherStudent);
        context.SaveChanges();

        var service = new EnrollmentService(context);
        service.Enroll(student.Id, course.Id);
        service.Enroll(otherStudent.Id, course.Id);

        var students = service.GetStudentsForCourse(course.Id).ToList();

        Assert.Equal(2, students.Count);
        Assert.Contains(students, s => s.Id == student.Id);
        Assert.Contains(students, s => s.Id == otherStudent.Id);
    }

    [Fact]
    public void GetStudentsForCourse_CourseWithNoEnrollments_ReturnsEmpty()
    {
        var (context, _, course) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);

        var students = service.GetStudentsForCourse(course.Id);

        Assert.Empty(students);
    }

    [Fact]
    public void RemoveEnrollment_ExistingEnrollment_ReturnsTrueAndDeletesRow()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);
        service.Enroll(student.Id, course.Id);

        var removed = service.RemoveEnrollment(student.Id, course.Id);

        Assert.True(removed);
        Assert.Empty(context.Enrollments);
    }

    [Fact]
    public void RemoveEnrollment_NonExistentEnrollment_ReturnsFalse()
    {
        var (context, student, course) = CreateContextWithStudentAndCourse();
        var service = new EnrollmentService(context);

        var removed = service.RemoveEnrollment(student.Id, course.Id);

        Assert.False(removed);
    }
}
