using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Tests.Services;

public class StudentServiceTests
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
        var service = new StudentService(context);

        var students = service.GetAll();

        Assert.Single(students);
    }

    [Fact]
    public void GetById_ExistingStudent_ReturnsStudent()
    {
        var (context, student) = CreateContextWithStudent();
        var service = new StudentService(context);

        var result = service.GetById(student.Id);

        Assert.NotNull(result);
        Assert.Equal(student.Id, result!.Id);
    }

    [Fact]
    public void GetById_NonExistentStudent_ReturnsNull()
    {
        var context = CreateContext();
        var service = new StudentService(context);

        var result = service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public void Create_ValidStudent_PersistsAndReturnsStudent()
    {
        var context = CreateContext();
        var service = new StudentService(context);
        var student = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" };

        var result = service.Create(student);

        Assert.Equal(student.Id, result.Id);
        Assert.Single(context.Students);
    }

    [Fact]
    public void Update_ExistingStudent_UpdatesFieldsAndReturnsTrue()
    {
        var (context, student) = CreateContextWithStudent();
        var service = new StudentService(context);

        var result = service.Update(student.Id, new Student { FirstName = "Updated", LastName = "Name", Email = "updated@example.com" });

        Assert.True(result);
        Assert.Equal("Updated", context.Students.Find(student.Id)!.FirstName);
    }

    [Fact]
    public void Update_NonExistentStudent_ReturnsFalse()
    {
        var context = CreateContext();
        var service = new StudentService(context);

        var result = service.Update(999, new Student { FirstName = "Ghost", LastName = "Student", Email = "ghost@example.com" });

        Assert.False(result);
    }

    [Fact]
    public void Delete_ExistingStudent_RemovesStudentAndReturnsTrue()
    {
        var (context, student) = CreateContextWithStudent();
        var service = new StudentService(context);

        var result = service.Delete(student.Id);

        Assert.True(result);
        Assert.Empty(context.Students);
    }

    [Fact]
    public void Delete_NonExistentStudent_ReturnsFalse()
    {
        var context = CreateContext();
        var service = new StudentService(context);

        var result = service.Delete(999);

        Assert.False(result);
    }
}
