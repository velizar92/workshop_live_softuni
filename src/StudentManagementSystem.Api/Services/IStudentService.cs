using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Services;

public interface IStudentService
{
    IEnumerable<Student> GetAll();
    Student? GetById(int id);
    Student Create(Student student);
    bool Update(int id, Student updatedStudent);
    bool Delete(int id);
}
