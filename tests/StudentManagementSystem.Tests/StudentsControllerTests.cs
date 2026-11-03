using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Api.Controllers;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Tests.Fakes;

namespace StudentManagementSystem.Tests;

public class StudentsControllerTests
{
    [Fact]
    public void GetAll_ReturnsOkWithServiceResult()
    {
        var students = new List<Student> { new() { Id = 1, FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" } };
        var fake = new FakeStudentService { StudentsToReturn = students };
        var controller = new StudentsController(fake);

        var result = controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(students, okResult.Value);
    }

    [Fact]
    public void GetById_ServiceReturnsStudent_ReturnsOk()
    {
        var student = new Student { Id = 1, FirstName = "Alice", LastName = "Doe", Email = "alice@example.com" };
        var fake = new FakeStudentService { StudentToReturn = student };
        var controller = new StudentsController(fake);

        var result = controller.GetById(student.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(student, okResult.Value);
    }

    [Fact]
    public void GetById_ServiceReturnsNull_ReturnsNotFound()
    {
        var fake = new FakeStudentService { StudentToReturn = null };
        var controller = new StudentsController(fake);

        var result = controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_DelegatesToServiceAndReturnsCreated()
    {
        var created = new Student { Id = 5, FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };
        var fake = new FakeStudentService { CreateResultToReturn = created };
        var controller = new StudentsController(fake);
        var newStudent = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };

        var result = controller.Create(newStudent);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Same(created, createdResult.Value);
        Assert.Same(newStudent, fake.LastCreateCall);
    }

    [Fact]
    public void Update_ServiceReturnsTrue_ReturnsNoContent()
    {
        var fake = new FakeStudentService { UpdateResultToReturn = true };
        var controller = new StudentsController(fake);
        var updatedStudent = new Student { FirstName = "Updated", LastName = "Name", Email = "updated@example.com" };

        var result = controller.Update(1, updatedStudent);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal((1, updatedStudent), fake.LastUpdateCall);
    }

    [Fact]
    public void Update_ServiceReturnsFalse_ReturnsNotFound()
    {
        var fake = new FakeStudentService { UpdateResultToReturn = false };
        var controller = new StudentsController(fake);

        var result = controller.Update(999, new Student { FirstName = "Ghost", LastName = "Student", Email = "ghost@example.com" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ServiceReturnsTrue_ReturnsNoContent()
    {
        var fake = new FakeStudentService { DeleteResultToReturn = true };
        var controller = new StudentsController(fake);

        var result = controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, fake.LastDeleteCall);
    }

    [Fact]
    public void Delete_ServiceReturnsFalse_ReturnsNotFound()
    {
        var fake = new FakeStudentService { DeleteResultToReturn = false };
        var controller = new StudentsController(fake);

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
