using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Api.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int MaxCapacity { get; set; }

    public List<Enrollment> Enrollments { get; set; } = new();
}
