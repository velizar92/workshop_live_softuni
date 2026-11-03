using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _context;

    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Course> GetAll()
    {
        return _context.Courses.ToList();
    }

    public Course? GetById(int id)
    {
        return _context.Courses.Find(id);
    }

    public Course Create(Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChanges();

        return course;
    }

    public bool Update(int id, Course updatedCourse)
    {
        var course = _context.Courses.Find(id);
        if (course == null)
        {
            return false;
        }

        course.Name = updatedCourse.Name;
        course.Description = updatedCourse.Description;
        course.MaxCapacity = updatedCourse.MaxCapacity;

        _context.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var course = _context.Courses.Find(id);
        if (course == null)
        {
            return false;
        }

        _context.Courses.Remove(course);
        _context.SaveChanges();

        return true;
    }
}
