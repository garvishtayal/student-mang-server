using System.ComponentModel.DataAnnotations.Schema;

namespace student_management.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}