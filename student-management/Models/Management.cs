namespace student_management.Models
{
    public class Management
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}