using StudentManagementSystem.Api.Models;
using StudentManagementSystem.Api.Services;

namespace StudentManagementSystem.Tests.Fakes;

internal class FakeStudentService : IStudentService
{
    public IEnumerable<Student> StudentsToReturn { get; set; } = Enumerable.Empty<Student>();
    public Student? StudentToReturn { get; set; }
    public Student CreateResultToReturn { get; set; } = new();
    public bool UpdateResultToReturn { get; set; }
    public bool DeleteResultToReturn { get; set; }

    public Student? LastCreateCall { get; private set; }
    public (int Id, Student UpdatedStudent)? LastUpdateCall { get; private set; }
    public int? LastDeleteCall { get; private set; }

    public IEnumerable<Student> GetAll() => StudentsToReturn;

    public Student? GetById(int id) => StudentToReturn;

    public Student Create(Student student)
    {
        LastCreateCall = student;
        return CreateResultToReturn;
    }

    public bool Update(int id, Student updatedStudent)
    {
        LastUpdateCall = (id, updatedStudent);
        return UpdateResultToReturn;
    }

    public bool Delete(int id)
    {
        LastDeleteCall = id;
        return DeleteResultToReturn;
    }
}
