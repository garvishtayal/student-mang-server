using System.ComponentModel.DataAnnotations.Schema;

namespace student_management.Models
{
    public class Parent
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Guid StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student StudentNavigation { get; set; }
    }
}