using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Api.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    public List<Enrollment> Enrollments { get; set; } = new();
}
