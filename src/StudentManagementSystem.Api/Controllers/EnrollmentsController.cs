using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Api.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost]
    public ActionResult<Enrollment> Enroll(EnrollmentRequest request)
    {
        var result = _enrollmentService.Enroll(request.StudentId, request.CourseId);

        return result.Outcome switch
        {
            EnrollOutcome.StudentNotFound => NotFound($"Student {request.StudentId} does not exist."),
            EnrollOutcome.CourseNotFound => NotFound($"Course {request.CourseId} does not exist."),
            EnrollOutcome.AlreadyEnrolled => Conflict($"Student {request.StudentId} is already enrolled in course {request.CourseId}."),
            EnrollOutcome.CourseAtCapacity => Conflict($"Course {request.CourseId} has reached its maximum capacity."),
            _ => StatusCode(StatusCodes.Status201Created, result.Enrollment)
        };
    }

    [HttpGet("students/{studentId}/courses")]
    public ActionResult<IEnumerable<Course>> GetCoursesForStudent(int studentId)
    {
        return Ok(_enrollmentService.GetCoursesForStudent(studentId));
    }

    [HttpGet("courses/{courseId}/students")]
    public ActionResult<IEnumerable<Student>> GetStudentsForCourse(int courseId)
    {
        return Ok(_enrollmentService.GetStudentsForCourse(courseId));
    }

    [HttpDelete("students/{studentId}/courses/{courseId}")]
    public IActionResult RemoveEnrollment(int studentId, int courseId)
    {
        return _enrollmentService.RemoveEnrollment(studentId, courseId) ? NoContent() : NotFound();
    }
}

public class EnrollmentRequest
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
}
