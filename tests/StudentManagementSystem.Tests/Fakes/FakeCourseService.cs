using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Tests.Fakes;

internal class FakeCourseService : ICourseService
{
    public IEnumerable<Course> CoursesToReturn { get; set; } = Enumerable.Empty<Course>();
    public Course? CourseToReturn { get; set; }
    public Course CreateResultToReturn { get; set; } = new();
    public bool UpdateResultToReturn { get; set; }
    public bool DeleteResultToReturn { get; set; }

    public Course? LastCreateCall { get; private set; }
    public (int Id, Course UpdatedCourse)? LastUpdateCall { get; private set; }
    public int? LastDeleteCall { get; private set; }

    public IEnumerable<Course> GetAll() => CoursesToReturn;

    public Course? GetById(int id) => CourseToReturn;

    public Course Create(Course course)
    {
        LastCreateCall = course;
        return CreateResultToReturn;
    }

    public bool Update(int id, Course updatedCourse)
    {
        LastUpdateCall = (id, updatedCourse);
        return UpdateResultToReturn;
    }

    public bool Delete(int id)
    {
        LastDeleteCall = id;
        return DeleteResultToReturn;
    }
}
