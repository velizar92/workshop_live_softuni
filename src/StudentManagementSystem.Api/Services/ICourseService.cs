using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Services;

public interface ICourseService
{
    IEnumerable<Course> GetAll();
    Course? GetById(int id);
    Course Create(Course course);
    bool Update(int id, Course updatedCourse);
    bool Delete(int id);
}
