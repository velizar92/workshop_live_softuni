using StudentManagementSystem.Api.Models;

namespace StudentManagementSystem.Api.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Students.Any() || context.Courses.Any())
        {
            return;
        }

        var alice = new Student { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" };
        var bob = new Student { FirstName = "Bob", LastName = "Smith", Email = "bob.smith@example.com" };
        var carol = new Student { FirstName = "Carol", LastName = "Davis", Email = "carol.davis@example.com" };
        var david = new Student { FirstName = "David", LastName = "Wilson", Email = "david.wilson@example.com" };

        context.Students.AddRange(alice, bob, carol, david);

        var mathematics = new Course
        {
            Name = "Mathematics 101",
            Description = "Introductory algebra and calculus concepts.",
            MaxCapacity = 30
        };

        var computerScience = new Course
        {
            Name = "Introduction to Computer Science",
            Description = "Fundamentals of programming and computer science.",
            MaxCapacity = 2
        };

        var historyOfArt = new Course
        {
            Name = "History of Art",
            Description = "A survey of art history from antiquity to modern times.",
            MaxCapacity = 20
        };

        context.Courses.AddRange(mathematics, computerScience, historyOfArt);

        context.SaveChanges();

        context.Enrollments.AddRange(
            new Enrollment { StudentId = alice.Id, CourseId = mathematics.Id, EnrollmentDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
            new Enrollment { StudentId = bob.Id, CourseId = mathematics.Id, EnrollmentDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
            new Enrollment { StudentId = alice.Id, CourseId = computerScience.Id, EnrollmentDate = new DateTime(2026, 1, 13, 0, 0, 0, DateTimeKind.Utc) },
            new Enrollment { StudentId = bob.Id, CourseId = computerScience.Id, EnrollmentDate = new DateTime(2026, 1, 13, 0, 0, 0, DateTimeKind.Utc) },
            new Enrollment { StudentId = carol.Id, CourseId = historyOfArt.Id, EnrollmentDate = new DateTime(2026, 1, 14, 0, 0, 0, DateTimeKind.Utc) }
        );

        context.SaveChanges();
    }
}
