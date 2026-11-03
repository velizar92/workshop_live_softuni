using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly AppDbContext _context;

    public EnrollmentService(AppDbContext context)
    {
        _context = context;
    }

    public EnrollResult Enroll(int studentId, int courseId)
    {
        var student = _context.Students.Find(studentId);
        if (student == null)
        {
            return EnrollResult.StudentNotFound();
        }

        var course = _context.Courses.Find(courseId);
        if (course == null)
        {
            return EnrollResult.CourseNotFound();
        }

        var alreadyEnrolled = _context.Enrollments
            .Any(e => e.StudentId == studentId && e.CourseId == courseId);

        if (alreadyEnrolled)
        {
            return EnrollResult.AlreadyEnrolled();
        }

        var enrolledCount = _context.Enrollments.Count(e => e.CourseId == courseId);

        if (enrolledCount >= course.MaxCapacity)
        {
            return EnrollResult.CourseAtCapacity();
        }

        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            EnrollmentDate = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);

        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException)
        {
            return EnrollResult.AlreadyEnrolled();
        }

        return EnrollResult.Created(enrollment);
    }

    public IEnumerable<Course> GetCoursesForStudent(int studentId)
    {
        return _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Select(e => e.Course!)
            .ToList();
    }

    public IEnumerable<Student> GetStudentsForCourse(int courseId)
    {
        return _context.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.Student!)
            .ToList();
    }

    public bool RemoveEnrollment(int studentId, int courseId)
    {
        var enrollment = _context.Enrollments
            .FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);

        if (enrollment == null)
        {
            return false;
        }

        _context.Enrollments.Remove(enrollment);
        _context.SaveChanges();

        return true;
    }
}
