using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Tests.Fakes;

internal class FakeEnrollmentService : IEnrollmentService
{
    public EnrollResult EnrollResultToReturn { get; set; } = EnrollResult.Created(new Enrollment());
    public bool RemoveResultToReturn { get; set; }
    public IEnumerable<Course> CoursesToReturn { get; set; } = Enumerable.Empty<Course>();
    public IEnumerable<Student> StudentsToReturn { get; set; } = Enumerable.Empty<Student>();

    public (int StudentId, int CourseId)? LastEnrollCall { get; private set; }
    public (int StudentId, int CourseId)? LastRemoveCall { get; private set; }

    public EnrollResult Enroll(int studentId, int courseId)
    {
        LastEnrollCall = (studentId, courseId);
        return EnrollResultToReturn;
    }

    public IEnumerable<Course> GetCoursesForStudent(int studentId) => CoursesToReturn;

    public IEnumerable<Student> GetStudentsForCourse(int courseId) => StudentsToReturn;

    public bool RemoveEnrollment(int studentId, int courseId)
    {
        LastRemoveCall = (studentId, courseId);
        return RemoveResultToReturn;
    }
}
