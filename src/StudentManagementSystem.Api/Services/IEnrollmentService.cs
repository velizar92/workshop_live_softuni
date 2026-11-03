using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Services;

public interface IEnrollmentService
{
    EnrollResult Enroll(int studentId, int courseId);
    IEnumerable<Course> GetCoursesForStudent(int studentId);
    IEnumerable<Student> GetStudentsForCourse(int courseId);
    bool RemoveEnrollment(int studentId, int courseId);
}

public enum EnrollOutcome
{
    Created,
    StudentNotFound,
    CourseNotFound,
    AlreadyEnrolled,
    CourseAtCapacity
}

public sealed class EnrollResult
{
    public EnrollOutcome Outcome { get; }
    public Enrollment? Enrollment { get; }

    private EnrollResult(EnrollOutcome outcome, Enrollment? enrollment)
    {
        Outcome = outcome;
        Enrollment = enrollment;
    }

    public static EnrollResult Created(Enrollment enrollment) => new(EnrollOutcome.Created, enrollment);
    public static EnrollResult StudentNotFound() => new(EnrollOutcome.StudentNotFound, null);
    public static EnrollResult CourseNotFound() => new(EnrollOutcome.CourseNotFound, null);
    public static EnrollResult AlreadyEnrolled() => new(EnrollOutcome.AlreadyEnrolled, null);
    public static EnrollResult CourseAtCapacity() => new(EnrollOutcome.CourseAtCapacity, null);
}
