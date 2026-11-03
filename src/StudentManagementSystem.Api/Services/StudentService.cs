using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Student> GetAll()
    {
        return _context.Students.ToList();
    }

    public Student? GetById(int id)
    {
        return _context.Students.Find(id);
    }

    public Student Create(Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();

        return student;
    }

    public bool Update(int id, Student updatedStudent)
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return false;
        }

        student.FirstName = updatedStudent.FirstName;
        student.LastName = updatedStudent.LastName;
        student.Email = updatedStudent.Email;

        _context.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return false;
        }

        _context.Students.Remove(student);
        _context.SaveChanges();

        return true;
    }
}
