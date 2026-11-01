using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public ActionResult<Enrollment> Enroll(EnrollmentRequest request)
    {
        var student = _context.Students.Single(s => s.Id == request.StudentId);
        var course = _context.Courses.Single(c => c.Id == request.CourseId);

        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            EnrollmentDate = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        _context.SaveChanges();

        return StatusCode(StatusCodes.Status201Created, enrollment);
    }

    [HttpGet("students/{studentId}/courses")]
    public ActionResult<IEnumerable<Course>> GetCoursesForStudent(int studentId)
    {
        var courses = _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Select(e => e.Course)
            .ToList();

        return Ok(courses);
    }

    [HttpGet("courses/{courseId}/students")]
    public ActionResult<IEnumerable<Student>> GetStudentsForCourse(int courseId)
    {
        var students = _context.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.Student)
            .ToList();

        return Ok(students);
    }

    [HttpDelete("students/{studentId}/courses/{courseId}")]
    public IActionResult RemoveEnrollment(int studentId, int courseId)
    {
        var enrollment = _context.Enrollments
            .FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);

        if (enrollment == null)
        {
            return NotFound();
        }

        _context.Enrollments.Remove(enrollment);
        _context.SaveChanges();

        return NoContent();
    }
}

public class EnrollmentRequest
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
}
