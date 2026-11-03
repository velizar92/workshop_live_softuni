using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Api.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Course>> GetAll([FromQuery] bool availableOnly = false)
    {
        var query = _context.Courses.AsNoTracking();

        if (availableOnly)
        {
            query = query.Where(c => c.Enrollments.Count < c.MaxCapacity);
        }

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Course> GetById(int id)
    {
        var course = _courseService.GetById(id);
        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    public ActionResult<Course> Create(Course course)
    {
        var created = _courseService.Create(course);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Course updatedCourse)
    {
        return _courseService.Update(id, updatedCourse) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return _courseService.Delete(id) ? NoContent() : NotFound();
    }
}
