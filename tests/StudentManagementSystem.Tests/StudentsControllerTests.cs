using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Controllers;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Tests;

public class StudentsControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static (AppDbContext Context, Student Student) CreateContextWithStudent()
    {
        var context = CreateContext();

        var student = new Student { FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" };
        context.Students.Add(student);
        context.SaveChanges();

        return (context, student);
    }

    [Fact]
    public void GetAll_ReturnsAllStudents()
    {
        var (context, _) = CreateContextWithStudent();
        var controller = new StudentsController(context);

        var result = controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Single(Assert.IsAssignableFrom<IEnumerable<Student>>(okResult.Value));
    }

    [Fact]
    public void GetById_ExistingStudent_ReturnsStudent()
    {
        var (context, student) = CreateContextWithStudent();
        var controller = new StudentsController(context);

        var result = controller.GetById(student.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(student.Id, Assert.IsType<Student>(okResult.Value).Id);
    }

    [Fact]
    public void GetById_NonExistentStudent_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = new StudentsController(context);

        var result = controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_ValidStudent_ReturnsCreated()
    {
        var context = CreateContext();
        var controller = new StudentsController(context);

        var result = controller.Create(new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" });

        Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Single(context.Students);
    }

    [Fact]
    public void Update_ExistingStudent_ReturnsNoContent()
    {
        var (context, student) = CreateContextWithStudent();
        var controller = new StudentsController(context);

        var result = controller.Update(student.Id, new Student { FirstName = "Updated", LastName = "Name", Email = "updated@example.com" });

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated", context.Students.Find(student.Id)!.FirstName);
    }

    [Fact]
    public void Update_NonExistentStudent_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = new StudentsController(context);

        var result = controller.Update(999, new Student { FirstName = "Ghost", LastName = "Student", Email = "ghost@example.com" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ExistingStudent_ReturnsNoContent()
    {
        var (context, student) = CreateContextWithStudent();
        var controller = new StudentsController(context);

        var result = controller.Delete(student.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Students);
    }

    [Fact]
    public void Delete_NonExistentStudent_ReturnsNotFound()
    {
        var context = CreateContext();
        var controller = new StudentsController(context);

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
