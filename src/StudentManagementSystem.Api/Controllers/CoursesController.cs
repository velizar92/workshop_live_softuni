using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Course>> GetAll()
    {
        var courses = _context.Courses.ToList();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public ActionResult<Course> GetById(int id)
    {
        var course = _context.Courses.Find(id);
        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    public ActionResult<Course> Create(Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Course updatedCourse)
    {
        var course = _context.Courses.Find(id);
        if (course == null)
        {
            return NotFound();
        }

        course.Name = updatedCourse.Name;
        course.Description = updatedCourse.Description;
        course.MaxCapacity = updatedCourse.MaxCapacity;

        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var course = _context.Courses.Single(c => c.Id == id);

        _context.Courses.Remove(course);
        _context.SaveChanges();

        return NoContent();
    }
}
